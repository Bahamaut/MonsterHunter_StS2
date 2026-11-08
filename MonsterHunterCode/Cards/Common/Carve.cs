using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Token;
using MonsterHunter.MonsterHunterCode.Character;

namespace MonsterHunter.MonsterHunterCode.Cards.Common;

public class Carve() : MonsterHunterCard(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8M, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull((object)play.Target, "play.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play)
            .Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);


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

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(2M);
        this.DynamicVars.Cards.UpgradeValueBy(1M);
    }
}