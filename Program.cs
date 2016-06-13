using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Garage_Linq.Controllers;
using Garage_Linq.DataStorage;
using Garage_Linq.Models;

namespace Garage_Linq
{
    internal class Program
    {
        public const int MaxNumberOfVehicles = 10;
        public static GarageController<Vehicle> GarageController;

        private const string CommandNameSpace = "Garage_Linq.Commands";
        private const string ReadPrompt = "console> ";
        private static Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> _commandLibraries;

        private static void Main(string[] args)
        {
            Console.Title = "GarageController LINQ";
            _commandLibraries = new Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>>();

            var commandClasses =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsClass && t.Namespace == CommandNameSpace)
                    .ToList();

            foreach (var commandClass in commandClasses)
            {
                var methods = commandClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var methodDictionary = new Dictionary<string, IEnumerable<ParameterInfo>>();
                foreach (var method in methods)
                {
                    var commandName = method.Name;
                    methodDictionary.Add(commandName, method.GetParameters());
                }
                _commandLibraries.Add(commandClass.Name, methodDictionary);
            }

            SetUpGarage();

            Run();

            Console.ReadLine();
        }

        private static void SetUpGarage()
        {
            GarageController = new GarageController<Vehicle>(20)
            {
                new Car("abc123", "röd", "david", true),
                new Buss("abc123", "blå", "tobias", true),
                new Airplane("abc123", "röd", "tobias", 2, 10),
                new Boat("abc123", "röd", "tobias", 0),
                new MotorCycle("abc123", "röd", "david", false)
            };

            /*foreach (var typesOfVehicle in GarageController.GetDictionaryTypesOfVehicles())
            {
                Console.WriteLine(typesOfVehicle.Key + ": " + typesOfVehicle.Value);
            }*/

           
        }

        private static void Run()
        {
            while (true)
            {
                var consoleInput = ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;

                try
                {
                    var cmd = new ConsoleCommand(consoleInput);
                    var result = Execute(cmd);
                    WriteToConsole(result);
                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.Message);
                }
            }
        }

        private static string Execute(ConsoleCommand command)
        {
            if (!_commandLibraries.ContainsKey(command.LibraryClassName))
            {
                return "No such command-library found";
            }
            var methodDictionary = _commandLibraries[command.LibraryClassName];
            if (!methodDictionary.ContainsKey(command.Name))
            {
                return "No such command-name found";
            }

            var methodParameterValueList = new List<object>();
            IEnumerable<ParameterInfo> parameterInfoList = methodDictionary[command.Name].ToList();
            var requiredParams = parameterInfoList.Where(p => p.IsOptional == false);
            var optionalParams = parameterInfoList.Where(p => p.IsOptional);
            var requiredCount = requiredParams.Count();
            var optionalCount = optionalParams.Count();
            var providedCount = command.Arguments.Count();

            if (requiredCount > providedCount)
            {
                if (requiredCount > providedCount)
                {
                    return
                        $"Missing required argument. {requiredCount} required, {optionalCount} optional, {providedCount} provided";
                }
            }

            if (parameterInfoList.Count() > 0)
            {
                foreach (var parameterInfo in parameterInfoList)
                {
                    methodParameterValueList.Add(parameterInfo.DefaultValue);
                }
            }

            for (var i = 0; i < command.Arguments.Count(); i++)
            {
                var methodParam = parameterInfoList.ElementAt(i);
                var typeRequired = methodParam.ParameterType;
                try
                {
                    var value = CoerceArgument(typeRequired, command.Arguments.ElementAt(i));
                    methodParameterValueList.RemoveAt(i);
                    methodParameterValueList.Insert(i, value);
                }
                catch (ArgumentException)
                {
                    var argumentName = methodParam.Name;
                    var argumentTypeName = typeRequired.Name;
                    string message =
                        $"The value passed for argument '{argumentName}' cannot be parsed to type '{argumentTypeName}'";
                    throw new ArgumentException(message);
                }
            }

            var currentAssembly = typeof(Program).Assembly;

            var commmandLibraryClass = currentAssembly.GetType(CommandNameSpace + "." + command.LibraryClassName);
            object[] inputArguments = null;
            if (methodParameterValueList.Count > 0)
            {
                inputArguments = methodParameterValueList.ToArray();
            }

            var typeInfo = commmandLibraryClass;

            try
            {
                var result = typeInfo.InvokeMember(command.Name,
                    BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null,
                    inputArguments);
                return result.ToString();
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }


        private static object CoerceArgument(Type requiredType, string inputValue)
        {
            var requiredTypeCode = Type.GetTypeCode(requiredType);
            var exceptionMessage =
                $"Cannnot coerce the input argument {inputValue} to required type {requiredType.Name}";

            object result = null;
            switch (requiredTypeCode)
            {
                case TypeCode.String:
                    result = inputValue;
                    break;
                case TypeCode.Int16:
                    short number16;
                    if (short.TryParse(inputValue, out number16))
                    {
                        result = number16;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Int32:
                    int number32;
                    if (int.TryParse(inputValue, out number32))
                    {
                        result = number32;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Int64:
                    long number64;
                    if (long.TryParse(inputValue, out number64))
                    {
                        result = number64;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Boolean:
                    bool trueFalse;
                    if (bool.TryParse(inputValue, out trueFalse))
                    {
                        result = trueFalse;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Byte:
                    byte byteValue;
                    if (byte.TryParse(inputValue, out byteValue))
                    {
                        result = byteValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Char:
                    char charValue;
                    if (char.TryParse(inputValue, out charValue))
                    {
                        result = charValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.DateTime:
                    DateTime dateValue;
                    if (DateTime.TryParse(inputValue, out dateValue))
                    {
                        result = dateValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Decimal:
                    decimal decimalValue;
                    if (decimal.TryParse(inputValue, out decimalValue))
                    {
                        result = decimalValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Double:
                    double doubleValue;
                    if (double.TryParse(inputValue, out doubleValue))
                    {
                        result = doubleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Single:
                    float singleValue;
                    if (float.TryParse(inputValue, out singleValue))
                    {
                        result = singleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.UInt16:
                    ushort uInt16Value;
                    if (ushort.TryParse(inputValue, out uInt16Value))
                    {
                        result = uInt16Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.UInt32:
                    uint uInt32Value;
                    if (uint.TryParse(inputValue, out uInt32Value))
                    {
                        result = uInt32Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.UInt64:
                    ulong uInt64Value;
                    if (ulong.TryParse(inputValue, out uInt64Value))
                    {
                        result = uInt64Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.SByte:
                    break;
                default:
                    throw new ArgumentException(exceptionMessage);
            }
            return result;
        }

        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }

        public static string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write(ReadPrompt + promptMessage);
            return Console.ReadLine();
        }
    }

    internal class ConsoleFormatting
    {
        public static string Indent(int i)
        {
            var indent = "";
            for (var j = 0; j < i; j++)
            {
                indent += " ";
            }
            return indent;
        }
    }
}