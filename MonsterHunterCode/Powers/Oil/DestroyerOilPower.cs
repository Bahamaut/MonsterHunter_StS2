using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers.Oil;

public class DestroyerOilPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override int DisplayAmount => Math.Max(0, this.Amount - this.GetInternalData<DestroyerOilPower.Data>().attacksPlayed);
    
    protected override object InitInternalData() => (object) new DestroyerOilPower.Data();
    
    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (target.HasPower<StaminaOilPower>())
        {
            if(target.HasPower<StaminaOilTempDexterityPower>())
            {
                await PowerCmd.Apply<DexterityPower>( new ThrowingPlayerChoiceContext(), target, -(target._powers.OfType<StaminaOilTempDexterityPower>().FirstOrDefault().Amount), target, (CardModel) null);
                await PowerCmd.Remove<StaminaOilTempDexterityPower>(target);
            }
            await PowerCmd.Remove<StaminaOilPower>(target);
        }
        else if (target.HasPower<AffinityOilPower>())
        {
            if (target.HasPower<AffinityOilTempStrengthPower>())
            {
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target,
                    -(target._powers.OfType<AffinityOilTempStrengthPower>().FirstOrDefault().Amount), target,
                    (CardModel)null);
                await PowerCmd.Remove<AffinityOilTempStrengthPower>(target);
            }

            await PowerCmd.Remove<AffinityOilPower>(target);
        }
        else if (target.HasPower<MindsEyeOilPower>())
        {
            await PowerCmd.Remove<MindsEyeOilPower>(target);
        }
        else if (target.HasPower<DestroyerOilPower>())
        {
            await PowerCmd.Remove<DestroyerOilPower>(target);
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
            !props.IsPoweredAttack() || result.TotalDamage <= 0 || this.GetInternalData<DestroyerOilPower.Data>().attacksPlayed >= this.Amount)
            return;
        
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, target, this.Amount, dealer, (CardModel) cardSource);
        SetAttacksPlayed(1);
    }
    
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return Task.CompletedTask;
        this.SetAttacksPlayed(0);
        this.InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
    
    public void SetAttacksPlayed(int value)
    {
        this.GetInternalData<DestroyerOilPower.Data>().attacksPlayed = value;
        this.InvokeDisplayAmountChanged();
    }

    public class Data
    {
        public int attacksPlayed;
    }

    
}