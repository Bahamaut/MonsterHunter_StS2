using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class SongOPurrtectionPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext,
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(Owner.Player==null || !participants.Contains(Owner))
            return;
        
        foreach (var player in combatState.Players)
        {
            await CreatureCmd.TriggerAnim(player.Creature, "PowerUp", player.Character.PowerUpAnimDelay);
            await PowerCmd.Apply<StretchPower>(choiceContext, player.Creature, Amount, Owner, null);
        }
    }
}