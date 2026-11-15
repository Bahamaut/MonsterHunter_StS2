using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers.Oil;

public class DestroyerOilPower : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override int DisplayAmount => Math.Max(0, Amount - GetInternalData<Data>().AttacksPlayed);
    
    protected override object InitInternalData() => new Data();
    
    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (target.Player is null)
            return;
        
        if (target.HasPower<StaminaOilPower>())
        {
            if(target.HasPower<StaminaOilTempDexterityPower>())
            {
                    await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target,
                        -target.Player.Creature.Powers.OfType<StaminaOilTempDexterityPower>().First().Amount, 
                        applier, cardSource);
                await PowerCmd.Remove<StaminaOilTempDexterityPower>(target);
            }
            
            await PowerCmd.Remove<StaminaOilPower>(target);
        }
        else if (target.HasPower<AffinityOilPower>())
        {
            if (target.HasPower<AffinityOilTempStrengthPower>())
            {
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target,
                    -target._powers.OfType<AffinityOilTempStrengthPower>().First().Amount, 
                    applier, cardSource);
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
    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, 
        DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer == null || dealer != Owner && dealer.PetOwner?.Creature != Owner ||
            !props.IsPoweredAttack() || result.TotalDamage <= 0 || GetInternalData<Data>().AttacksPlayed >= Amount)
            return;
        
        await PowerCmd.Apply<WeakPower>(choiceContext, target, Amount, dealer, cardSource);
        SetAttacksPlayed(1);
    }
    
    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return Task.CompletedTask;
        SetAttacksPlayed(0);
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    private void SetAttacksPlayed(int value)
    {
        GetInternalData<Data>().AttacksPlayed = value;
        InvokeDisplayAmountChanged();
    }

    private class Data
    {
        public int AttacksPlayed;
    }

    
}