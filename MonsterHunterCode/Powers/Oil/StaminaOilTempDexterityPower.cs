using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MonsterHunter.MonsterHunterCode.Cards.Oil;

namespace MonsterHunter.MonsterHunterCode.Powers.Oil;

public class StaminaOilTempDexterityPower() : TemporaryDexterityPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<StaminaOil>();
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
    }
}