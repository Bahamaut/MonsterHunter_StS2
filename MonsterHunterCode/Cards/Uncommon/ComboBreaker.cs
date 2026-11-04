using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Uncommon;

public class ComboBreaker() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override bool ShouldGlowGoldInternal => this.WasLastCardCombo;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(7M, ValueProp.Move)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_gaze");
        int blockGains = this.WasLastCardCombo ? 2 : 1;
        for (int i = 0; i < blockGains; ++i)
        {
            Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);

    public bool WasLastCardCombo
    {
        get
        {
            var lastCardPlayed = CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>()
                .Last((e => e.HappenedThisTurn(this.CombatState) && e.Actor == this.Owner.Creature));

            return lastCardPlayed.CardPlay.Card.DynamicVars.ContainsKey("Combo") &&
                   !lastCardPlayed.CardPlay.Card.DynamicVars.ContainsKey("Finisher");
        }
    }
}