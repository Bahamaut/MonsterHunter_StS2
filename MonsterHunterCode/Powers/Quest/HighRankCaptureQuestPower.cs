using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Powers;
using MonsterHunter.MonsterHunterCode.Relics;

namespace MonsterHunter.MonsterHunterCode.Powers.Quest;

public class HighRankCaptureQuestPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    public bool _tookDamageThisCombat;
    
    [SavedProperty]
    public bool TookDamageThisCombat
    {
        get => this._tookDamageThisCombat;
        set
        {
            this.AssertMutable();
            this._tookDamageThisCombat = value;
        }
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        this.TookDamageThisCombat = false;
        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != this.Owner || result.UnblockedDamage <= 0 || props.HasFlag((Enum) ValueProp.Unblockable))
            return Task.CompletedTask;
        this.TookDamageThisCombat = true;
        return Task.CompletedTask;
    }

    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player.Creature != this.Owner || this.TookDamageThisCombat)
            return false;
        foreach (CardCreationResult cardReward in cardRewards)
        {
            CardModel card1 = cardReward.Card;
            if (card1.IsUpgradable)
            {
                CardModel card2 = this.Owner.CombatState.CloneCard(card1);
                CardCmd.Upgrade(card2);
                cardReward.ModifyCard(card2);
            }
        }
        return true;
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if(TookDamageThisCombat)
            return Task.CompletedTask;

        if (Owner.Player._relics.Any(r => r is HighRankQuestBoard))
           Owner.Player._relics.OfType<HighRankQuestBoard>().FirstOrDefault().CaptureQuestComplete = true;
        return Task.CompletedTask;
    }
}