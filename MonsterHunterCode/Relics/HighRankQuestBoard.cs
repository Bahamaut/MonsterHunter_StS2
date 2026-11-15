using System.Collections.ObjectModel;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MonsterHunter.MonsterHunterCode.Cards.Quest;
using MonsterHunter.MonsterHunterCode.Relics;

namespace MonsterHunter.MonsterHunterCode.Relics;

public class HighRankQuestBoard() : MonsterHunterRelic
{
   public override RelicRarity Rarity =>
        RelicRarity.Starter;

   private bool _captureQuestComplete;

    [SavedProperty]
    private bool CaptureQuestComplete
    {
        get => _captureQuestComplete;
        set
        {
            AssertMutable();
            _captureQuestComplete = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<HighRankCaptureQuest>(),
        HoverTipFactory.FromCard<HighRankSlayQuest>(),
        HoverTipFactory.FromCard<HighRankGatheringQuest>()
    ];

    private static IReadOnlyList<IChoosable> Quests => [
            ModelDb.Card<HighRankCaptureQuest>(),
            ModelDb.Card<HighRankSlayQuest>(),
            ModelDb.Card<HighRankGatheringQuest>(),
        ];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (Owner.PlayerCombatState != null && (player != Owner || Owner.PlayerCombatState.TurnNumber !=1))
            return;
        
        Flash();
        var cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext,
            Quests.Select((Func<IChoosable, CardModel>)(
                c => combatState.CreateCard((CardModel)c, player))).ToList(), Owner);
        if (cardModel == null)
            return;
        
        await ((IChoosable)cardModel).OnChosen(choiceContext);
    }

    public interface IChoosable
    {
        Task OnChosen(PlayerChoiceContext choiceContext);
    }
}