using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Character;

namespace MonsterHunter.MonsterHunterCode.Cards.Common;

public class Carve() : MonsterHunterCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8M, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        
        var cards = CardFactory.GetDistinctForCombat(Owner,
            ModelDb.CardPool<MonsterHunterCardPool>().GetUnlockedCards(cardPlay.Player.UnlockState,
                cardPlay.Player.RunState.CardMultiplayerConstraint).Where(c => 
                c.DynamicVars.ContainsKey("Consumable") && c.Rarity != CardRarity.Rare),
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration);

        foreach (var card in cards)
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}