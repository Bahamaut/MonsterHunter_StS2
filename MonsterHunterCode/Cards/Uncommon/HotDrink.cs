using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class HotDrink() : MonsterHunterCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VigorPower>(4M),
        new StringVar("Consumable")
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature,
            DynamicVars.Power<VigorPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Power<VigorPower>().UpgradeValueBy(3M);
}