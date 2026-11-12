using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class IntentOneTwo() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target.Monster.IntendsToAttack)
            await PowerCmd.Apply<BurstPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature,
                (CardModel)this);
        else
            await PowerCmd.Apply<OneTwoPunchPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature,
                (CardModel)this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}