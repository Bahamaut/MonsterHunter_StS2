using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class EnergeticFlourishPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.DynamicVars.ContainsKey("Finisher"))
        {
            await PlayerCmd.GainEnergy(Amount, cardPlay.Card.Owner);
            await CardPileCmd.Draw(ctx, Amount, cardPlay.Card.Owner);
        }
    }
}