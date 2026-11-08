using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class FreeMealPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        return card.Owner.Creature != this.Owner ||
               CombatManager.Instance.History.CardPlaysStarted.Count(
                   (e => e.Actor == this.Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(this.CombatState) &&
                         e.CardPlay.Card.DynamicVars.ContainsKey("Consumable"))) >= this.Amount
            ? playCount
            : playCount + 1;
    }
    
    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        this.Flash();
        return Task.CompletedTask;
    }
}