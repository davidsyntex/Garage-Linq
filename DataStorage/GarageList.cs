using System.Collections.Generic;

namespace Garage_Linq.DataStorage
{
    internal static class GarageList<T>
    {
        public static List<T> InternalStorage { get; set; } = new List<T>();
    }
}
