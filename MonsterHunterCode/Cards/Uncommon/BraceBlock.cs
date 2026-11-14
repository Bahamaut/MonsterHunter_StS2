using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class BraceBlock() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        (DynamicVar) new BlockVar(8M, ValueProp.Move),
        new PowerVar<StretchPower>(4M)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StretchPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);
        StretchPower stretchPower = await PowerCmd.Apply<StretchPower>(choiceContext, Owner.Creature,
            DynamicVars.Power<StretchPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2M);
        DynamicVars.Power<StretchPower>().UpgradeValueBy(3M);
    }
}