using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Token;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class Chop() : MonsterHunterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4M, ValueProp.Move),
        new PowerVar<VulnerablePower>(1M),
        new StringVar("Combo")
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<SideSlash>(),
        HoverTipFactory.FromCard<DiagonalRisingSlash>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard((CardModel)this, cardPlay)
            .Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab").Execute(choiceContext);
        VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,
            this.DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, (CardModel)this);

        SideSlash card = ((CardModel)this).CombatState.CreateCard<SideSlash>(((CardModel)this).Owner);
        if (((CardModel)this).IsUpgraded)
        {
            CardCmd.Upgrade((CardModel)(object)card);
        }

        await CardPileCmd.AddGeneratedCardToCombat((CardModel)(object)card, PileType.Hand, ((CardModel)this).Owner);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(2M);
        this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}