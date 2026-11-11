using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class Dismantle() : MonsterHunterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return this.CombatState != null && this.CombatState.HittableEnemies.Any<Creature>(
                (Func<Creature, bool>)(e =>
                {
                    MonsterModel monster = e.Monster;
                    return monster != null && monster.IntendsToAttack;
                }));
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8M, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        int hitCount = cardPlay.Target.Monster.IntendsToAttack ? 2 : 1;
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard((CardModel)this, cardPlay).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}