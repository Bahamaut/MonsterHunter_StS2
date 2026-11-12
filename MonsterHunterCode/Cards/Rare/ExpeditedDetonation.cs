using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class ExpeditedDetonation() : MonsterHunterCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(0)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        for (int i = 0; i <= DynamicVars.Repeat.BaseValue; i++)
        {
            var blastBlightAmount = play.Target.HasPower<BlastBlightPower>()
                ? play.Target.Powers.First(x => x is BlastBlightPower).Amount
                : 1;
            
            var blastAmount = play.Target.HasPower<BlastPower>()
                ? play.Target.Powers.First(x => x is BlastPower).Amount
                : 0;
            
            MonsterHunterBlastCmd.HandleBlast(choiceContext, play.Target, blastBlightAmount-blastAmount,play.Player.Creature, this);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}