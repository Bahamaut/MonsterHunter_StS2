using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class FelyneInsurance() : MonsterHunterCard(3,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        FelyneInsurancePower felyneInsurancePower = await PowerCmd.Apply<FelyneInsurancePower>(choiceContext,
            this.Owner.Creature, 1M, this.Owner.Creature, this);
        await CardPileCmd.RemoveFromDeck(this.DeckVersion);
        this.DeckVersion = null;
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}