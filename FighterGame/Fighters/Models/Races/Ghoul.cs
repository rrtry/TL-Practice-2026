namespace Fighters.Models.Races;

public class Ghoul : IRace
{
    public string Name => "Гуль";
    public int Damage => 6;
    public int Health => 110;
    public int Armor => 2;
    public int Initiative => 8;
}
