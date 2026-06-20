using CarFactory.Models;

namespace CarFactory.UI;

public interface ICarCreator
{
    Car CreateCar();
    bool AskToContinue();
}