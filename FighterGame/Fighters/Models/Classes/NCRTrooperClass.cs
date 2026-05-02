namespace Fighters.Models.Classes;

public class NCRTrooperClass : IClass
{
    public string Name => "Солдат НКР";
    public int Damage => 10;
    public int Health => 25;
    public int Armor => 8;
    public int Initiative => 10;
}
