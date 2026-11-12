using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class BlastCharge() : MonsterHunterCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar)new CalculationBaseVar(12M),
        (DynamicVar)new ExtraDamageVar(4M),
        (DynamicVar)new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, _) =>
                (CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>()
                    .Count(e => e.Power is BlastPower && e.Amount < 0))))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.CalculatedDamage)
            .FromCard((CardModel)this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_starry_impact")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.ExtraDamage.UpgradeValueBy(2M);
}