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
public class Roundslash() : MonsterHunterCard(1, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6M, ValueProp.Move),
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

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if(CombatState is null)
            return;
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard( this, play)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
    
    //Remove the card if moved to deck somehow
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Pile != null && (!participants.Contains(Owner.Creature) || Pile.Type != PileType.Draw))
            return;
        
        await CardCmd.Exhaust(choiceContext, this);
    }
}