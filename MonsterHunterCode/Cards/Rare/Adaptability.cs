using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class Adaptability() : MonsterHunterCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<DexterityPower>(1M),
        new PowerVar<StrengthPower>(1M),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AdaptabilityPower adaptabilityPower = await PowerCmd.Apply<AdaptabilityPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Innate);
}