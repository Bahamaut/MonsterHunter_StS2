using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class PerfectGuardPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != ((PowerModel)(object)this).Owner)
        {
            return false;
        }
        if (card.Type != CardType.Attack)
        {
            return false;
        }
        PileType? pileType = card.Pile?.Type;
        if (pileType == PileType.Hand || pileType == PileType.Play)
        {
            modifiedCost = Math.Max(0m, originalCost - 1);
            return true;
        }
        return false;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == ((PowerModel)(object)this).Owner && cardPlay.Card.Type == CardType.Attack)
        {
            PileType? pileType = cardPlay.Card.Pile?.Type;
            if (pileType == PileType.Hand || pileType == PileType.Play)
            {
                await PowerCmd.Remove((PowerModel?)(object)this);
            }
        }
    }
}