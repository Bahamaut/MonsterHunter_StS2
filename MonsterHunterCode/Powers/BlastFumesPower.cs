using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Powers;

  
public class BlastFumesPower() : MonsterHunterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        this.Flash();
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
        {
            NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
            if (creatureNode != null)
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Godot.Node) NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Godot.Color("83eb85")));
            await MonsterHunterBlastHelper.HandleBlast(new ThrowingPlayerChoiceContext(),hittableEnemy,this.Amount,this.Owner,null);
        }
    }
}