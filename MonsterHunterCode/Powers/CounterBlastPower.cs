using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Commands;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class CounterBlastPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.BlockedDamage <= 0 || !props.IsPoweredAttack() || dealer == null)
            return;
        await MonsterHunterBlastCmd.HandleBlast(new ThrowingPlayerChoiceContext(),
            dealer,result.BlockedDamage,Owner,cardSource);
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return;
        await PowerCmd.Decrement(this);
    }
}