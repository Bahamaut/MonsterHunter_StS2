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
    public static async Task HandleBlast(PlayerChoiceContext choiceContext, Creature target, decimal amount,
        Creature? applier, CardModel? cardSource)
    {
        if (!target.HasPower<BlastBlightPower>())
            await PowerCmd.Apply<BlastBlightPower>(choiceContext, target, 1M, applier, cardSource);

        await PowerCmd.Apply<BlastPower>(choiceContext, target, amount, applier, cardSource);
        
        while (target.HasPower<BlastPower>() && target.Powers.First(x => x is BlastPower).Amount >=
               target.Powers.First(x => x is BlastBlightPower).Amount)
        {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(
                (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), target,
                target.Powers.First(x => x is BlastBlightPower).Amount,
                ValueProp.Unblockable | ValueProp.Unpowered, (CardModel)null, (CardPlay)null);
            
            if(target.IsDead)
                return;
                
            await PowerCmd.ModifyAmount(choiceContext, target.Powers.First(x => x is BlastPower),
                -target.Powers.First(x => x is BlastBlightPower).Amount, applier, cardSource);
            
            await PowerCmd.ModifyAmount(choiceContext, target.Powers.First(x => x is BlastBlightPower),
                2M, applier, cardSource);
        }
    }
}