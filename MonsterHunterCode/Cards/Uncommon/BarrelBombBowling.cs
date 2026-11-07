using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class BarrelBombBowling() : MonsterHunterCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.RandomEnemy)
{
    private readonly Godot.Color _vfxTint = new Godot.Color("3D9191");
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BlastPower>(3M),
        new RepeatVar(3)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        Vector2 lastPos = Vector2.Zero;
        for (int i = 0; i < this.DynamicVars.Repeat.IntValue; ++i)
        {
            Creature enemy = this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.CombatState.HittableEnemies);
            if (enemy != null)
            {
                if (TestMode.IsOff)
                {
                    if (i == 0)
                        lastPos = NCombatRoom.Instance.GetCreatureNode(this.Owner.Creature).VfxSpawnPosition;
                    NCreature targetNode = NCombatRoom.Instance.GetCreatureNode(enemy);
                    if (targetNode != null)
                    {
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Godot.Node) NItemThrowVfx.Create(lastPos, targetNode.GetBottomOfHitbox(), ModelDb.Potion<PoisonPotion>().Image));
                        lastPos = targetNode.VfxSpawnPosition;
                        await Cmd.Wait(0.5f);
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Godot.Node) NSplashVfx.Create(targetNode.VfxSpawnPosition, this._vfxTint));
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Godot.Node) NLiquidOverlayVfx.Create(enemy, this._vfxTint));
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Godot.Node) NGaseousImpactVfx.Create(targetNode.VfxSpawnPosition, this._vfxTint));
                    }
                    targetNode = (NCreature) null;
                }
                await MonsterHunterBlastHelper.HandleBlast(new ThrowingPlayerChoiceContext(),enemy,this.DynamicVars.Power<BlastPower>().BaseValue, this.Owner.Creature,null);
            }
            enemy = (Creature) null;
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}