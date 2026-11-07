using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class BlastResistance() : MonsterHunterCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) =>
        {
            ICombatState combatState = card.CombatState;
            return combatState != null ? combatState.Enemies.Where(c => c.IsAlive).Sum(c => c.GetPowerAmount<BlastBlightPower>()) : 0;
        }))];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.CalculatedBlock.Calculate(play.Target), this.DynamicVars.CalculatedBlock.Props, play);
    }

    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);
}