using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class DefensiveFlourishPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner &&
            (cardPlay.Card.DynamicVars.ContainsKey("Combo") ||
             cardPlay.Card.DynamicVars.ContainsKey("Finisher")))
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, cardPlay);
        }
    }
}