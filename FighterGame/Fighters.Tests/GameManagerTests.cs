using Fighters.Models.Fighters;
using Fighters.Models.Races;

namespace Fighters.Tests
{
    [TestFixture]
    public class GameManagerTests
    {
        [Test]
        public void Play_TwoEqualFighters_FirstFighterWins()
        {
            // Arrange 
            var gameManager = new GameManager();
            var fighterA = new Knight("FighterA", new Human());
            var fighterB = new Knight("FighterB", new Human());

            // Act
            var winner = gameManager.Play(fighterA, fighterB);

            // Asssert
            Assert.That(winner.Name, Is.EqualTo(fighterA.Name));
        }

        [Test]
        public void Play_TwoEqualFighters_SecondFighterDies()
        {
            // Arrange 
            var gameManager = new GameManager();
            var fighterA = new Knight("FighterA", new Human());
            var fighterB = new Knight("FighterB", new Human());

            // Act
            gameManager.Play(fighterA, fighterB);

            // Asssert
            Assert.That(fighterA.GetCurrentHealth(), Is.GreaterThan(0)); 
            Assert.That(fighterB.GetCurrentHealth(), Is.EqualTo(0));
        }
    }
}
