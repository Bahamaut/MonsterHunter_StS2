using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class SupermanDive() : MonsterHunterCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(1M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        IntangiblePower intangiblePower = await PowerCmd.Apply<IntangiblePower>(choiceContext, this.Owner.Creature, this.DynamicVars["IntangiblePower"].BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}