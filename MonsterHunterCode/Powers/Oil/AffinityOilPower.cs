using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers.Oil;

public class AffinityOilPower : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>[HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (target.Player == null)
            return;
        
        if (target.HasPower<AffinityOilPower>())
        {
            await PowerCmd.Remove(this);
        }
        else if (target.HasPower<StaminaOilPower>())
        {
            if (target.HasPower<StaminaOilTempDexterityPower>())
            {
                await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target,
                    -target.Player.Creature.Powers.OfType<StaminaOilTempDexterityPower>().First().Amount,
                    applier, cardSource);
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

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer,
        DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer == null || dealer != Owner && dealer.PetOwner?.Creature != Owner ||
            !props.IsPoweredAttack() || result.TotalDamage <= 0)
            return;

        //Check for 2 stacks
        if (dealer.HasPower<AffinityOilTempStrengthPower>() &&
            dealer.Player != null &&
            dealer.Player.Creature.Powers.OfType<AffinityOilTempStrengthPower>().First().Amount == 2)
            return;

        await PowerCmd.Apply<AffinityOilTempStrengthPower>(choiceContext, dealer, 
            1M, Owner, null);
    }
}