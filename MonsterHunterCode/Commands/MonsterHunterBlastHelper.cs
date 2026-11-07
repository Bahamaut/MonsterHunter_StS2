using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Commands;

public class MonsterHunterBlastHelper
{
    public static async Task HandleBlast(PlayerChoiceContext choiceContext, CardPlay cardPlay, decimal amount,
        Creature? applier, CardModel? cardSource)
    {
        if (!cardPlay.Target.HasPower<BlastBlightPower>())
            await PowerCmd.Apply<BlastBlightPower>(choiceContext, cardPlay.Target, 1M, applier, cardSource);

        await PowerCmd.Apply<BlastPower>(choiceContext, cardPlay.Target, amount, applier, cardSource);

        while (cardPlay.Target.Powers.First(x => x is BlastPower).Amount >=
               cardPlay.Target.Powers.First(x => x is BlastBlightPower).Amount)
        {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(
                (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), cardPlay.Target,
                cardPlay.Target.Powers.First(x => x is BlastBlightPower).Amount,
                ValueProp.Unblockable | ValueProp.Unpowered, (CardModel)null, (CardPlay)null);

            await PowerCmd.ModifyAmount(choiceContext, cardPlay.Target.Powers.First(x => x is BlastPower),
                -cardPlay.Target.Powers.First(x => x is BlastBlightPower).Amount, applier, cardSource);
            
            await PowerCmd.ModifyAmount(choiceContext, cardPlay.Target.Powers.First(x => x is BlastBlightPower),
                2M, applier, cardSource);
        }
    }
}