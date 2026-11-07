using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MonsterHunter.MonsterHunterCode.Cards;
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

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
        {
            NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
            if (creatureNode != null)
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Godot.Node) NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Godot.Color("83eb85")));
            await MonsterHunterBlastHelper.HandleBlast(new ThrowingPlayerChoiceContext(),hittableEnemy,this.DynamicVars.Power<BlastPower>().BaseValue, this.Owner.Creature,null);
        }
        IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>(choiceContext, (IEnumerable<Creature>) this.CombatState?.HittableEnemies, this.DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Power<BlastPower>().UpgradeValueBy(2M);
        this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}