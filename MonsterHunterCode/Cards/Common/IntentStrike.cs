using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Common;

public class IntentStrike() : MonsterHunterCard(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return this.CombatState != null && this.CombatState.HittableEnemies.Any<Creature>((Func<Creature, bool>) (e =>
            {
                MonsterModel monster = e.Monster;
                return monster != null && monster.IntendsToAttack;
            }));
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        (DynamicVar)new CalculationBaseVar(9M),
        (DynamicVar)new ExtraDamageVar(9M),
        (DynamicVar)new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, _) =>
                card.CurrentTarget != null && card.CurrentTarget.Monster.IntendsToAttack ? 1 : 0))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.CalculatedDamage).FromCard((CardModel) this, play).Targeting(play.Target).WithHitFx("vfx/vfx_starry_impact").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.ExtraDamage.UpgradeValueBy(2M);
        this.DynamicVars.CalculationBase.UpgradeValueBy(2M);
    }
}