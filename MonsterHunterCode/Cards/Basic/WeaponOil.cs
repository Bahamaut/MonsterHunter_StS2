using System.Collections.ObjectModel;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Oil;

namespace MonsterHunter.MonsterHunterCode.Cards.Basic;

public class WeaponOil() : MonsterHunterCard(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(4)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<AffinityOil>(),
        HoverTipFactory.FromCard<StaminaOil>(),
        HoverTipFactory.FromCard<DestroyerOil>(),
        HoverTipFactory.FromCard<MindsEyeOil>()
    ];
    
    public static readonly IReadOnlyList<IChoosable> _weaponOils =
        new ReadOnlyCollection<IChoosable>(
        [
            ModelDb.Card<AffinityOil>(),
            ModelDb.Card<StaminaOil>(),
            ModelDb.Card<DestroyerOil>(),
            ModelDb.Card<MindsEyeOil>()
        ]);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext,
            (IReadOnlyList<CardModel>)_weaponOils.Select<IChoosable, CardModel>((Func<IChoosable, CardModel>)(
                c =>
                {
                    CardModel card = CombatState.CreateCard((CardModel)c, this.Owner);
                    return card;
                })).ToList(), this.Owner);
        if (cardModel == null)
            return;
        await ((IChoosable)cardModel).OnChosen(choiceContext);
    }

    public interface IChoosable
    {
        Task OnChosen(PlayerChoiceContext choiceContext);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}