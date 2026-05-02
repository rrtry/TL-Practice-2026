namespace Fighters.Models.Weapons;

public interface IWeapon : IDisplayable
{
    public int Damage { get; }
};