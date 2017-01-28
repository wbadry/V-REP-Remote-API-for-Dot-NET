using System;using System.Linq;
using CoppeliaRobotics;

namespace Generate_Method_List
{
    class Program
    {
        private static void Main()
        {
     
                ShowMethods(typeof(Vrep));
            }

        private static void ShowMethods(Type type)
    {
                foreach (var method in type.GetMethods())
                {
                    var parameterDescriptions = string.Join
                        (", ", method.GetParameters()
                                     .Select(x => x.ParameterType + " " + x.Name)
                                     .ToArray());

                    Console.WriteLine("{0} {1} ({2})",
                                      method.ReturnType,
                                      method.Name,
                                      parameterDescriptions);
                }
            }
        }
    }
