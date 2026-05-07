using System.Security.Cryptography.X509Certificates;

namespace CarFactory.Models.Interfaces;

public interface IBody : IDisplay
{
    double AerodynamicFactor { get; }
}