using System.Collections.Generic;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using MonsterHunter.MonsterHunterCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MonsterHunter.MonsterHunterCode.Cards;
using MonsterHunter.MonsterHunterCode.Cards.Basic;
using MonsterHunter.MonsterHunterCode.Cards.Rare;
using MonsterHunter.MonsterHunterCode.Cards.Uncommon;
using MonsterHunter.MonsterHunterCode.Relics;

namespace MonsterHunter.MonsterHunterCode.Character;

public class MonsterHunter : PlaceholderCharacterModel
{
    public const string CharacterId = "Monster Hunter";

    public static readonly Color Color = new("#3D9191");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<AdvancingSlash>(),
        ModelDb.Card<StrikeMonsterHunter>(),
        ModelDb.Card<StrikeMonsterHunter>(),
        ModelDb.Card<StrikeMonsterHunter>(),
        ModelDb.Card<StrikeMonsterHunter>(),
        ModelDb.Card<DefendMonsterHunter>(),
        ModelDb.Card<DefendMonsterHunter>(),
        ModelDb.Card<DefendMonsterHunter>(),
        ModelDb.Card<DefendMonsterHunter>(),
        ModelDb.Card<WeaponOil>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<LowRankQuestBoard>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<MonsterHunterCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<MonsterHunterRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<MonsterHunterPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}