using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class BarrelBombBowling() : MonsterHunterCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    private readonly Color _vfxTint = new ("3D9191");
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BlastPower>(3M),
        new RepeatVar(3)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        var lastPos = Vector2.Zero;
        if (CombatState is null || NCombatRoom.Instance is null)
            return;
        
        for (var i = 0; i < DynamicVars.Repeat.IntValue; ++i)
        {
            var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (enemy != null)
            {
                if (TestMode.IsOff)
                {
                    if (i == 0)
                        lastPos = NCombatRoom.Instance.GetCreatureNode(Owner.Creature).VfxSpawnPosition;
                    var targetNode = NCombatRoom.Instance.GetCreatureNode(enemy);
                    if (targetNode != null)
                    {
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Node) NItemThrowVfx.Create(lastPos, targetNode.GetBottomOfHitbox(), ModelDb.Potion<PoisonPotion>().Image));
                        lastPos = targetNode.VfxSpawnPosition;
                        await Cmd.Wait(0.5f);
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Node) NSplashVfx.Create(targetNode.VfxSpawnPosition, _vfxTint));
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Node) NLiquidOverlayVfx.Create(enemy, _vfxTint));
                        NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Node) NGaseousImpactVfx.Create(targetNode.VfxSpawnPosition, _vfxTint));
                    }
                    targetNode = null;
                }
                await MonsterHunterBlastCmd.HandleBlast(new ThrowingPlayerChoiceContext(),enemy,DynamicVars.Power<BlastPower>().BaseValue, Owner.Creature,null);
            }
            enemy = null;
        }
    }

    protected override void OnUpgrade() => DynamicVars.Repeat.UpgradeValueBy(1M);
}