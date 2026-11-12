using BaseLib.Common.Rewards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class ImprovementThroughIteration() : MonsterHunterCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (!(this.CombatState.RunState.CurrentRoom is CombatRoom combatRoom))
        {
            combatRoom = (CombatRoom) null;
        }
        else
        {
            ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
            bool shouldTriggerFatal = cardPlay.Target.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()));
            AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
            if (!shouldTriggerFatal)
                combatRoom = (CombatRoom) null;
            else if (!attackCommand.Results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))
            {
                combatRoom = (CombatRoom) null;
            }
            else
            {
                combatRoom.AddExtraReward(this.Owner, new RandomCardUpgradeReward(this.Owner));
                //TheHuntPower theHuntPower = await PowerCmd.Apply<TheHuntPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
                combatRoom = (CombatRoom) null;
            }
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}