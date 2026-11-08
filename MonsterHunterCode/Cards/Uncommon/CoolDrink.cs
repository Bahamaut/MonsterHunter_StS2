using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class CoolDrink() : MonsterHunterCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BracePower>(4M),
        new StringVar("Consumable")
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        BracePower bracePower = await PowerCmd.Apply<BracePower>(choiceContext, Owner.Creature,
            DynamicVars.Power<BracePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Power<BracePower>().UpgradeValueBy(3M);
}