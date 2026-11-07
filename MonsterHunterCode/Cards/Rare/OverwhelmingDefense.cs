using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class OverwhelmingDefense() : MonsterHunterCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(30M, ValueProp.Move)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);
        
        foreach (Creature target in (IEnumerable<Creature>) this.CombatState.GetTeammatesOf(this.Owner.Creature))
        {
            if (target.IsAlive && target.IsPlayer && target != this.Owner.Creature)
            {
                CoveredPower coveredPower = await PowerCmd.Apply<CoveredPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, 1M, this.Owner.Creature, (CardModel) null);
            }
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(10M);
}