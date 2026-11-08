using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards.Uncommon;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class WideRangePower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.CombatState == null || !cardPlay.Card.DynamicVars.ContainsKey("Consumable") || this.Owner != cardPlay.Player.Creature)
            return;
        foreach (Creature teammate in this.CombatState.GetTeammatesOf(this.Owner).Where<Creature>((c => c != null && c.IsAlive && c.IsPlayer)))
        {
            await CardCmd.AutoPlay(choiceContext, cardPlay.Card, teammate);
            await Cmd.Wait(0.1f);
        }
    }
}