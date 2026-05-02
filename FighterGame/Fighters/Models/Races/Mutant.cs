namespace Fighters.Models.Races;

public class Mutant : IRace
{
    public string Name => "Мутант";
    public int Damage => 15;
    public int Health => 120;
    public int Armor => 8;
    public int Initiative => 6;
}
