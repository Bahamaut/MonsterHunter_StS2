using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class PerfectGuardPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner || card.Type != CardType.Attack)
            return false;

        var pileType = card.Pile?.Type;
        if (pileType != PileType.Hand && pileType != PileType.Play) 
            return false;
        
        modifiedCost = Math.Max(0m, originalCost - 1);
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.Type == CardType.Attack)
        {
            var pileType = cardPlay.Card.Pile?.Type;
            if (pileType == PileType.Hand || pileType == PileType.Play)
                await PowerCmd.Remove(this);
        }
    }
}