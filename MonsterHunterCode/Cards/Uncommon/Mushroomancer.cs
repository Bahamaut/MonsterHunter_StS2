using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class Mushroomancer() : MonsterHunterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar)new CalculationBaseVar(8M),
        (DynamicVar)new ExtraDamageVar(2M),
        (DynamicVar)new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, _) =>
                (Decimal)card.Owner.PlayerCombatState.AllCards.Count<CardModel>(
                    (Func<CardModel, bool>)(c => c.DynamicVars.ContainsKey("Consumable")))))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.CalculatedDamage).FromCard((CardModel) this, play).Targeting(play.Target).WithHitFx("vfx/vfx_starry_impact").Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.ExtraDamage.UpgradeValueBy(1M);
}