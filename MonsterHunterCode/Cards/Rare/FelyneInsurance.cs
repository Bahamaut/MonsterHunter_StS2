using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class FelyneInsurance() : MonsterHunterCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FelyneInsurancePower>(1M)];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<FelyneInsurancePower>(choiceContext, Owner.Creature, 
            DynamicVars.Power<FelyneInsurancePower>().BaseValue, Owner.Creature, this);
        if (DeckVersion is null)
            return;
        
        await CardPileCmd.RemoveFromDeck(DeckVersion);
        DeckVersion = null;
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}