using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Cards.Common;

public class GoForTheHead() : MonsterHunterCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal =>
        CombatState != null && CombatState.HittableEnemies.Any(e =>
            e.Monster != null && e.Monster.IntendsToAttack);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3M, ValueProp.Move),
        new PowerVar<VulnerablePower>(1M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
        if (cardPlay.Target.Monster != null && !cardPlay.Target.Monster.IntendsToAttack)
            return;

        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,
            DynamicVars.Vulnerable.BaseValue,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1M);
        DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}