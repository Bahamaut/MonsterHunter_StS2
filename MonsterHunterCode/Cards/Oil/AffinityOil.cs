using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MonsterHunter.MonsterHunterCode.Cards.Basic;
using MonsterHunter.MonsterHunterCode.Powers.Oil;


namespace MonsterHunter.MonsterHunterCode.Cards.Oil;

[Pool(typeof(TokenCardPool))]
public class AffinityOil()
    : MonsterHunterCard(-1, CardType.None, CardRarity.None, TargetType.Self), WeaponOil.IChoosable
{
    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AffinityOilPower>(1M)];

    public async Task OnChosen(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<AffinityOilPower>(choiceContext, Owner.Creature,
            DynamicVars.Power<AffinityOilPower>().BaseValue, Owner.Creature, this);
    }
}