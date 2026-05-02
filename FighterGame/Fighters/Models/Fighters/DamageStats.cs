namespace Fighters.Models.Fighters;

public struct DamageStats
{
    public bool IsCritical { get; }
    public int Damage { get; }

    public DamageStats( bool isCritical, int damage )
    {
        IsCritical = isCritical;
        Damage = damage;
    }
}
