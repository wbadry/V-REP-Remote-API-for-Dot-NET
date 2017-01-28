using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CoppeliaRobotics;
using DevExpress.Xpf.Core;

namespace PhantomXPincher
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow
    {
        private int _clientId;
        private readonly int[] _jointHandles = {0, 0, 0, 0};
        private readonly float[] _actualJointPosition = {0, 0, 0, 0};
        private readonly float[] _targetJointPosition = {0, 0, 0, 0};
        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectToVrep(object sender, RoutedEventArgs e)
        {
            //Disable connect button
            StartButton.IsEnabled = false;
            MoveJointsButton.IsEnabled = false;

            // Just in case, close all opened connections
            Vrep.simxFinish(-1);

            //Connect to v-rep running on local machine with server port 5000  
            _clientId = Vrep.simxStart("127.0.0.1", 19999, true, true, 5000, 5);

            //check if connection can fail safe (ClientId == -1) //Failed to connect
            if (_clientId == -1)
            {
                DXMessageBox.Show("Make sure you started " +
                                "V-rep before connection and child script " +
                                "has \"simExtRemoteApiStart(5000);\" inside it",
                    "Failed to connect to V-Rep!");
                Vrep.simxFinish(-1);
                StartButton.IsEnabled = true;
            }
            else
            {
                //Enable Disconnect button
                StopButton.IsEnabled = true;
                MoveJointsButton.IsEnabled = true;}
        }

        private void DisconnectFromVrep(object sender, RoutedEventArgs e)
        {

            Vrep.simxFinish(-1); //Close connection
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void SetJointPosition(object sender, RoutedEventArgs e)
        {
            MoveJointsButton.IsEnabled = false;
            ////Assign Target positions
            _targetJointPosition[0] = (float)(JointBase.Value * (Math.PI / 180.0));
            _targetJointPosition[1] = (float)(JointShoulder.Value * (Math.PI / 180.0));
            _targetJointPosition[2] = (float)(JointElbow.Value * (Math.PI / 180.0));
            _targetJointPosition[3] = (float)(JointWrist.Value * (Math.PI / 180.0));

            Task.Run(() =>
            {
                // Get the vrep handle of each position
                string baseName = "PhantomXPincher_joint";
                for (int i = 0; i <= 3; i++)
                {
                    _jointHandles[i] = Vrep.SimGetObjectHandle(_clientId, baseName+(i+1));
                }

                //Set the position of each joint
                for (int i = 0; i <= 3; i++)
                {
                    Vrep.SimSetJointTargetVelocity(_clientId, _jointHandles[i], (float)(10.0*(Math.PI/180.0)));
                    Vrep.SimSetJointTargetPosition(_clientId, _jointHandles[i], _targetJointPosition[i]);
                }

                //Read Joint Position (start streaming)
                for (int i = 0; i <= 3; i++)
                {
                    Vrep.SimGetJointPositionInit(_clientId, _jointHandles[i]);
                }


                while ((Math.Abs(_actualJointPosition[0] - _targetJointPosition[0]) > 0.1) ||
               (Math.Abs(_actualJointPosition[1] - _targetJointPosition[1]) > 0.1) ||
               (Math.Abs(_actualJointPosition[2] - _targetJointPosition[2]) > 0.1) ||
               (Math.Abs(_actualJointPosition[3] - _targetJointPosition[3]) > 0.1))
                {
                    // Get current angle of each joint
                    for (int i = 0; i <= 3; i++)
                    {
                        _actualJointPosition[i] = Vrep.SimGetJointPositionRadian(_clientId, _jointHandles[i]);
                    }

                    _dispatcher.Invoke(() =>
                    {
                        GBase.    Scales[0].Needles[0].Value = _actualJointPosition[0] * (180.0 / Math.PI);
                        GShoulder.Scales[0].Needles[0].Value = _actualJointPosition[1] * (180.0 / Math.PI);
                        GElbow.   Scales[0].Needles[0].Value = _actualJointPosition[2] * (180.0 / Math.PI);
                        GWrist.   Scales[0].Needles[0].Value = _actualJointPosition[3] * (180.0 / Math.PI);
                    });

                }

                //Stop joint position streaming
                for (int i = 0; i <= 3; i++)
                {
                    Vrep.SimGetJointPositionEnd(_clientId, _jointHandles[i]);
                }
               
                //Enable move button
                _dispatcher.Invoke(() =>
                {
                    MoveJointsButton.IsEnabled = true;
                });

            }
            );

        }

        private void BaseSetValue(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            GBase.Scales[0].Markers[0].Value = JointBase.Value;
        }

        private void ShoulderSetValue(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            GShoulder.Scales[0].Markers[0].Value = JointShoulder.Value;
        }

        private void ElbowSetValue(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            GElbow.Scales[0].Markers[0].Value = JointElbow.Value;
        }

        private void WristSetValue(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            GWrist.Scales[0].Markers[0].Value = JointWrist.Value;
        }
    }
}