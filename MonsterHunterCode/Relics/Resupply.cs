using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Relics;

  
public class Resupply : MonsterHunterRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ResupplyPower>(1M)];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        Flash();
        
        if(combatState.HittableEnemies.Any(e =>
               e.Monster != null && e.Monster.IntendsToAttack))
            await PowerCmd.Apply<ResupplyPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 
                DynamicVars.Power<ResupplyPower>().BaseValue, null, null);
    }
}