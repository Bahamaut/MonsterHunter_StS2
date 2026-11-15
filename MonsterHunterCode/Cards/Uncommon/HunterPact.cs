using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class HunterPact() : MonsterHunterCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var amount = CombatState != null && CombatState.HittableEnemies.Any(e =>
            e.Monster != null && e.Monster.IntendsToAttack)
            ? DynamicVars.Cards.IntValue + 1
            : DynamicVars.Cards.IntValue;
        await CardPileCmd.Draw(choiceContext, amount, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1M);
}