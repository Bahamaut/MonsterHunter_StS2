using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class Nullberry() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        new BlockVar(5M, ValueProp.Move),
        new StringVar("Consumable")
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);
        
        if(play.Player.Creature.Powers.FirstOrDefault(p => p is VulnerablePower) != null)
            await PowerCmd.ModifyAmount(choiceContext, play.Player.Creature.Powers.First(p => p is VulnerablePower),
            Math.Ceiling(((decimal)play.Player.Creature.Powers.First(p => p is VulnerablePower).Amount) / 2),
            play.Player.Creature, this);
        
        if(play.Player.Creature.Powers.FirstOrDefault(p => p is WeakPower) != null)
            await PowerCmd.ModifyAmount(choiceContext, play.Player.Creature.Powers.First(p => p is WeakPower),
                Math.Ceiling(((decimal)play.Player.Creature.Powers.First(p => p is WeakPower).Amount) / 2),
                play.Player.Creature, this);
        
        if(play.Player.Creature.Powers.FirstOrDefault(p => p is FrailPower) != null)
            await PowerCmd.ModifyAmount(choiceContext, play.Player.Creature.Powers.First(p => p is FrailPower),
                Math.Ceiling(((decimal)play.Player.Creature.Powers.First(p => p is FrailPower).Amount) / 2),
                play.Player.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(3M);
    }
}