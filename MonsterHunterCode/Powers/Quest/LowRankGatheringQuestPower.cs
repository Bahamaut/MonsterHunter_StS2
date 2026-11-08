using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers.Quest;

public class LowRankGatheringQuestPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner.Player == null)
            return Task.CompletedTask;

        PotionModel? potion = null;

        while (potion is null)
        {
            potion = PotionFactory.CreateRandomPotionOutOfCombat(this.Owner.Player,
                this.Owner.Player.PlayerRng.Rewards,
                PotionFactory.GetPotionOptions(Owner.Player).Where(p =>
                    p.Rarity != PotionRarity.Common));
        }

        room.AddExtraReward(this.Owner.Player, (Reward)new PotionReward(potion.ToMutable(), this.Owner.Player));
        return Task.CompletedTask;
    }
}