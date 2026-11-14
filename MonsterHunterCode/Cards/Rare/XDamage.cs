using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class XDamage() : MonsterHunterCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal =>
        CombatState != null && CombatState.HittableEnemies.Any(e =>
            e.Monster != null && e.Monster.IntendsToAttack);

    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar)new CalculationBaseVar(8M),
        (DynamicVar)new ExtraDamageVar(3M),
        (DynamicVar)new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, creature) =>
                creature != null && creature.Monster != null && creature.Monster.IntendsToAttack ? 1 : 0))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var attackAmount = ResolveEnergyXValue();
        var damageAmount = cardPlay.Target.Monster != null && cardPlay.Target.Monster.IntendsToAttack
            ? DynamicVars.CalculationBase.BaseValue + DynamicVars.ExtraDamage.BaseValue
            : DynamicVars.CalculationBase.BaseValue;
        AttackCommand attackCommand = await DamageCmd.Attack(damageAmount)
            .WithHitCount(attackAmount).FromCard((CardModel)this, cardPlay).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_giant_horizontal_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(1M);
}