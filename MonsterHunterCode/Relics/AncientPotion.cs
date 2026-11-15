using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MonsterHunter.MonsterHunterCode.Relics;

public class AncientPotion : MonsterHunterRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        var creature = Owner.Creature;
        await CreatureCmd.Heal(creature, (creature.MaxHp - creature.CurrentHp));
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (Owner.PlayerCombatState != null && (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1))
            return;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}