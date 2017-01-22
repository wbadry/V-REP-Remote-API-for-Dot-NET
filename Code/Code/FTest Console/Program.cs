using CoppeliaRobotics;
using System;

namespace FTest
{
    internal static class Program
    {
        private static void Main()
        {
            Vrep.simxFinish(-1);
            var clientId = Vrep.simxStart("127.0.0.1", 19999, true, true, 5000, 5);
            if (clientId == -1)
            {
                Console.WriteLine("Make sure you started " +
                                "V-rep before connection and child script " +
                                "has \"simExtRemoteApiStart(5000);\" inside it");
                Vrep.simxFinish(-1);
            }
            else{
                Console.WriteLine("Connection is successful");
                var message = "Hello from C#";
                Vrep.SimAddStatusbarMessage(clientId, message);


                Console.ReadKey();}
        }
      }
    }
