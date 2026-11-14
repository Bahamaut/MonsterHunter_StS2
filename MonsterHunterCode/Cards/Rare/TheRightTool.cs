using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Rare;

public class TheRightTool() : MonsterHunterCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        ForegoneConclusionPower foregoneConclusionPower = await PowerCmd.Apply<ForegoneConclusionPower>(choiceContext, this.Owner.Creature, this.DynamicVars.Cards.BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}