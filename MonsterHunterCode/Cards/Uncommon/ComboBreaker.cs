using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class ComboBreaker() : MonsterHunterCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override bool ShouldGlowGoldInternal => WasLastCardCombo;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7M, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_gaze");
        var blockGains = WasLastCardCombo ? 2 : 1;
        for (var i = 0; i < blockGains; ++i)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3M);

    public bool WasLastCardCombo
    {
        get
        {
            var lastCardPlayed = CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>()
                .LastOrDefault((e => e.HappenedThisTurn(CombatState) && e.Actor == Owner.Creature));

            if (lastCardPlayed == null)
                return false;
            
            return lastCardPlayed.CardPlay.Card.DynamicVars.ContainsKey("Combo") &&
                   !lastCardPlayed.CardPlay.Card.DynamicVars.ContainsKey("Finisher");
        }
    }
}