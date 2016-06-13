using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Garage_Linq.DataAccess;
using Garage_Linq.Exceptions;
using Garage_Linq.Models;

namespace Garage_Linq.Controllers
{
    internal class GarageController<T> : IEnumerable<T> where T : Vehicle
    {
        private readonly int _maxVehicles;
        private readonly List<Vehicle> _internalStorage;
 

        public GarageController(int maxVehicles)
        {
            _maxVehicles = maxVehicles;
            _internalStorage = DataAccessFactory.GetInstance<Vehicle>(DataAccessTypes.Internal).GetAccess();
        }

        private bool IsVehicleMaxLimitReached()
        {
            return _internalStorage.Count == _maxVehicles;
        }

        public bool Add(T t)
        {
            try
            {
                if (IsVehicleMaxLimitReached()) throw new VehicleMaxLimitException();
                _internalStorage.Add(t);
                return true;
            }
            catch (VehicleMaxLimitException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Remove(int index)
        {
            try
            {
                _internalStorage.RemoveAt(index);
                return true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Remove(T t)
        {
            try
            {
                _internalStorage.Remove(t);
                return true;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<T> GetAllVehicles()
        {
            return (IEnumerable<T>) _internalStorage.ToList();
        }

        public T FindCar(string registernumberToFind)
        {
            return (T) _internalStorage.FirstOrDefault(vehicle => vehicle.RegisterNumber.Equals(registernumberToFind));
        }

        public IEnumerable<T> FindVehiclesByColor(string colorToFind)
        {
            return (IEnumerable<T>) _internalStorage.Where(vehicle => vehicle.Color.Equals(colorToFind)).ToList();
        }

        public IEnumerable<T> FindVehiclesByNumberOfWheels(int numberOfWheelsToFind)
        {
            return (IEnumerable<T>) _internalStorage.Where(vehicle => vehicle.NumberOfWeels == numberOfWheelsToFind).ToList();
        }

        public List<string> GetTypesOfVehicles()
        {
            var types = new List<string>();

            foreach (var vehicle in _internalStorage)
            {
                if (!types.Contains(vehicle.GetType().Name))
                {
                    types.Add(vehicle.GetType().Name);
                }
            }

            return types;
        }


        public Dictionary<string, int> GetDictionaryTypesOfVehicles()
        {
            var types = new Dictionary<string, int>();

            foreach (var vehicle in _internalStorage)
            {
                if (types.ContainsKey(vehicle.GetType().Name))
                {
                    types[vehicle.GetType().Name]++;
                }
                else
                {
                    types.Add(vehicle.GetType().Name, 1);
                }
            }

            return types;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _internalStorage).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}