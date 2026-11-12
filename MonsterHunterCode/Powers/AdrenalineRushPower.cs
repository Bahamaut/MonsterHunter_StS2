using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class AdrenalineRushPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        if (this.Owner != attack.Attacker)
            return base.ModifyAttackHitCount(attack, hitCount);
        
        return base.ModifyAttackHitCount(attack, hitCount*2);
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        return this.Owner != dealer || !props.IsPoweredAttack() ? 0M : -Math.Ceiling(amount/2);
        //return base.ModifyDamageAdditive(target, Math.Ceiling(amount/2), props, dealer, cardSource, cardPlay);
    }
}