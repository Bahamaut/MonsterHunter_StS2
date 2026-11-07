using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class BlastWeapon() : MonsterHunterCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BlastWeaponPower>(1M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlastPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        BlastWeaponPower blastWeaponPower = await PowerCmd.Apply<BlastWeaponPower>(choiceContext, this.Owner.Creature, this.DynamicVars["BlastWeaponPower"].BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade() => this.DynamicVars["BlastWeaponPower"].UpgradeValueBy(1M);
}