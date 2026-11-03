using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers.Quest;

public class LowRankSlayQuestPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    public bool _dealtFatalThisCombat;
    
    [SavedProperty]
    public bool DealtFatalThisCombat
    {
        get => this._dealtFatalThisCombat;
        set
        {
            this.AssertMutable();
            this._dealtFatalThisCombat = value;
        }
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer == null || dealer != this.Owner && dealer.PetOwner?.Creature != this.Owner ||
            !props.IsPoweredAttack() || result.TotalDamage <= 0 || target.IsAlive || !target.IsPrimaryEnemy)
            return;
        _dealtFatalThisCombat = true;
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (this.DealtFatalThisCombat){
            room.AddExtraReward(this.Owner.Player, (Reward) new GoldReward(20, this.Owner.Player));
            return Task.CompletedTask;
        }
        
        room.AddExtraReward(this.Owner.Player, (Reward) new GoldReward(10, this.Owner.Player));
        return Task.CompletedTask;
    }
}