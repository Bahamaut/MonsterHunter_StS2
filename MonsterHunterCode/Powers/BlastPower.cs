using MegaCrit.Sts2.Core.Entities.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class BlastPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
}