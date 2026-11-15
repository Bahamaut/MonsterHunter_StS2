using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace MonsterHunter.MonsterHunterCode.Powers;


public class BlastBuildupPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];

    public override Decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver,
        Decimal amount, Creature? target, CardModel? cardSource)
    {
        return !(power is BlastPower) || giver != Owner || amount <= 0 ? 0M : Amount;
    }
    
    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        Flash();
        return Task.CompletedTask;
    }
}