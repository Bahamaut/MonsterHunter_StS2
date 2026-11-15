using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MonsterHunter.MonsterHunterCode.Relics;

public class SpeedEater : MonsterHunterRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (Owner.PlayerCombatState != null &&
            (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1))
            return;

        IReadOnlyList<CardModel> list1 = Owner.Character.CardPool
            .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(c => c.DynamicVars.ContainsKey("Consumable")).ToList();
        if (list1.Count <= 0)
            return;
        
        Flash();
        var list2 = CardFactory.GetDistinctForCombat(Owner, list1,
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration).ToList();
        Flash();
        await CardPileCmd.AddGeneratedCardsToCombat(list2, PileType.Hand, Owner);
    }
}