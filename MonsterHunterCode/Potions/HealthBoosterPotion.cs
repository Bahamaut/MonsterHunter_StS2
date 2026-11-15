using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Character;

namespace MonsterHunter.MonsterHunterCode.Potions;

public class HealthBoosterPotion : MonsterHunterPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        if (Owner.Creature.Player is null)
            return;
        
        var cards = CardFactory.GetDistinctForCombat(Owner.Creature.Player,
            ModelDb.CardPool<MonsterHunterCardPool>().GetUnlockedCards(Owner.Creature.Player.UnlockState,
                Owner.Creature.Player.RunState.CardMultiplayerConstraint).Where(c =>
                c.DynamicVars.ContainsKey("Consumable") && c.Rarity != CardRarity.Rare),
            DynamicVars.Cards.IntValue, Owner.Creature.Player.RunState.Rng.CombatCardGeneration);

        foreach (var card in cards)
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
            CardCmd.Upgrade(card);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Creature.Player);
        }
    }
}