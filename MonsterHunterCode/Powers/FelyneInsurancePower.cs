using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

  
public class FelyneInsurancePower() : MonsterHunterPower
{
    private bool _preventedDeath;
    
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override bool ShouldDie(Creature creature)
    {
        _preventedDeath = true;
        return false;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        if (_preventedDeath)
        {
            _preventedDeath = false;
            int num = (int)((decimal)((PowerModel)this).Owner.MaxHp * 0.30m);
            await CreatureCmd.SetCurrentHp(((PowerModel)this).Owner, num);
            await PowerCmd.Remove((PowerModel)(object)this);
            ((PowerModel)this).Flash();
        }
    }
}