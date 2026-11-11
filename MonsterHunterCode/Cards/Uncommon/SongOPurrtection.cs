using BaseLib.Extensions;
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

public class SongOPurrtection() : MonsterHunterCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<SongOPurrtectionPower>(2M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<BracePower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        SongOPurrtectionPower songOPurrtectionPower = await PowerCmd.Apply<SongOPurrtectionPower>(choiceContext, this.Owner.Creature,
            this.DynamicVars.Power<SongOPurrtectionPower>().BaseValue, this.Owner.Creature, (CardModel)this);
    }

    protected override void OnUpgrade() => DynamicVars.Power<SongOPurrtectionPower>().UpgradeValueBy(2M);
}