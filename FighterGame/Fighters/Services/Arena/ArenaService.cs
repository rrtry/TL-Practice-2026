using Fighters.Models.Fighters;
using Fighters.Services.Environment;
using Fighters.Services.Randomization;
using Fighters.Utils;

namespace Fighters.Services.Arena;

public class ArenaService
{
    private const int MAX_ROUNDS = 100;

    private readonly IEnvironmentService _env;
    private readonly IRandomService _rand;

    private readonly List<IFighter> _fighters = new();
    private List<IFighter> _alive = new();

    public IReadOnlyList<IFighter> Fighters => _fighters;

    public ArenaService( IEnvironmentService env, IRandomService rand )
    {
        _env = env;
        _rand = rand;
    }

    public List<IFighter> GetFighters() => _fighters;

    public void SimulateBattle()
    {
        if ( _fighters.Count() < 2 )
        {
            _env.WriteLine( "Добавьте как минимум 2-ух бойцов на арену" );
            return;
        }

        int round = 0;
        _alive = _fighters.FindAll( f => f.GetCurrentHealth() > 0 );

        while ( _alive.Count > 1 && round <= MAX_ROUNDS )
        {
            round++;
            _env.WriteLine( $"\nРаунд {round}" );
            SimulateRound();
        }

        PrintBattleResults();

        _fighters.Clear();
        _alive.Clear();
    }

    public void AddFighter( IFighter fighter )
    {
        _fighters.Add( fighter );
        _env.WriteLine( $"Боец {fighter.Name} добавлен на арену!" );
    }

    public void RemoveFighter()
    {
        _env.Write( "Ввёдите номер бойца: " );
        int removeAt = EnvironmentUtils.ReadIntInRange( _env, 1, _fighters.Count() );
        _env.WriteLine( $"Боец под номером {removeAt} удалён" );
        _fighters.RemoveAt( removeAt - 1 );
    }

    public void ListFighters()
    {
        _env.WriteLine( $"\nКол-во бойцов: {_fighters.Count()}" );
        for ( int i = 0; i < _fighters.Count(); i++ )
        {
            IFighter fighter = _fighters[ i ];
            _env.Write( $"{i + 1} - " );
            _env.Write( fighter.ToString()! );
        }
    }

    private bool SelectOpponent( List<IFighter> turnOrder, IFighter fighter, out IFighter? opponent )
    {
        var opponents = turnOrder.Where( f => f != fighter && f.GetCurrentHealth() > 0 ).ToList();
        if ( opponents.Count == 0 )
        {
            opponent = null;
            return false;
        }

        opponent = opponents[ _rand.Next( opponents.Count ) ];

        return true;
    }

    private void UpdateRoundDamageStats(
        Dictionary<IFighter, int> damageDealt,
        Dictionary<IFighter, int> damageReceived,
        IFighter attacker,
        IFighter target
    )
    {
        DamageStats damageStats = target.TakeDamage( _rand, attacker );

        damageDealt[ attacker ] = damageDealt.GetValueOrDefault( attacker ) + damageStats.Damage;
        damageReceived[ target ] = damageReceived.GetValueOrDefault( target ) + damageStats.Damage;

        _env.WriteLine( $"{attacker.Name} атакует {target.Name} и наносит {damageStats.Damage} урона" +
                          ( damageStats.IsCritical ? " (критический удар!)" : "" ) );
    }

    private void PrintRoundDamageStats(
        List<IFighter> turnOrder,
        Dictionary<IFighter, int> damageDealt,
        Dictionary<IFighter, int> damageReceived
    )
    {
        foreach ( var fighter in turnOrder )
        {
            string dealt = damageDealt.ContainsKey( fighter ) ? damageDealt[ fighter ].ToString() : "0";
            string received = damageReceived.ContainsKey( fighter ) ? damageReceived[ fighter ].ToString() : "0";
            _env.WriteLine( $"{fighter.Name} наносит {dealt} урона, получает {received}" );
        }
    }

    private void UpdateRoundSurvivors()
    {
        var survived = _fighters.Where( f => f.GetCurrentHealth() > 0 ).ToList();
        foreach ( var dead in _alive.Except( survived ) )
        {
            _env.WriteLine( $"{dead.Name} погибает!" );
        }

        _alive = survived;
    }

    private void SimulateRound()
    {
        List<IFighter> turnOrder = _alive.OrderByDescending( f => f.GetInitiative() ).ToList();

        Dictionary<IFighter, int> damageDealt = new Dictionary<IFighter, int>();
        Dictionary<IFighter, int> damageReceived = new Dictionary<IFighter, int>();

        foreach ( IFighter fighter in turnOrder )
        {
            if ( fighter.GetCurrentHealth() <= 0 )
            {
                continue;
            }

            IFighter? target;
            if ( !SelectOpponent( turnOrder, fighter, out target ) )
            {
                break;
            }

            UpdateRoundDamageStats( damageDealt, damageReceived, fighter, target! );
        }

        PrintRoundDamageStats( turnOrder, damageDealt, damageReceived );
        UpdateRoundSurvivors();
    }

    private void PrintBattleResults()
    {
        if ( _alive.Count == 1 )
        {
            _env.WriteLine( $"\n{_alive[ 0 ].Name} выживает и побеждает!" );
        }
        else if ( _alive.Count == 0 )
        {
            _env.WriteLine( "\nНичья! Все бойцы погибли." );
        }
        else
        {
            IFighter winner = _alive.OrderByDescending( fighter => fighter.GetCurrentHealth() ).First();
            _env.WriteLine( $"\nЛимит раундов исчерпан. Самым живучим оказался {winner.Name}!" );
        }
    }
}
