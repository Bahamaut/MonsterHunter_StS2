using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class Nullberry() : MonsterHunterCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        new BlockVar(5M, ValueProp.Move),
        new StringVar("Consumable")
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        
        if(cardPlay.Player.Creature.Powers.Any(p => p is VulnerablePower))
            await PowerCmd.ModifyAmount(choiceContext, cardPlay.Player.Creature.Powers.First(p => p is VulnerablePower),
            -Math.Ceiling(((decimal)cardPlay.Player.Creature.Powers.First(p => p is VulnerablePower).Amount) / 2),
            cardPlay.Player.Creature, this);
        
        if(cardPlay.Player.Creature.Powers.Any(p => p is WeakPower))
            await PowerCmd.ModifyAmount(choiceContext, cardPlay.Player.Creature.Powers.First(p => p is WeakPower),
                -Math.Ceiling(((decimal)cardPlay.Player.Creature.Powers.First(p => p is WeakPower).Amount) / 2),
                cardPlay.Player.Creature, this);
        
        if(cardPlay.Player.Creature.Powers.Any(p => p is FrailPower))
            await PowerCmd.ModifyAmount(choiceContext, cardPlay.Player.Creature.Powers.First(p => p is FrailPower),
                -Math.Ceiling(((decimal)cardPlay.Player.Creature.Powers.First(p => p is FrailPower).Amount) / 2),
                cardPlay.Player.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
    }
}