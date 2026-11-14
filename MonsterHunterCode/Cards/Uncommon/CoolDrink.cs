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
        new PowerVar<StretchPower>(4M),
        new StringVar("Consumable")
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        StretchPower stretchPower = await PowerCmd.Apply<StretchPower>(choiceContext, Owner.Creature,
            DynamicVars.Power<StretchPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Power<StretchPower>().UpgradeValueBy(3M);
}