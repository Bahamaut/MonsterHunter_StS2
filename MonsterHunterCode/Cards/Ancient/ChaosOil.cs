using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers.Oil;

namespace MonsterHunter.MonsterHunterCode.Cards.Ancient;

public class ChaosOil() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Innate
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ChaosOilPower chaosOilPower = await PowerCmd.Apply<ChaosOilPower>(choiceContext, this.Owner.Creature,
            1M, this.Owner.Creature, (CardModel)this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}