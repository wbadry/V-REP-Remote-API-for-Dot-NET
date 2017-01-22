using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace remoteAPI
{
    /// <summary>
    /// The port class of C/C++ v-rep remote API
    /// </summary>
    public static class Vrep
    {

        /// <summary>
        /// Ends the communication thread. This should be the very last remote API function called on the client side. simxFinish should only be called after a successfull call to simxStart. This is a remote API helper function.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart. Can be -1 to end all running communication threads.</param>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void simxFinish(int clientId);
        
        /// <summary>
        /// Returns the ID of the current connection. Use this function to track the connection state to the server. See also simxStart. This is a remote API helper function.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int simxGetConnectionId(int clientId);

        /// <summary>
        /// Gets the value of a float signal. Signals are cleared at simulation start. See also simxSetFloatSignal, simxClearFloatSignal, simxGetIntegerSignal and simxGetStringSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="value">pointer to a location receiving the value of the signal</param>
        /// <param name="opmode">a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns> remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetFloatSignal(int clientId, string signalName, ref float value, SimxOpmode opmode);

        /// <summary>
        /// Gets the value of an integer signal. Signals are cleared at simulation start. See also simxSetIntegerSignal, simxClearIntegerSignal, simxGetFloatSignal and simxGetStringSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="value">pointer to a location receiving the value of the signal</param>
        /// <param name="opmode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetIntegerSignal(int clientId, string signalName, ref int value, SimxOpmode opmode);

        /// <summary>
        /// More managed way to Gets the value of an integer signal. Signals are cleared at simulation start. See also simxSetIntegerSignal, simxClearIntegerSignal, simxGetFloatSignal and simxGetStringSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>integer signal value</returns>
        public static int SimwGetIntegerSignal(int clientId, string signalName)
        {
            int v = -1;
            simxGetIntegerSignal(clientId, signalName, ref v, SimxOpmode.streaming);
            Thread.Sleep(150);
            simxGetIntegerSignal(clientId, signalName, ref v, SimxOpmode.buffer);
            return v;
        }

        /// <summary>
        /// Sets the value of a string signal. If that signal is not yet present, it is added. See also simxWriteStringStream, simxGetStringSignal, simxClearStringSignal, simxSetIntegerSignal and simxSetFloatSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="value">value of the signal (which may contain any value, including embedded zeros)</param>
        /// <param name="length">size of the signalValue string.</param>
        /// <param name="opmode">a remote API function operation mode. Recommended operation mode for this function is simx_opmode_oneshot</param>
        /// <returns>a remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxSetStringSignal(int clientId, string signalName, string value, int length, SimxOpmode opmode);

        /// <summary>
        /// Gets the value of a string signal. Signals are cleared at simulation start. See also simxSetStringSignal, simxReadStringStream, simxClearStringSignal, simxGetIntegerSignal and simxGetFloatSignal.
        /// </summary>
        /// <param name="clientId"> the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <param name="pointerToValue"> pointer to a pointer receiving the value of the signal. The signal value will remain valid until next remote API call</param>
        /// <param name="signalLength">pointer to a location receiving the value of the signal length, since it may contain any data (also embedded zeros).</param>
        /// <param name="opmode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetStringSignal(int clientId, string signalName, ref IntPtr pointerToValue, ref int signalLength, SimxOpmode opmode);

        /// <summary>
        /// A managed function to get the value of a string signal. Signals are cleared at simulation start. See also simxSetStringSignal, simxReadStringStream, simxClearStringSignal, simxGetIntegerSignal and simxGetFloatSignal.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="signalName">name of the signal</param>
        /// <returns>the string value of signalName</returns>
        public static string SimGetStringSignal(int clientId, string signalName)
        {
            IntPtr p = IntPtr.Zero;
            var l1 = 0;
            var l2 = 0;
            simx_error e;
            Thread.Sleep(150);
            e = simxGetStringSignal(clientId, signalName, ref p, ref l2, SimxOpmode.buffer);
            Console.WriteLine("Signal {0} -> {1}/{2}", signalName, l1, l2);
            if (e == simx_error.simx_return_ok && p != IntPtr.Zero)
            {
                var s = Marshal.PtrToStringAnsi(p, l2);
                Marshal.Release(p);
                return s;
            }
            return "";
        }

        /// <summary>
        /// DEPRECATED. Refer to simxReadStringStream instead.
        /// </summary>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetAndClearStringSignal(int clientId, string signalName, ref IntPtr pointerToValue, ref int signalLength, SimxOpmode opmode);

        /// <summary>
        /// Retrieves the intrinsic position of a joint. This function cannot be used with spherical joints (use simxGetJointMatrix instead). See also simxSetJointPosition and simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="targetPosition"> intrinsic position of the joint. This is a one-dimensional value: if the joint is revolute, the rotation angle is returned, if the joint is prismatic, the translation amount is returned, etc.</param>
        /// <param name="opmode">a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern simx_error simxGetJointPosition(int clientId, int jointHandle, ref float targetPosition, SimxOpmode opmode);

        /// <summary>
        /// This method is called to start streaming of joint position
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        public static void SimGetJointPositionInit(int clientId, int jointHandle)
        {
            float pos=0;
            simxGetJointPosition(clientId,jointHandle, ref pos, SimxOpmode.streaming);
        }

        /// <summary>
        /// This method is called to end streaming of joint position
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        public static void SimGetJointPositionEnd(int clientId, int jointHandle)
        {
            float pos = 0;
            simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.discontinue);
        }


        /// <summary>
        ///  Retrieves the intrinsic position of a joint. This function cannot be used with spherical joints (use simxGetJointMatrix instead). See also simxSetJointPosition and simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <returns>joint position in radian</returns>
        public static float SimGetJointPositionRadian(int clientId, int jointHandle)
        {
           float pos = 0;
           simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.buffer);
           return pos;
        }

        /// <summary>
        ///  Retrieves the intrinsic position of a joint. This function cannot be used with spherical joints (use simxGetJointMatrix instead). See also simxSetJointPosition and simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <returns>joint position in degrees</returns>
        public static float SimGetJointPositionDegrees(int clientId, int jointHandle)
        {
            float pos = 0;
            simxGetJointPosition(clientId, jointHandle, ref pos, SimxOpmode.buffer);
            return pos*(float)(180.0/Math.PI);
        }

        /// <summary>
        /// Retrieves an integer parameter of a object. See also simxSetObjectIntParameter and simxGetObjectFloatParameter.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="objectHandle">handle of the object</param>
        /// <param name="parameterId"> identifier of the parameter to retrieve. See the list of all possible object parameter identifiers</param>
        /// <param name="parameterValue"> pointer to a location that will receive the value of the parameter</param>
        /// <param name="opmode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls), or simx_opmode_blocking (depending on the intended usage)</param>
        /// <returns>a remote API function return code</returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetObjectIntParameter(int clientId, int objectHandle, int parameterId, ref int parameterValue, SimxOpmode opmode);

        /// <summary>
        /// Retrieves a floating-point parameter of a object. See also simxSetObjectFloatParameter and simxGetObjectIntParameter.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="objectHandle"> handle of the object</param>
        /// <param name="parameterId">identifier of the parameter to retrieve. See the list of all possible object parameter identifiers</param>
        /// <param name="parameterValue">pointer to a location that will receive the value of the parameter</param>
        /// <param name="opmode">a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls), or simx_opmode_blocking (depending on the intended usage)</param>
        /// <returns>a remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetObjectFloatParameter(int clientId, int objectHandle, int parameterId, ref float parameterValue, SimxOpmode opmode);

        /// <summary>
        /// Retrieves the orientation (Euler angles) of an object. See also simxSetObjectOrientation, simxGetObjectPosition and simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the object</param>
        /// <param name="relativeToHandle">indicates relative to which reference frame we want the orientation. Specify -1 to retrieve the absolute orientation, sim_handle_parent to retrieve the orientation relative to the object's parent, or an object handle relative to whose reference frame you want the orientation</param>
        /// <param name="orientations">pointer to 3 values receiving the Euler angles (alpha, beta and gamma)</param>
        /// <param name="opmode"> a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns> remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetObjectOrientation(int clientId, int jointHandle, int relativeToHandle, float[] orientations, SimxOpmode opmode);

        /// <summary>
        /// Retrieves the position of an object. See also simxSetObjectPosition, simxGetObjectOrientation and simxGetObjectGroupData.
        /// </summary>
        /// <param name="clientId">the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the object</param>
        /// <param name="relativeToHandle">indicates relative to which reference frame we want the position. Specify -1 to retrieve the absolute position, sim_handle_parent to retrieve the position relative to the object's parent, or an object handle relative to whose reference frame you want the position</param>
        /// <param name="positions">pointer to 3 values receiving the position</param>
        /// <param name="opmode">a remote API function operation mode. Recommended operation modes for this function are simx_opmode_streaming (the first call) and simx_opmode_buffer (the following calls)</param>
        /// <returns>a remote API function return code</returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetObjectPosition(int clientId, int jointHandle, int relativeToHandle, float[] positions, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliendId"></param>
        /// <param name="pause"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxPauseCommunication(int cliendId, int pause);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sensorHandle"></param>
        /// <param name="detectionState"></param>
        /// <param name="detectionPoint"></param>
        /// <param name="objectHandle"></param>
        /// <param name="normalVector"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static simx_error simxReadProximitySensor(int clientId, int sensorHandle,
                                                         ref char detectionState, float[] detectionPoint, ref int objectHandle, float[] normalVector, SimxOpmode opmode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="jointHandle"></param>
        /// <param name="targetPosition"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxSetJointTargetPosition(int clientId, int jointHandle, float targetPosition, SimxOpmode opmode);

        /// <summary>
        /// Sets the target position of a joint if the joint is in torque/force mode (also make sure that the joint's motor and position control are enabled). See also simxSetJointPosition.
        /// </summary>
        /// <param name="clientId"> the client ID. refer to simxStart.</param>
        /// <param name="jointHandle">handle of the joint</param>
        /// <param name="targetPosition"> target position of the joint (angular (radian) or linear value depending on the joint type)</param>
        /// <returns></returns>
        public static simx_error SimSetJointTargetPosition(int clientId, int jointHandle, float targetPosition)
        {
            //Call function with non-blocking fashion
           simx_error x= simxSetJointTargetPosition(clientId,jointHandle, targetPosition, SimxOpmode.oneshot);
           return x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="jointHandle"></param>
        /// <param name="velocity"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern simx_error simxSetJointTargetVelocity(int clientId, int jointHandle, float velocity, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="objectHandle"></param>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxSetObjectFloatParameter(int clientId, int objectHandle, int parameterId, float parameterValue, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="objectHandle"></param>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxSetObjectIntParameter(int clientId, int objectHandle, int parameterId, int parameterValue, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="waitForConnection"></param>
        /// <param name="reconnectOnDisconnect"></param>
        /// <param name="timeoutMs"></param>
        /// <param name="cycleTimeMs"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int simxStart(string ip, int port, bool waitForConnection, bool reconnectOnDisconnect, int timeoutMs, int cycleTimeMs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="uiHandle"></param>
        /// <param name="uiEventButtonId"></param>
        /// <param name="aux"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetUIEventButton(int clientId, int uiHandle, ref int uiEventButtonId, IntPtr aux, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="uiName"></param>
        /// <param name="p"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        // public static extern simx_error simxGetUIHandle(int clientID, string uiName, out int handle, simx_opmode opmode);
        public static extern simx_error simxGetUIHandle(int clientId, string uiName, IntPtr p, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="objectName"></param>
        /// <param name="handle"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxGetObjectHandle(int clientId, string objectName, out int handle, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxStartSimulation(int clientId, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
       [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern simx_error simxStopSimulation(int clientId, SimxOpmode opmode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sensorHandle"></param>
        /// <param name="resolution"></param>
        /// <param name="image"></param>
        /// <param name="option"></param>
        /// <param name="opmode"></param>
        /// <returns></returns>
        [DllImport("remoteApi.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static simx_error simxGetVisionSensorImage(int clientId, int sensorHandle, out int resolution, out IntPtr image, char option, SimxOpmode opmode);
    }
}