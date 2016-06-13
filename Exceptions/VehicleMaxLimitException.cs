using System;

namespace Garage_Linq.Exceptions
{
    internal class VehicleMaxLimitException : Exception
    {
        public override string Message
        {
            get { return "Maximum number of vehicles was reached."; }
        }
    }
}