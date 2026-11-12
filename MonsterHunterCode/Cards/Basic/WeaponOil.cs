using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards.Oil;
using MonsterHunter.MonsterHunterCode.Commands;
using MonsterHunter.MonsterHunterCode.Powers.Oil;

namespace MonsterHunter.MonsterHunterCode.Cards.Basic;

public class WeaponOil() : MonsterHunterCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<AffinityOilPower>(),
        HoverTipFactory.FromPower<StaminaOilPower>(),
        HoverTipFactory.FromPower<DestroyerOilPower>(),
        HoverTipFactory.FromPower<MindsEyeOilPower>()
    ];

    private static IReadOnlyList<IChoosable> WeaponOils =>
        [
            ModelDb.Card<AffinityOil>(),
            ModelDb.Card<StaminaOil>(),
            ModelDb.Card<DestroyerOil>(),
            ModelDb.Card<MindsEyeOil>()
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        
        var oilList = await MonsterHunterCardSelectCmd.FromChooseACardScreen(choiceContext,
            WeaponOils.Select(c => CombatState.CreateCard((CardModel)c, cardPlay.Player)).ToList(), cardPlay.Player);
        if (oilList == null)
            return;
        
        await ((IChoosable)oilList).OnChosen(choiceContext);
    }

    public interface IChoosable
    {
        Task OnChosen(PlayerChoiceContext choiceContext);
    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}