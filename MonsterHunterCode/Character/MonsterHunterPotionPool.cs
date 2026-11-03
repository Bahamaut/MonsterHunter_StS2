using BaseLib.Abstracts;
using MonsterHunter.MonsterHunterCode.Extensions;
using Godot;

namespace MonsterHunter.MonsterHunterCode.Character;

public class MonsterHunterPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => MonsterHunter.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}