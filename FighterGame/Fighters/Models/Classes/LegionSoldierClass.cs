namespace Fighters.Models.Classes;

public class LegionSoldierClass : IClass
{
    public string Name => "Легионер";
    public int Damage => 12;
    public int Health => 30;
    public int Armor => 6;
    public int Initiative => 12;
}
