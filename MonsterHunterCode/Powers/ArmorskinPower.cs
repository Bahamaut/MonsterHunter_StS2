using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards.Common;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class ArmorskinPower : TemporaryDexterityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Armorskin>();
}