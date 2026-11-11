using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class AllOut() : MonsterHunterCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(2),
        new DynamicVar("Power", 1M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        int amount = this.DynamicVars["Power"].IntValue;
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    }

    protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1M);
}