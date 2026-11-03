using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Token;

[Pool(typeof(TokenCardPool))] 
public class HealthPotion() : MonsterHunterCard(0,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        new BlockVar(5M, ValueProp.Move),
        new HealVar(3M)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);
        await CreatureCmd.Heal(Owner.Creature, this.DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Heal.UpgradeValueBy(1M);
    }
    
    public static async Task<CardModel?> CreateInHand(
        Player owner,
        ICombatState combatState,
        Player? creator = null)
    {
        return (await HealthPotion.CreateInHand(owner, 1, combatState, creator)).FirstOrDefault<CardModel>();
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        ICombatState combatState,
        Player? creator = null)
    {
        if (count == 0)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        List<CardModel> healthPotions = new List<CardModel>();
        for (int index = 0; index < count; ++index)
            healthPotions.Add((CardModel) combatState.CreateCard<HealthPotion>(owner));
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) healthPotions, PileType.Hand, creator ?? owner);
        return (IEnumerable<CardModel>) healthPotions;
    }
}