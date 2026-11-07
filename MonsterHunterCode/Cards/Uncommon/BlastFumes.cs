using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class BlastFumes() : MonsterHunterCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    private const string _blastPerTurnKey = "BlastPerTurn";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("BlastPerTurn", 2M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        BlastFumesPower blastFumesPower = await PowerCmd.Apply<BlastFumesPower>(choiceContext,
            this.Owner.Creature, this.DynamicVars["BlastPerTurn"].BaseValue, this.Owner.Creature, (CardModel)this);
    }

    protected override void OnUpgrade() => this.DynamicVars["BlastPerTurn"].UpgradeValueBy(1M);
}