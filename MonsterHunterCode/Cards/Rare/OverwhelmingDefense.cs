using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class OverwhelmingDefense() : MonsterHunterCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(30M, ValueProp.Move),
        new PowerVar<CoveredPower>(1M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if(CombatState is null) 
            return;
        
        foreach (var target in CombatState.GetTeammatesOf(Owner.Creature))
        {
            if (target.IsAlive && target.IsPlayer && target != Owner.Creature)
                await PowerCmd.Apply<CoveredPower>(new ThrowingPlayerChoiceContext(), target, 
                    DynamicVars.Power<CoveredPower>().BaseValue, Owner.Creature, null);
        }
        PlayerCmd.EndTurn(cardPlay.Player, canBackOut: false);
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(10M);
}