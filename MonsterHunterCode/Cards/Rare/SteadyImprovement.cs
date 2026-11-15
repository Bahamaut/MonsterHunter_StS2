using BaseLib.Common.Rewards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Powers;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class SteadyImprovement() : MonsterHunterCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState?.RunState.CurrentRoom is CombatRoom combatRoom)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var shouldTriggerFatal = cardPlay.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
            var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard((CardModel)this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            if (shouldTriggerFatal && attackCommand.Results.SelectMany(r => r)
                    .Any((Func<DamageResult, bool>)(r => r.WasTargetKilled)))
            {
                combatRoom.AddExtraReward(Owner, new RandomCardUpgradeReward(Owner));
                await PowerCmd.Apply<SteadyImprovementPower>(choiceContext, Owner.Creature,
                    1M, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(5M);
}