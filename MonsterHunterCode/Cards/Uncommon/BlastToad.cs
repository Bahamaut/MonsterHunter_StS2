using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class BlastToad() : MonsterHunterCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BlastPower>(4M),
        new PowerVar<VulnerablePower>(1M)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>(), HoverTipFactory.FromPower<VulnerablePower>() ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(CombatState == null)
            return;
        
        foreach (var hittableEnemy in CombatState.HittableEnemies)
        {
            var creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
            if (creatureNode != null)
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely((Godot.Node) NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Godot.Color("83eb85")));
            await MonsterHunterBlastCmd.HandleBlast(new ThrowingPlayerChoiceContext(),hittableEnemy,
                DynamicVars.Power<BlastPower>().BaseValue, Owner.Creature,null);
        }
        await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, 
            DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<BlastPower>().UpgradeValueBy(2M);
        DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}