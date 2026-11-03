using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class ComboMasterPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {

        if (!participants.Contains<Creature>(this.Owner))
            return;
        foreach (CardModel card in PileType.Draw.GetPile(this.Owner.Player).Cards
                     .Where<CardModel>((Func<CardModel, bool>)(c => c.DynamicVars.ContainsKey("Combo")))
                     .ToList<CardModel>().UnstableShuffle<CardModel>(this.Owner.Player.RunState.Rng.CombatCardSelection)
                     .Take<CardModel>(1))
        {
            CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
            if (card.IsUpgradable)
                CardCmd.Upgrade(card);
        }
    }
}