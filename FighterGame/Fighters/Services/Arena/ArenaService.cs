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

    public void SimulateBattle()
    {
        if ( _fighters.Count() < 2 )
        {
            _env.WriteLine( ApplicationMessages.ArenaNotEnoughFighters );
            return;
        }

        int round = 0;
        _alive = _fighters.FindAll( f => f.GetCurrentHealth() > 0 );

        while ( _alive.Count > 1 && round <= MAX_ROUNDS )
        {
            round++;
            _env.WriteLine( ApplicationMessages.ArenaRoundHeader( round ) );
            SimulateRound();
        }

        PrintBattleResults();

        _fighters.Clear();
        _alive.Clear();
    }

    public void AddFighter( IFighter fighter )
    {
        _fighters.Add( fighter );
        _env.WriteLine( ApplicationMessages.ArenaFighterAdded( fighter.Name ) );
    }

    public void RemoveFighter()
    {
        _env.Write( ApplicationMessages.ArenaPromptRemoveFighter );
        int removeAt = EnvironmentUtils.ReadIntInRange( _env, 1, _fighters.Count() );
        _env.WriteLine( ApplicationMessages.ArenaFighterRemoved( removeAt ) );
        _fighters.RemoveAt( removeAt - 1 );
    }

    public void ListFighters()
    {
        _env.WriteLine( ApplicationMessages.ArenaFightersCount( _fighters.Count() ) );
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

        _env.WriteLine( ApplicationMessages.ArenaAttackMessage( attacker.Name, target.Name, damageStats.Damage, damageStats.IsCritical ) );
    }

    private void PrintRoundDamageStats(
        List<IFighter> turnOrder,
        Dictionary<IFighter, int> damageDealt,
        Dictionary<IFighter, int> damageReceived
    )
    {
        foreach ( var fighter in turnOrder )
        {
            int dealt = damageDealt.ContainsKey( fighter ) ? damageDealt[ fighter ] : 0;
            int received = damageReceived.ContainsKey( fighter ) ? damageReceived[ fighter ] : 0;

            _env.WriteLine( ApplicationMessages.ArenaRoundDamageStats( fighter.Name, dealt, received ) );
        }
    }

    private void UpdateRoundSurvivors()
    {
        var survived = _fighters.Where( f => f.GetCurrentHealth() > 0 ).ToList();
        foreach ( var dead in _alive.Except( survived ) )
        {
            _env.WriteLine( ApplicationMessages.ArenaFighterDied( dead.Name ) );
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
            _env.WriteLine( ApplicationMessages.ArenaWinner( _alive.First().Name ) );
        }
        else
        {
            IFighter winner = _alive.OrderByDescending( fighter => fighter.GetCurrentHealth() ).First();
            _env.WriteLine( ApplicationMessages.ArenaMaxRoundsExhausted( winner.Name ) );
        }
    }
}
