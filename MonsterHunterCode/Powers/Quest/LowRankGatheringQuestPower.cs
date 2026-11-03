using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
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
        var potion = PotionFactory.CreateRandomPotionsOutOfCombat(this.Owner.Player, 1,
                this.Owner.Player.RunState.Rng.CombatPotionGeneration,
                PotionFactory.GetPotionOptions(this.Owner.Player).Where(p => p.Rarity != PotionRarity.Common))
            .FirstOrDefault();
        if (potion == null)
            return Task.CompletedTask;

        room.AddExtraReward(this.Owner.Player, (Reward)new PotionReward(potion, this.Owner.Player));
        return Task.CompletedTask;
    }
}