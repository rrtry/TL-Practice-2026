namespace Fighters.Models.Classes;

public interface IClass : INamed
{
    int Damage { get; }
    int Health { get; }
    int Armor { get; }
    int Initiative { get; }
}