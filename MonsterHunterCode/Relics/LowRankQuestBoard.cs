using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Basic;
using MonsterHunter.MonsterHunterCode.Cards.Oil;
using MonsterHunter.MonsterHunterCode.Cards.Quest;

namespace MonsterHunter.MonsterHunterCode.Relics;


public class LowRankQuestBoard() : MonsterHunterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

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
}