using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MonsterHunter.MonsterHunterCode.Character;
using MonsterHunter.MonsterHunterCode.Extensions;

namespace MonsterHunter.MonsterHunterCode.Potions;

[Pool(typeof(MonsterHunterPotionPool))]
public abstract class MonsterHunterPotion : CustomPotionModel
{
    public override string? CustomPackedImagePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();

    public override string? CustomPackedOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}