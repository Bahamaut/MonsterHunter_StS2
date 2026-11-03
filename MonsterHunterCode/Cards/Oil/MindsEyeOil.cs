using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Basic;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Oil;

[Pool(typeof(TokenCardPool))]
public class MindsEyeOil() : MonsterHunterCard(-1,
    CardType.None, CardRarity.None, TargetType.Self), WeaponOil.IChoosable
{
    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MindsEyeOilPower>(1M)];

    public async Task OnChosen(PlayerChoiceContext choiceContext)
    {
        MindsEyeOilPower mindsEyeOilPower = await PowerCmd.Apply<MindsEyeOilPower>(choiceContext, this.Owner.Creature,
            this.DynamicVars[nameof(MindsEyeOilPower)].BaseValue, this.Owner.Creature, (CardModel)this);
    }
}