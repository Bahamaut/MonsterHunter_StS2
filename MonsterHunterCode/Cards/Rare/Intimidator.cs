using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class Intimidator() : MonsterHunterCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    private const string _strengthLossKey = "StrengthLoss";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("StrengthLoss", 1M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        MonarchsGazePower monarchsGazePower = await PowerCmd.Apply<MonarchsGazePower>(choiceContext,
            this.Owner.Creature, this.DynamicVars["StrengthLoss"].BaseValue, this.Owner.Creature, (CardModel)this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}