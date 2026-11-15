using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class IntentOneTwo() : MonsterHunterCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BurstPower>(1M),
        new PowerVar<OneTwoPunchPower>(1M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        if (cardPlay.Target.Monster == null)
            return;
        
        if (cardPlay.Target.Monster.IntendsToAttack)
            await PowerCmd.Apply<BurstPower>(choiceContext, Owner.Creature, 
                DynamicVars.Power<BurstPower>().BaseValue, Owner.Creature, this);
        else
            await PowerCmd.Apply<OneTwoPunchPower>(choiceContext, Owner.Creature, 
                DynamicVars.Power<OneTwoPunchPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}