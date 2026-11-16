using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Relics;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class ResupplyPower : TemporaryStrengthPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override AbstractModel OriginModel => ModelDb.Relic<Resupply>();
}