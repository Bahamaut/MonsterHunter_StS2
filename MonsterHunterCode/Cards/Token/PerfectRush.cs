using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PerfectRush() : MonsterHunterCard(2, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5M, ValueProp.Move),
        new RepeatVar(4),
        new StringVar("Combo"),
        new StringVar("Finisher")
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Ethereal,
        CardKeyword.Exhaust
    ];
    
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Player == Owner && cardPlay.Card != this && Pile != null && Pile.Type == PileType.Hand)
            await CardCmd.Exhaust(new ThrowingPlayerChoiceContext(), this);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_starry_impact", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Repeat.UpgradeValueBy(1M);
    
    //Remove the card if moved to deck somehow
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Pile != null && (!participants.Contains(Owner.Creature) || Pile.Type != PileType.Draw))
            return;
        
        await CardCmd.Exhaust(choiceContext, this);
    }
}