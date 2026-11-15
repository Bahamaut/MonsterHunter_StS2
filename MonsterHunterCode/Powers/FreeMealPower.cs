using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class FreeMealPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (!card.DynamicVars.ContainsKey("Consumable"))
            return playCount;

        return card.Owner.Creature != this.Owner ||
               CombatManager.Instance.History.CardPlaysStarted.Count(
                   (e => e.Actor == Owner && e.CardPlay.IsFirstInSeries
                       && e.HappenedThisTurn(CombatState) &&
                       e.CardPlay.Card.DynamicVars.ContainsKey("Consumable"))) >= Amount
            ? playCount
            : playCount + 1;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        return Task.CompletedTask;
    }
}