using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class SnackSlap() : MonsterHunterCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3M, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard((CardModel)this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_starry_impact")
            .Execute(choiceContext);
    }

    public override async Task AfterCardPlayedLate(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Skill ||
            Pile.Type == PileType.Hand || !cardPlay.Card.DynamicVars.ContainsKey("Consumable"))
            return;
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add((CardModel)this, PileType.Hand);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}