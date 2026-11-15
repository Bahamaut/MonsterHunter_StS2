using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MonsterHunter.MonsterHunterCode.Commands;

public class MonsterHunterCardSelectCmd
{
    public static async Task<CardModel?> FromChooseACardScreen(PlayerChoiceContext context,
    IReadOnlyList<CardModel> cards, Player player, bool canSkip = false)
  {
    if (cards.Count > 4)
      throw new ArgumentException("Only works with less than 4 cards", nameof (cards));
    if (cards.Count == 0)
    {
      CardSelectCmd.ReportSoftlock();
      return null;
    }
    CardSelectCmd.UndoEndTurnIfNecessary(player);
    var selector = RunManager.Instance.CardSelectorStack.GetSelector(player);
    CardModel? result = null;
    if (selector != null)
    {
      result = (await selector.GetSelectedCards(cards, 0, 1)).FirstOrDefault();
    }
    else
    {
      var choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        var localSelector = RunManager.Instance.CardSelectorStack.LocalSelector;
        if (localSelector != null)
        {
          result = (await localSelector.GetSelectedCards(cards, 0, 1)).FirstOrDefault();
        }
        else
        {
          NPlayerHand.Instance?.CancelAllCardPlay();
          var acardSelectionScreen = NChooseACardSelectionScreen.ShowScreen(cards, canSkip);
          if (LocalContext.IsMe(player))
          {
            foreach (var card in cards)
              SaveManager.Instance.MarkCardAsSeen(card);
          }

          if (acardSelectionScreen != null)
            result = (await acardSelectionScreen.CardsSelected()).FirstOrDefault();
          
          if(result != null)
            RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndex(cards.IndexOf(result)));
        }
      }
      else
      {
        var index = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
        result = index < 0 ? null : cards[index];
      }
      await context.SignalPlayerChoiceEnded();
    }
    // ISSUE: object of a compiler-generated type is created
    if(result != null)
      CardSelectCmd.LogChoice(player, new List<CardModel>{result});
    return result;
  }
}