namespace Garage_Linq.Models
{
    internal abstract class Vehicle
    {
        protected Vehicle(string registerNumber, string color, string owner, int numberOfWeels)
        {
            RegisterNumber = registerNumber;
            Color = color;
            NumberOfWeels = numberOfWeels;
            Owner = owner;
        }

        public string Owner { get; protected set; }
        public string RegisterNumber { get; protected set; }
        public string Color { get; protected set; }
        public int NumberOfWeels { get; protected set; }
    }

    internal class Boat : Vehicle
    {
        public Boat(string registerNumber, string color, string owner, int numberOfMasts, int numberOfWeels = 0)
            : base(registerNumber, color, owner, numberOfWeels)
        {
            NumberOfMasts = numberOfMasts;
        }

        public int NumberOfMasts { get; protected set; }
    }

    internal class Buss : Vehicle
    {
        public Buss(string registerNumber, string color, string owner, bool isStopButtonPressed = false,
            int numberOfWeels = 6) : base(registerNumber, color, owner, numberOfWeels)
        {
            IsStopButtonPressed = isStopButtonPressed;
        }

        public bool IsStopButtonPressed { get; protected set; }
    }

    internal class Car : Vehicle
    {
        public Car(string registerNumber, string color, string owner, bool hasElectricWindows, int numberOfWeels = 4)
            : base(registerNumber, color, owner, numberOfWeels)
        {
            HasElectricWindows = hasElectricWindows;
        }

        public bool HasElectricWindows { get; protected set; }
    }

    internal class MotorCycle : Vehicle
    {
        public MotorCycle(string registerNumber, string color, string owner, bool hasSideCart, int numberOfWeels = 2)
            : base(registerNumber, color, owner, numberOfWeels)
        {
            HasSideCart = hasSideCart;
        }

        public bool HasSideCart { get; protected set; }
    }

    internal class Airplane : Vehicle
    {
        public int NumberOfEngines { get; protected set; }

        public Airplane(string registerNumber, string color, string owner, int numberOfEngines, int numberOfWeels)
            : base(registerNumber, color, owner, numberOfWeels)
        {
            NumberOfEngines = numberOfEngines;
        }
    }
}