using System;

namespace Garage_Linq.Commands
{
    public static class DefaultCommands
    {
        public static int exit()
        {
            Environment.Exit(0);
            return 0;
        }

        public static string Add()
        {

            return "";
        }

        public static string Do(int id, int data)
        {
            return $"I did something to the record Id {id} and save the data {data}";
        }

        public static string Mani(int id, int data)
        {
            return $"I did something to the record Id {id} and save the data {data}";
        }

        public static string DoSomethingOptional(int id, string data = "No Data Provided")
        {
            var result = $"I did something to the record Id {id} and save the data {data}";

            if (data == "No Data Provided")
            {
                result = $"I did something to the record Id {id} but the optinal parameter " +
                         $"was not provided, so I saved the value '{data}'";
            }
            return result;
        }
    }
}