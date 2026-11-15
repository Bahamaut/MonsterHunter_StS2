using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MonsterHunter.MonsterHunterCode.Relics;

public class AfterComboStretches : MonsterHunterRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StretchPower>(4M)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack || cardPlay.Card.Owner != Owner ||
            !cardPlay.Card.DynamicVars.ContainsKey("Finisher"))
            return;
        Flash();
        await PowerCmd.Apply<StretchPower>(choiceContext, Owner.Creature, DynamicVars.Power<StretchPower>().IntValue,
            null, null);
    }
}