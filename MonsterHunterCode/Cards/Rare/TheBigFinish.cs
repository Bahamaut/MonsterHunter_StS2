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

public class TheBigFinish() : MonsterHunterCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar)new CalculationBaseVar(8M),
        (DynamicVar)new ExtraDamageVar(2M),
        (DynamicVar)new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, _) =>
                (CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>()
                    .Count(e => e.CardSource != null && 
                                (e.CardSource.DynamicVars.ContainsKey("Combo") ||
                                 e.CardSource.DynamicVars.ContainsKey("Finisher"))))))
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

    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(1M);
}