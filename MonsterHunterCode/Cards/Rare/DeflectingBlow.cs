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

public class DeflectingBlow() : MonsterHunterCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal =>
        this.CombatState != null && this.CombatState.HittableEnemies.Any<Creature>((Func<Creature, bool>)(e =>
        {
            MonsterModel monster = e.Monster;
            return monster != null && monster.IntendsToAttack;
        }));

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(18M, ValueProp.Move),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await CreatureCmd.GainBlock(Owner.Creature,
            (await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext)).Results
            .SelectMany(r => r)
            .Sum((Func<DamageResult, int>)(r =>
                cardPlay.Target.Monster.IntendsToAttack
                    ? r.TotalDamage + r.OverkillDamage
                    : (int)Math.Ceiling((decimal)(r.TotalDamage + r.OverkillDamage) / 2))), 
            ValueProp.Move, cardPlay);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(6M);
}