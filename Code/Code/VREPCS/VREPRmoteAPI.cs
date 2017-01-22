using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace CoppeliaRobotics
{
    /// <summary>
    /// This is unofficial C# wrapper class for V-REP client remote API.
    /// The GitHub repository will be update with more functions soon. 
    /// </summary>
    /// <remarks>
    /// Author : Waleed El-Badry
    /// E-Mail : waleed.elbadry@must.edu.eg
    /// </remarks>
    public static class Vrep
    {

        /// <summary>
        /// Adds a message to the status bar.
        /// </summary>
        /// <param name="clientId"> the client ID. refer to simxStart.</param>
        /// <param name="message">the message to display</param>
        /// <param name="operationMode"> a remote API function operation mode. Recommended operation mode for this function is simx_opmode_oneshot</param>
        /// <returns></returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void simxAddStatusbarMessage(int clientId,string message,SimxOpmode operationMode);

        /// <summary>
        /// (Managed) Adds a message to the status bar.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="message">the message to display</param>
        public static void SimAddStatusbarMessage(int clientId, [NotNull] string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            simxAddStatusbarMessage(clientId,  message, SimxOpmode.Oneshot);
        }

        /// <summary>
        /// Returns the ID of the current connection. Use this function to track the connection state to the server. See also simxStart. This is a remote API helper function.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <returns></returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SimxGetConnectionId(int clientId);

        /// <summary>
        /// Starts a communication thread with the server (i.e. V-REP). A same client may start several communication threads (but only one communication thread for a given IP and port). This should be the very first remote API function called on the client side. Make sure to start an appropriate remote API server service on the server side, that will wait for a connection. See also simxFinish. This is a remote API helper function.
        /// </summary>
        /// <param name="connectionAddress"> the IP address where the server is located (i.e. V-REP)</param>
        /// <param name="connectionPort">the port number where to connect</param>
        /// <param name="waitUntilConnected"> if different from zero, then the function blocks until connected (or timed out).</param>
        /// <param name="doNotReconnectOnceDisconnected">if different from zero, then the communication thread will not attempt a second connection if a connection was lost.</param>
        /// <param name="timeOutInMs">
        /// if positive: the connection time-out in milliseconds for the first connection attempt. In that case, the time-out for blocking function calls is 5000 milliseconds.
        /// if negative: its positive value is the time-out for blocking function calls.In that case, the connection time-out for the first connection attempt is 5000 milliseconds.
        /// </param>
        /// <param name="commThreadCycleInMs"> indicates how often data packets are sent back and forth. Reducing this number improves responsiveness, and a default value of 5 is recommended.</param>
        /// <returns>the client ID, or -1 if the connection to the server was not possible (i.e. a timeout was reached). A call to simxStart should always be followed at the end with a call to <c>simxFinish</c> if <c>simxStart</c> didn't return -1</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int simxStart(string connectionAddress, int connectionPort, bool waitUntilConnected, bool doNotReconnectOnceDisconnected, int timeOutInMs, int commThreadCycleInMs);

        /// <summary>
        /// Ends the communication thread. This should be the very last remote API function called on the client side. simxFinish should only be called after a successful call to simxStart. This is a remote API helper function.
        /// </summary>
        /// <param name="clientId">the client ID. refer to <c>Vrep.simxStart</c>. Can be -1 to end all running communication threads.</param>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void simxFinish(int clientId);

        /// <summary>
        /// Retrieves an object handle based on its name. If the client application is launched from a child script, then you could also let the child script figure out what handle correspond to what objects, and send the handles as additional arguments to the client application during its launch. See also simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="objectName">name of the object. If possible, don't rely on the automatic name adjustment mechanism, and always specify the full object name, including the #: if the object is "myJoint", specify "myJoint#", if the object is "myJoint#0", specify "myJoint#0", etc. </param>
        /// <param name="handle">pointer to a value that will receive the handle</param>
        /// <param name="operationMode">a remote API function operation mode. Recommended operation mode for this function is simx_opmode_blocking</param>
        /// <returns></returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern SimxError simxGetObjectHandle(int clientId, string objectName, out int handle, SimxOpmode operationMode);

        /// <summary>
        /// (Managed) Retrieves an object handle based on its name. If the client application is launched from a child script, then you could also let the child script figure out what handle correspond to what objects, and send the handles as additional arguments to the client application during its launch. See also simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="objectName">name of the object. If possible, don't rely on the automatic name adjustment mechanism, and always specify the full object name, including the #: if the object is "myJoint", specify "myJoint#", if the object is "myJoint#0", specify "myJoint#0", etc. </param>
        /// <returns>a value that will receive the handle</returns>
        public static int SimGetObjectHandle(int clientId, string objectName)
        {
            int handle;
            simxGetObjectHandle(clientId, objectName, out handle, SimxOpmode.OneshotWait);
            return handle;
        }

        /// <summary>
        /// Sets the target position of a joint if the joint is in torque/force mode (also make sure that the joint's motor and position control are enabled). See also simxSetJointPosition.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="targetPosition">target position of the joint (angular or linear value depending on the joint type)</param>
        /// <param name="operationMode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_oneshot or simx_opmode_streaming</param>
        /// <returns></returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern SimxError simxSetJointTargetPosition(int clientId, int jointHandle, float targetPosition, SimxOpmode operationMode);

        /// <summary>
        /// (Managed) Sets the target position of a joint if the joint is in torque/force mode (also make sure that the joint's motor and position control are enabled). See also simxSetJointPosition.
        /// </summary>
        /// <param name="clientId"> the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="targetPosition"> target position of the joint (angular (radian) or linear value depending on the joint type)</param>
        /// <returns></returns>
        public static SimxError SimSetJointTargetPosition(int clientId, int jointHandle, float targetPosition)
        {
            //Call function with non-blocking fashion
            SimxError x = simxSetJointTargetPosition(clientId, jointHandle, targetPosition, SimxOpmode.Oneshot);
            return x;
        }

        /// <summary>
        /// Retrieves the intrinsic position of a joint. This function cannot be used with spherical joints (use simxGetJointMatrix instead). See also simxSetJointPosition and simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="targetPosition"> intrinsic position of the joint. This is a one-dimensional value: if the joint is revolute, the rotation angle is returned, if the joint is prismatic, the translation amount is returned, etc.</param>
        /// <param name="operationMode">a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern SimxError simxGetJointPosition(int clientId, int jointHandle, ref float targetPosition, SimxOpmode operationMode);

        /// <summary>
        /// (Managed) This method is called to start streaming of joint position (called once)
        /// </summary>
        /// <param name="clientId">the client ID. refer to <c>simxStart</c>.</param>
        /// <param name="jointHandle">handle of the joint</param>
        public static void SimGetJointPositionInit(int clientId, int jointHandle)
        {
            float pos = 0;
            simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.Streaming);
        }

        /// <summary>
        /// (Managed) This method is called to end streaming of joint position (called once)
        /// </summary>
        /// <param name="clientId">the client ID. refer to <c>simxStart</c>.</param>
        /// <param name="jointHandle">handle of the joint</param>
        public static void SimGetJointPositionEnd(int clientId, int jointHandle)
        {
            float pos = 0;
            simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.Discontinue);
        }

        /// <summary>
        ///  (Managed) Retrieves the intrinsic position of a joint in Radian. 
        /// </summary>
        /// <param name="clientId">the client ID. refer to <c>simxStart</c>.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <returns>joint position in radian</returns>
        public static float SimGetJointPositionRadian(int clientId, int jointHandle)
        {
            float pos = 0;
            simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.Buffer);
            return pos;
        }

        /// <summary>
        ///  (Managed) Retrieves the intrinsic position of a joint.
        /// </summary>
        /// <param name="clientId">the client ID. refer to <c>simxStart</c>.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <returns>joint position in degrees</returns>
        public static float SimGetJointPositionDegrees(int clientId, int jointHandle)
        {
            float pos = 0;
            simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.Buffer);
            return (float)Math.Round(pos * (180.0 / Math.PI),2) ;
        }

        /// <summary>
        /// Sets the intrinsic target velocity of a non-spherical joint. This command makes only sense when the joint mode is in torque/force mode: the dynamics functionality and the joint motor have to be enabled (position control should however be disabled)
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="velocity">target velocity of the joint (linear or angular velocity depending on the joint-type)</param>
        /// <param name="operationMode"> remote API function operation mode. Recommended operation modes for this function are simx_opmode_oneshot or simx_opmode_streaming</param>
        /// <returns>a remote API function return code</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern SimxError simxSetJointTargetVelocity(int clientId, int jointHandle, float velocity, SimxOpmode operationMode);

        /// <summary>
        /// Sets the intrinsic target velocity of a non-spherical joint. This command makes only sense when the joint mode is in torque/force mode: the dynamics functionality and the joint motor have to be enabled (position control should however be disabled)
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="velocity">target velocity of the joint (linear or angular velocity depending on the joint-type)</param>
        /// <returns></returns>
        public static void SimSetJointTargetVelocity(int clientId, int jointHandle, float velocity)
        {
            simxSetJointTargetVelocity(clientId,jointHandle,velocity, SimxOpmode.Oneshot);
        }

        /// <summary>
        /// Gets the value of a float signal. Signals are cleared at simulation start. See also simxSetFloatSignal, simxClearFloatSignal, simxGetIntegerSignal and simxGetStringSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="signalValue">pointer to a location receiving the value of the signal</param>
        /// <param name="operationMode">a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns> remote API function return code</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern SimxError simxGetFloatSignal(int clientId, string signalName, ref float signalValue, SimxOpmode operationMode);

        /// <summary>
        /// (Managed) Start streaming of float signal value (called once)
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>the value of the signal</returns>
        public static float SimGetFloatSignalInit(int clientId, string signalName)
        {
            float val = 0;
            simxGetFloatSignal(clientId, signalName, ref val, SimxOpmode.Streaming);
            return val;
        }

        /// <summary>
        /// (Managed) Start streaming of float signal value (called once)
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>the value of the signal</returns>
        public static SimxError SimGetFloatSignalEnd(int clientId, string signalName)
        {
            float val = 0;
            var e = simxGetFloatSignal(clientId, signalName, ref val, SimxOpmode.Discontinue);
            return e;
        }

        /// <summary>
        /// (Managed) Streaming of float signal value. This should be called after SimGetFloatSignalInit
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>the value of the signal</returns>
        public static SimxError SimGetFloatSignal(int clientId, string signalName)
        {
            float val = 0;
            var e = simxGetFloatSignal(clientId, signalName, ref val, SimxOpmode.Streaming);
            return e;
        }

        /// <summary>
        /// Gets the value of an integer signal. Signals are cleared at simulation start. See also simxSetIntegerSignal, simxClearIntegerSignal, simxGetFloatSignal and simxGetStringSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="signalValue">pointer to a location receiving the value of the signal</param>
        /// <param name="operationMode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern SimxError simxGetIntegerSignal(int clientId, string signalName, ref int signalValue, SimxOpmode operationMode);

        /// <summary>
        /// (Managed) Start streaming of integer signal value (called once)
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>Error code</returns>
        public static SimxError SimGetIntegerSignalInit(int clientId, string signalName)
        {
            int val = 0;
            var e = simxGetIntegerSignal(clientId, signalName, ref val, SimxOpmode.Streaming);
            return e;
        }

        /// <summary>
        /// (Managed) Start streaming of integer signal value (called once)
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>Error code</returns>
        public static SimxError SimGetIntegerSignalEnd(int clientId, string signalName)
        {
            int val = 0;
            var e = simxGetIntegerSignal(clientId, signalName, ref val, SimxOpmode.Discontinue);
            return e;
        }

        /// <summary>
        /// (Managed) Streaming of integer signal value. This should be called after SimGetIntegerSignalInit
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>the value of the signal</returns>
        public static int SimGetIntegerSignal(int clientId, string signalName)
        {
            int val = 0;
            simxGetIntegerSignal(clientId, signalName, ref val, SimxOpmode.Streaming);
            return val;
        }

        /// <summary>
        /// Gets the value of a string signal. Signals are cleared at simulation start. See also simxSetStringSignal, simxReadStringStream, simxClearStringSignal, simxGetIntegerSignal and simxGetFloatSignal.
        /// </summary>
        /// <param name="clientId"> the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="signalValue"> pointer to a pointer receiving the value of the signal. The signal value will remain valid until next remote API call</param>
        /// <param name="signalLength">pointer to a location receiving the value of the signal length, since it may contain any data (also embedded zeros).</param>
        /// <param name="operationMode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SimxError simxGetStringSignal(int clientId, string signalName, ref IntPtr signalValue, ref int signalLength, SimxOpmode operationMode);

       



        public enum SimxError
        {
            SimxReturnOk = 0,
            SimxReturnNovalueFlag = 1,
            SimxReturnTimeoutFlag = 2,
            SimxReturnIllegalOpmodeFlag = 4,
            SimxReturnRemoteErrorFlag = 8,
            SimxReturnSplitProgressFlag = 16,
            SimxReturnLocalErrorFlag = 32,
            SimxReturnInitializeErrorFlag = 64,
        }

        public enum SimxOpmode
        {
            Oneshot = 0,
            OneshotWait = 65536,
            Streaming = 131072,
            Continuous = 131072,
            OneshotSplit = 196608,
            ContinuousSplit = 262144,
            StreamingSplit = 262144,
            Discontinue = 327680,
            Buffer = 393216,
            Remove = 458752,
        }
    }
}
