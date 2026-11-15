using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class ExpeditedDetonation() : MonsterHunterCard(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        for (var i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            var blastBlightAmount = cardPlay.Target.HasPower<BlastBlightPower>()
                ? cardPlay.Target.Powers.First(x => x is BlastBlightPower).Amount
                : 1;
            
            var blastAmount = cardPlay.Target.HasPower<BlastPower>()
                ? cardPlay.Target.Powers.First(x => x is BlastPower).Amount
                : 0;
            
            await MonsterHunterBlastCmd.HandleBlast(choiceContext, cardPlay.Target, blastBlightAmount-blastAmount,cardPlay.Player.Creature, this);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Repeat.UpgradeValueBy(1M);
}