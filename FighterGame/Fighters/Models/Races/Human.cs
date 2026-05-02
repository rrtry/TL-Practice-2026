namespace Fighters.Models.Races;

public class Human : IRace
{
    public string Name => "Человек";
    public int Damage => 5;
    public int Health => 100;
    public int Armor => 2;
    public int Initiative => 10;
}
