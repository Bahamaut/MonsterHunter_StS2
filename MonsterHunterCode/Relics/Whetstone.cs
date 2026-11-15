using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MonsterHunter.MonsterHunterCode.Relics;

public class Whetstone : MonsterHunterRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.Creature.Player == null)
            return;

        foreach (var card in PileType.Draw.GetPile(Owner.Creature.Player).Cards
                     .Where((c => c.DynamicVars.ContainsKey("Combo")))
                     .ToList()
                     .UnstableShuffle(Owner.Creature.Player.RunState.Rng.CombatCardSelection)
                     .Take(1))
        {
            await CardPileCmd.Add(card, PileType.Hand);
            if (card.IsUpgradable)
                CardCmd.Upgrade(card);
            card.SetToFreeThisTurn();
        }
    }
}