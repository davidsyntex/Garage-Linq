using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Garage_Linq.DataStorage;
using Garage_Linq.Models;

namespace Garage_Linq.DataAccess
{
    internal interface IDataAccess<T>
    {
        List<T> GetAccess();
    }

    internal class DataAccessFactory
    {
        public static IDataAccess<T> GetInstance<T>(DataAccessTypes dat)
        {
            switch (dat)
            {
                case DataAccessTypes.Internal:
                    return new InternalDataAccess<T>();
                case DataAccessTypes.Database:
                    throw new NotImplementedException();
                default:
                    break;
            }
            return null;
        }
    }

    internal class InternalDataAccess<T> : IDataAccess<T>
    {
        public InternalDataAccess()
        {
            Debug.WriteLine("Internal DB Setup");
        }

        public static List<Vehicle> GetAccess()
        {
            return GarageList<Vehicle>.InternalStorage;
        }

        List<T> IDataAccess<T>.GetAccess()
        {
            return GarageList<T>.InternalStorage;
        }
    }

    public enum DataAccessTypes
    {
        Internal,
        Database
    }
}