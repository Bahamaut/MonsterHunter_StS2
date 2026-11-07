using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class HunterFormPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
    ];

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        this.Flash();
        
        if(combatState != null && combatState.HittableEnemies.Any((Func<Creature, bool>) (e =>
               e.Monster != null && e.Monster.IntendsToAttack
           )))
            await PowerCmd.Apply<StrengthPower>(
                new ThrowingPlayerChoiceContext(), this.Owner, (Decimal)this.Amount, this.Owner,
                null);
        
        if(combatState != null && combatState.HittableEnemies.Any((Func<Creature, bool>) (e =>
                   e.Monster != null && !e.Monster.IntendsToAttack
               )))
            await PowerCmd.Apply<DexterityPower>(
            (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), this.Owner, (Decimal)this.Amount, this.Owner,
            (CardModel)null);
    }
}