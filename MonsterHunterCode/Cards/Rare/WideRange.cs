using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards.Uncommon;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class WideRange() : MonsterHunterCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        WideRangePower wideRangePower = await PowerCmd.Apply<WideRangePower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}