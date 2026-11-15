using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class WideRangePower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.Card.DynamicVars.ContainsKey("Consumable") || this.Owner != cardPlay.Player.Creature)
            return;
        foreach (var teammate in CombatState.GetTeammatesOf(Owner).Where((c => c.IsAlive && c.IsPlayer)))
        {
            await CardCmd.AutoPlay(choiceContext, cardPlay.Card, teammate);
            await Cmd.Wait(0.1f);
        }
    }
}