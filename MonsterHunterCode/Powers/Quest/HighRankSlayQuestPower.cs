using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MonsterHunter.MonsterHunterCode.Powers.Quest;

public class HighRankSlayQuestPower : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner.Player != null) 
            room.AddExtraReward(Owner.Player, new GoldReward(30, Owner.Player));
        return Task.CompletedTask;
    }
}