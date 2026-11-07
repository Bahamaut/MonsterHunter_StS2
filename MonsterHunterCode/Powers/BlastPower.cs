using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class BlastPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}