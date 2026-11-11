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

public class Spite() : MonsterHunterCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return this.CombatState != null && this.CombatState.HittableEnemies.Any(
                e => {
                    var monster = e.Monster;
                    return monster != null && monster.IntendsToAttack;
                });
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5M, ValueProp.Move),
        new RepeatVar(2)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        int hitCount = cardPlay.Target.Monster.IntendsToAttack ? this.DynamicVars.Repeat.IntValue : 1;
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(hitCount)
            .FromCard((CardModel)this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}