using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers.Quest;
using MonsterHunter.MonsterHunterCode.Relics;

namespace MonsterHunter.MonsterHunterCode.Cards.Quest;

[Pool(typeof(TokenCardPool))]
public class LowRankGatheringQuest() : MonsterHunterCard(-1,
    CardType.None, CardRarity.None,
    TargetType.Self), LowRankQuestBoard.IChoosable
{
    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LowRankGatheringQuestPower>(1M)];

    public async Task OnChosen(PlayerChoiceContext choiceContext)
    {
        LowRankGatheringQuestPower lowRankGatheringQuestPower = await PowerCmd.Apply<LowRankGatheringQuestPower>(choiceContext, this.Owner.Creature,
            this.DynamicVars[nameof(LowRankGatheringQuestPower)].BaseValue, this.Owner.Creature, (CardModel)this);
    }
}