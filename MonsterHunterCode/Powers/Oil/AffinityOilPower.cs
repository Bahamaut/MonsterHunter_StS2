using System.Collections.ObjectModel;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers.Oil;

public class AffinityOilPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new ReadOnlyCollection<IHoverTip>([HoverTipFactory.FromPower<StrengthPower>()]);

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if(target.HasPower<AffinityOilPower>())
        {
            await PowerCmd.Remove((PowerModel)this);  
        }
        else if (target.HasPower<StaminaOilPower>())
        {
            if(target.HasPower<StaminaOilTempDexterityPower>())
            {
                await PowerCmd.Apply<DexterityPower>( new ThrowingPlayerChoiceContext(), target, -(target._powers.OfType<StaminaOilTempDexterityPower>().FirstOrDefault().Amount), target, (CardModel) null);
                await PowerCmd.Remove<StaminaOilTempDexterityPower>(target);
            }
            await PowerCmd.Remove<StaminaOilPower>(target);
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
            !props.IsPoweredAttack() || result.TotalDamage <= 0)
            return;
        
        //Check for 2 stacks
        if (dealer.HasPower<AffinityOilTempStrengthPower>() &&
            dealer._powers.OfType<AffinityOilTempStrengthPower>().FirstOrDefault().Amount == 2)
            return;
        
        AffinityOilTempStrengthPower tempStrength = await PowerCmd.Apply<AffinityOilTempStrengthPower>(choiceContext, dealer,
            (Decimal)(1M), this.Owner, (CardModel)null);
    }
}
