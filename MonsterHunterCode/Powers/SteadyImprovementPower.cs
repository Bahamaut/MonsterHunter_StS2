using MegaCrit.Sts2.Core.Entities.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class SteadyImprovementPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
}