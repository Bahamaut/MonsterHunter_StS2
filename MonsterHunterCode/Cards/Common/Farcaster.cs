using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Character;

namespace MonsterHunter.MonsterHunterCode.Cards.Common;

public class Farcaster() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8M, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);
        
        var cards = CardFactory.GetDistinctForCombat(this.Owner,
            ModelDb.CardPool<MonsterHunterCardPool>().GetUnlockedCards(play.Player.UnlockState,
                play.Player.RunState.CardMultiplayerConstraint).Where(c =>
                c.DynamicVars.ContainsKey("Consumable") && c.Rarity != CardRarity.Rare),
            this.DynamicVars.Cards.IntValue, this.Owner.RunState.Rng.CombatCardGeneration);

        foreach (var card in cards)
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat((CardModel)(object)card, PileType.Hand, ((CardModel)this).Owner);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}