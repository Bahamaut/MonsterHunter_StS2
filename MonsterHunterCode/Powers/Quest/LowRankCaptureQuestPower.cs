using BaseLib.Common.Rewards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers.Quest;

public class LowRankCaptureQuestPower : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    private bool _tookDamageThisCombat;
    
    [SavedProperty]
    private bool TookDamageThisCombat
    {
        get => _tookDamageThisCombat;
        set
        {
            AssertMutable();
            _tookDamageThisCombat = value;
        }
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        TookDamageThisCombat = false;
        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0 || props.HasFlag(ValueProp.Unblockable))
            return Task.CompletedTask;
        TookDamageThisCombat = true;
        return Task.CompletedTask;
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if(TookDamageThisCombat || Owner.Player == null)
            return Task.CompletedTask;

        room.AddExtraReward(Owner.Player, new RandomCardUpgradeReward(Owner.Player));
        return Task.CompletedTask;
    }
}