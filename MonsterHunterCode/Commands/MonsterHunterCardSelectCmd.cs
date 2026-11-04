using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Commands;

public class MonsterHunterCardSelectCmd()
{
    public static async Task<CardModel?> FromChooseACardScreen(
    PlayerChoiceContext context,
    IReadOnlyList<CardModel> cards,
    Player player,
    bool canSkip = false)
  {
    if (cards.Count > 4)
      throw new ArgumentException("Only works with less than 3 cards", nameof (cards));
    if (cards.Count == 0)
    {
      CardSelectCmd.ReportSoftlock();
      return (CardModel) null;
    }
    CardSelectCmd.UndoEndTurnIfNecessary(player);
    CardModel result;
    if (CardSelectCmd.Selector != null)
    {
      result = (await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cards, 0, 1)).FirstOrDefault<CardModel>();
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          result = (await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cards, 0, 1)).FirstOrDefault<CardModel>();
        }
        else
        {
          NPlayerHand.Instance?.CancelAllCardPlay();
          NChooseACardSelectionScreen acardSelectionScreen = NChooseACardSelectionScreen.ShowScreen(cards, canSkip);
          if (LocalContext.IsMe(player))
          {
            foreach (CardModel card in (IEnumerable<CardModel>) cards)
              SaveManager.Instance.MarkCardAsSeen(card);
          }
          result = (await acardSelectionScreen.CardsSelected()).FirstOrDefault<CardModel>();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndex(new int?(cards.IndexOf<CardModel>(result))));
        }
      }
      else
      {
        int index = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
        result = index < 0 ? (CardModel) null : cards[index];
      }
      await context.SignalPlayerChoiceEnded();
    }
    // ISSUE: object of a compiler-generated type is created
    CardSelectCmd.LogChoice(player, new List<CardModel>{result});
    return result;
  }
}