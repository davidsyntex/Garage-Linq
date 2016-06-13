using System;
using System.Text;

namespace Garage_Linq.Commands
{
    public class Garage
    {
        public static string GetAll()
        {
            var sb = new StringBuilder();
            foreach (var vehicle in Program.GarageController.GetAllVehicles())
            {
                Console.WriteLine(vehicle.GetType().Name.ToUpper());

                foreach (var propertyInfo in vehicle.GetType().GetProperties())
                {
                    Console.WriteLine($"{propertyInfo.Name}: {propertyInfo.GetValue(vehicle)}");
                }
                Console.WriteLine();
            }
            return sb.ToString();
        }
    }
}