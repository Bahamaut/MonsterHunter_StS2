using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MonsterHunter.MonsterHunterCode.Cards;

namespace MonsterHunter.MonsterHunterCode.Cards.Common;

public class WatchTheEyes() : MonsterHunterCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return this.CombatState != null && this.CombatState.HittableEnemies.Any<Creature>((Func<Creature, bool>) (e =>
            {
                MonsterModel monster = e.Monster;
                return monster != null && monster.IntendsToAttack;
            }));
        }
    }
    
    public override bool GainsBlock => true;
    
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        new BlockVar(3M, ValueProp.Move),
        new PowerVar<WeakPower>(1M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        if (!cardPlay.Target.Monster.IntendsToAttack)
            return;
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, this.DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(1M);
        this.DynamicVars.Weak.UpgradeValueBy(1M);
    }
}