using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class ReadyUp() : MonsterHunterCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal =>
        CombatState != null && CombatState.HittableEnemies.Any(e =>
            e.Monster != null && e.Monster.IntendsToAttack);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (cardPlay.Target.Monster != null && !cardPlay.Target.Monster.IntendsToAttack)
            return;
        
        foreach (var card in PileType.Draw.GetPile(this.Owner).Cards
                     .Where(c => c.DynamicVars.ContainsKey("Combo"))
                     .ToList().UnstableShuffle(Owner.RunState.Rng.CombatCardSelection)
                     .Take(1))
        {
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}