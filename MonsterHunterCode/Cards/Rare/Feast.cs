using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Character;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class Feast() : MonsterHunterCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardFactory.GetDistinctForCombat(Owner,
            ModelDb.CardPool<MonsterHunterCardPool>()
                .GetUnlockedCards(cardPlay.Player.UnlockState, cardPlay.Player.RunState.CardMultiplayerConstraint)
                .Where(c => c.DynamicVars.ContainsKey("Consumable") && c.Rarity != CardRarity.Rare),
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration);

        foreach (var card in cards)
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);
}