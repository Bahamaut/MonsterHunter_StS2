using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MonsterHunter.MonsterHunterCode.Potions;

public class RocksteadyPotion : MonsterHunterPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>(8M),
        new PowerVar<StretchPower>(8M)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<VigorPower>(),
        HoverTipFactory.FromPower<StretchPower>()
    ];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<VigorPower>(choiceContext, target,
            DynamicVars.Power<VigorPower>().IntValue, Owner.Creature, null);
        await PowerCmd.Apply<StretchPower>(choiceContext, target,
            DynamicVars.Power<StretchPower>().IntValue, Owner.Creature, null);
    }
}