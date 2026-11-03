using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class StaminaOilPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new ReadOnlyCollection<IHoverTip>([HoverTipFactory.FromPower<DexterityPower>()]);

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (target.HasPower<StaminaOilPower>())
        {
            await PowerCmd.Remove((PowerModel)this);
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

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props,
        CardModel? cardSource)
    {
        if (cardSource ==null || cardSource.Owner.Creature!=Owner)
                return;

        if (Owner.HasPower<StaminaOilTempDexterityPower>() &&
            Owner._powers.OfType<StaminaOilTempDexterityPower>().FirstOrDefault().Amount == 2)
            return;

        StaminaOilTempDexterityPower tempDexterity = await PowerCmd.Apply<StaminaOilTempDexterityPower>(new ThrowingPlayerChoiceContext(),
            Owner,
            (Decimal)(1M), this.Owner, (CardModel)null);
    }
}