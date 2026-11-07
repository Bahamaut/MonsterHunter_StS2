using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class EvadeExtender() : MonsterHunterCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        (DynamicVar) new BlockVar(5M, ValueProp.Move),
        new DynamicVar(nameof (Blur), 1M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);
        BlurPower blurPower = await PowerCmd.Apply<BlurPower>(choiceContext, this.Owner.Creature, this.DynamicVars[nameof (Blur)].BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}