using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Character;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class SupplyBoxPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if(Owner.Player==null)
            return;
        
        var cards = CardFactory.GetDistinctForCombat(Owner.Player,
            ModelDb.CardPool<MonsterHunterCardPool>().GetUnlockedCards(Owner.Player.UnlockState,
                Owner.Player.RunState.CardMultiplayerConstraint).Where(c =>
                c.DynamicVars.ContainsKey("Consumable") && c.Rarity != CardRarity.Rare),
            this.Amount, Owner.Player.RunState.Rng.CombatCardGeneration);

        foreach (var card in cards)
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
        }
    }
}