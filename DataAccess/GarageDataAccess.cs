using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Linq.DataAccess
{
    class GarageDataAccess
    {
        internal class InternalList : GarageDataAccess
        {
        }
    }

    public enum DataStorage
    {
        InternalList,
        Database
    }
}
