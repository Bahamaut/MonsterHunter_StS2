using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Token;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class ChargingSlash() : MonsterHunterCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4M, ValueProp.Move),
        new EnergyVar(1),
        new StringVar("Combo")
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<ScalingSlash>(),
        HoverTipFactory.FromCard<PlungingThrust>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull((object)play.Target, "play.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play)
            .Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, play.Card.Owner); 

        ScalingSlash card = ((CardModel)this).CombatState.CreateCard<ScalingSlash>(((CardModel)this).Owner);
        if (((CardModel)this).IsUpgraded)
        {
            CardCmd.Upgrade((CardModel)(object)card);
        }

        await CardPileCmd.AddGeneratedCardToCombat((CardModel)(object)card, PileType.Hand, ((CardModel)this).Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}