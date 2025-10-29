using System;
using System.Reflection;

namespace CarShowroom
{
    // TODO: Apply encapsulation
    // Create a class Car with private fields and public methods
    public class Car
    {
        // STEP 1: Declare private fields for model, fuel, capacity, and consumption
        private string model;
        private double fuel;
        private int capacity;
        private double consumption;

        public double Fuel { get => fuel; set => fuel = value; }
        public double Consumption { get => consumption; set => consumption = value; }
        // STEP 2: Create a constructor that sets these values
        public Car(string carModel, double carFuel, int carCapacity, double carConsumption)
        {
            model = carModel;
            fuel = carFuel;
            capacity = carCapacity;
            consumption = carConsumption;
        }
        // STEP 3: Create public methods Refuel(double liters) and Drive(double km)
        // - Refuel: increase fuel up to capacity only
        public virtual void Refuel(double liters)
        {
            if (fuel+liters > capacity)
            {
                fuel = capacity;
                Console.WriteLine("Reached the maximum capacity of the car.");
            }
            else
            {
                fuel += liters;
            }
            Console.WriteLine($"{model} refueled to {fuel:F2}/{capacity}L");
        }
        // - Drive: decrease fuel based on consumption
        public virtual void Drive(double km)
        {
            double fuelConsump = (consumption/100) * km;

            if (fuelConsump > fuel)
            {
                Console.WriteLine($"{model} out of fuel.");
            }
            else
            {
                fuel -= fuelConsump;
                Console.WriteLine($"{model} drove {km}km, used {fuelConsump:F2}L -> {fuel:F2}L left");
            }
        }
        // - Display results using Console.WriteLine()

        // STEP 4: Add a ToString() method to show model info
        public override string ToString()
        {
            return $"{model} | fuel: {fuel:F2}/{capacity}L | Consumption; {consumption:F1}L/100km";
        }
    }

    // TODO: Apply inheritance
    // Create Toyota and SportsCar classes that inherit from Car
    public class Toyota : Car
    {
        
        public Toyota()
            : base("Toyota", 0, 45, 6.5) 
        {
        }
        public override void Refuel(double liters)
        {
            base.Refuel(liters);
        }
        public override void Drive(double km)
        {
            base.Drive(km);
        }
    }

    public class SportsCar : Car
    {
        public SportsCar()
            : base("SportsCar", 0, 60, 12)
        {
        }
        public override void Refuel(double liters)
        {
            base.Refuel(liters);
        }
        public override void Drive(double km)
        {
            Console.WriteLine("SportsCar drives aggressively!");
            base.Drive(km);
        }
    }
    // HINT: Toyota and SportsCar should have different default fuel capacities and consumptions
    // Toyota might be 45L capacity, 6.5L per 100km
    // SportsCar might be 60L capacity, 12L per 100km

    // TODO: Apply polymorphism
    // Override Drive() in SportsCar to print "SportsCar drives aggressively!"

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Car Showroom Simulator ===\n");

            // STEP 1: Create one Toyota and one SportsCar object
            // Example:
            Car toyota = new Toyota();
            Car sportscar = new SportsCar();

            // STEP 2: Refuel both cars
            // Example:
            toyota.Refuel(30);
            sportscar.Refuel(50);

            // STEP 3: Simulate driving
            // Example:
            toyota.Drive(100);
            sportscar.Drive(100);

            // STEP 4: Print final details
            // Example:
            Console.WriteLine();
            Console.WriteLine(toyota);
            Console.WriteLine(sportscar);

            Console.WriteLine("\n--- Fleet loop (Polymorphism via base type) ---");

            Car[] fleet = { toyota, sportscar };
            foreach (Car car in fleet)
            {
                car.Refuel(20);
                car.Drive(100);
            }

            Console.WriteLine("\n=== End of Simulation ===");
        }
    }
}
