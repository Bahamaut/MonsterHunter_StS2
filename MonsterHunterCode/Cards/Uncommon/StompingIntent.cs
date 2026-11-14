using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class StompingIntent() : MonsterHunterCard(3,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12M, ValueProp.Move)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard((CardModel)this, cardPlay).TargetingAllOpponents(this.CombatState)
            .WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").WithHitVfxSpawnedAtBase()
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card != this || originalCost <= 0M || CombatState == null)
            return false;
        
        modifiedCost = Math.Max(0M, originalCost - CombatState.HittableEnemies.Count(e =>
            e.Monster != null && e.Monster.IntendsToAttack));
        return true;
    }
}