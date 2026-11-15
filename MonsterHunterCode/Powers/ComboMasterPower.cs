using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MonsterHunter.MonsterHunterCode.Powers;

public class ComboMasterPower : MonsterHunterPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner) || Owner.Player is null)
            return;
        foreach (var card in PileType.Draw.GetPile(Owner.Player).Cards
                     .Where((c => c.DynamicVars.ContainsKey("Combo")))
                     .ToList()
                     .UnstableShuffle(Owner.Player.RunState.Rng.CombatCardSelection)
                     .Take(1))
        {
            await CardPileCmd.Add(card, PileType.Hand);
            if (card.IsUpgradable)
                CardCmd.Upgrade(card);
        }
    }
}