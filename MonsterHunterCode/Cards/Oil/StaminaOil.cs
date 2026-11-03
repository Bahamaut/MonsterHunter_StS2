using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MonsterHunter.MonsterHunterCode.Cards.Basic;
using MonsterHunter.MonsterHunterCode.Powers;
using MonsterHunter.MonsterHunterCode.Powers.Oil;

namespace MonsterHunter.MonsterHunterCode.Cards.Oil;

[Pool(typeof(TokenCardPool))]
public class StaminaOil() : MonsterHunterCard(-1,
    CardType.None, CardRarity.None, TargetType.Self), WeaponOil.IChoosable
{
    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StaminaOilPower>(1M)];

    public async Task OnChosen(PlayerChoiceContext choiceContext)
    {
        StaminaOilPower staminaOilPower = await PowerCmd.Apply<StaminaOilPower>(choiceContext, this.Owner.Creature,
            this.DynamicVars[nameof(StaminaOilPower)].BaseValue, this.Owner.Creature, (CardModel)this);
    }
}