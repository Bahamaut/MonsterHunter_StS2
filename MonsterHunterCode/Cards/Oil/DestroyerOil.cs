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
using MonsterHunter.MonsterHunterCode.Powers.Oil;
using MonsterHunter.MonsterHunterCode.Relics;

namespace MonsterHunter.MonsterHunterCode.Cards.Oil;

[Pool(typeof(TokenCardPool))]
public class DestroyerOil() : MonsterHunterCard(-1,
    CardType.None, CardRarity.None, TargetType.Self), WeaponOil.IChoosable
{
    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DestroyerOilPower>(1M)];

    public async Task OnChosen(PlayerChoiceContext choiceContext)
    {
        DestroyerOilPower destroyerOil = await PowerCmd.Apply<DestroyerOilPower>(choiceContext, this.Owner.Creature,
            this.DynamicVars[nameof(DestroyerOilPower)].BaseValue, this.Owner.Creature, (CardModel)this);
    }
}