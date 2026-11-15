using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MonsterHunter.MonsterHunterCode.Relics;

  
public class Resupply : MonsterHunterRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        Flash();
        
        if(combatState.HittableEnemies.Any(e =>
               e.Monster != null && e.Monster.IntendsToAttack))
            await PowerCmd.Apply<TemporaryStrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 
                1M, Owner.Creature, null);
    }
}