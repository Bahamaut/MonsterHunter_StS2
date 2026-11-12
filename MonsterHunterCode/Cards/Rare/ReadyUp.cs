using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class ReadyUp() : MonsterHunterCard(0,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard((CardModel)this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        foreach (CardModel card in PileType.Draw.GetPile(this.Owner).Cards
                     .Where<CardModel>((Func<CardModel, bool>)(c => c.DynamicVars.ContainsKey("Combo")))
                     .ToList<CardModel>().UnstableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection)
                     .Take<CardModel>(1))
        {
            CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}