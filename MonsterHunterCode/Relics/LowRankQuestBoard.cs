using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Basic;
using MonsterHunter.MonsterHunterCode.Cards.Oil;
using MonsterHunter.MonsterHunterCode.Cards.Quest;

namespace MonsterHunter.MonsterHunterCode.Relics;


public class LowRankQuestBoard() : MonsterHunterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    public bool _captureQuestComplete;

    [SavedProperty]
    public bool CaptureQuestComplete
    {
        get => this._captureQuestComplete;
        set
        {
            this.AssertMutable();
            this._captureQuestComplete = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<LowRankCaptureQuest>(),
        HoverTipFactory.FromCard<LowRankSlayQuest>(),
        HoverTipFactory.FromCard<LowRankGatheringQuest>()
    ];

    public static readonly IReadOnlyList<IChoosable> quests =
        new ReadOnlyCollection<IChoosable>(
        [
            ModelDb.Card<LowRankCaptureQuest>(),
            ModelDb.Card<LowRankSlayQuest>(),
            ModelDb.Card<LowRankGatheringQuest>(),
        ]);

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber !=1)
            return;
        this.Flash();

        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext,
            (IReadOnlyList<CardModel>)quests.Select<IChoosable, CardModel>((Func<IChoosable, CardModel>)(
                c =>
                {
                    CardModel card = combatState.CreateCard((CardModel)c, player);
                    return card;
                })).ToList(), this.Owner);
        if (cardModel == null)
            return;
        await ((IChoosable)cardModel).OnChosen(choiceContext);
    }

    public interface IChoosable
    {
        Task OnChosen(PlayerChoiceContext choiceContext);
    }
    
    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (!(this.Owner.RunState.CurrentRoom is CombatRoom) || player != this.Owner || !this.CaptureQuestComplete)
            return false;
        var rewardToUpgrade = cardRewards.FirstOrDefault(cr => !cr.Card.IsUpgraded);

        if (rewardToUpgrade == null)
            return false;
        
        CardModel card1 = rewardToUpgrade.Card;
        if (card1.IsUpgradable)
        {
            CardModel card2 = this.Owner.RunState.CloneCard(card1);
            CardCmd.Upgrade(card2);
            rewardToUpgrade.ModifyCard(card2, (RelicModel) this);
        }
        CaptureQuestComplete = false;
        return true;
    }
}