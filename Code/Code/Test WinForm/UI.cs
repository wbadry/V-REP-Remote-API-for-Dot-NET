using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;
using remoteAPI;
using System.Windows.Threading;

namespace WindowsFormsApplication1
{
    public partial class UI : Form
    {
        public int ClientId;
        public int[] JointHandles = { 0, 0, 0, 0 };
        public float[] ActualJointPosition = { 0, 0, 0, 0 };
        public float[] TargetJointPosition = { 0, 0, 0, 0 };
        public Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;
        public UI()
        {
            InitializeComponent();
        }

        private void ConnectToVrEp(object sender, EventArgs e)
        {
            //Disable connect button
            ConnectButton.Enabled = false;
            MoveButton.Enabled = false;

            // Just in case, close all opened connections
            Vrep.simxFinish(-1);

            //Connect to v-rep running on local machine with server port 5000  
            ClientId = Vrep.simxStart("127.0.0.1", 19999, true, true, 5000, 5);

            //check if connection failsif (ClientId == -1) //Failed to connect
            if (ClientId == -1)
            {
                MessageBox.Show(Resources.Form1_ConnectToVrEp_Make_sure_you_started_V_rep_before_connection_and_child_script_has__simExtRemoteApiStart_5000____inside_it,
                    Resources.Form1_ConnectToVrEp_Failed_to_connect_to_V_Rep_);
                Vrep.simxFinish(-1);
                ConnectButton.Enabled = true;
            }
            else
            {
                //Enable Disconnect button
                DisconnectButton.Enabled = true;
                MoveButton.Enabled = true;
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Vrep.simxFinish(-1); //Close connection
            ConnectButton.Enabled = true;
            DisconnectButton.Enabled = false;
        }

        private void MoveJoint(object sender, EventArgs e)
        {
            MoveButton.Enabled = false;

            ////Assign Target positions
            TargetJointPosition[0] = (float)(BaseTrack.Value * (Math.PI / 180.0));
            //TargetJointPosition[1] = (float)(JointShoulder.Value * (Math.PI / 180.0));
            //TargetJointPosition[2] = (float)(JointElbow.Value * (Math.PI / 180.0));
            //TargetJointPosition[3] = (float)(JointWrist.Value * (Math.PI / 180.0));
            Task.Run(() =>
            {
                // Get the vrep handle of each position
                Vrep.simxGetObjectHandle(ClientId, "PhantomXPincher_joint1", out JointHandles[0],
                    SimxOpmode.oneshot_wait);
                Vrep.simxGetObjectHandle(ClientId, "PhantomXPincher_joint2", out JointHandles[1],
                    SimxOpmode.oneshot_wait);
                Vrep.simxGetObjectHandle(ClientId, "PhantomXPincher_joint3", out JointHandles[2],
                    SimxOpmode.oneshot_wait);
                Vrep.simxGetObjectHandle(ClientId, "PhantomXPincher_joint4", out JointHandles[3],
                    SimxOpmode.oneshot_wait);

               

                //Set the position of each joint
                Vrep.SimSetJointTargetPosition(ClientId, JointHandles[0], TargetJointPosition[0]);
                //Vrep.SimSetJointTargetPosition(ClientId, JointHandles[1], TargetJointPosition[1]);
                //Vrep.SimSetJointTargetPosition(ClientId, JointHandles[2], TargetJointPosition[2]);
                //Vrep.SimSetJointTargetPosition(ClientId, JointHandles[3], TargetJointPosition[3]);


                //Read Joint Position (start streaming)
                Vrep.SimGetJointPositionInit(ClientId, JointHandles[0]);
                //Vrep.SimGetJointPositionInit(ClientId, JointHandles[1]);
                //Vrep.SimGetJointPositionInit(ClientId, JointHandles[2]);
                //Vrep.SimGetJointPositionInit(ClientId, JointHandles[3]);

                while ((Math.Abs(ActualJointPosition[0] - TargetJointPosition[0]) > 0.1))
                {
                    // Get current angle of each joint
                    ActualJointPosition[0] = Vrep.SimGetJointPositionRadian(ClientId, JointHandles[0]);
                    //ActualJointPosition[1] = Vrep.SimGetJointPositionRadian(ClientId, JointHandles[1]);
                    //ActualJointPosition[2] = Vrep.SimGetJointPositionRadian(ClientId, JointHandles[2]);
                    //ActualJointPosition[3] = Vrep.SimGetJointPositionRadian(ClientId, JointHandles[3]);

                    Dispatcher.Invoke(
                   () =>
                    {
                        BaseLabel.Text = (ActualJointPosition[0]*(180.0/Math.PI)).ToString(CultureInfo.InvariantCulture);
                       
                    });
                }
                //Read Joint Position (start streaming)
                Vrep.SimGetJointPositionEnd(ClientId, JointHandles[0]);
                //Vrep.SimGetJointPositionEnd(ClientId, JointHandles[1]);
                //Vrep.SimGetJointPositionEnd(ClientId, JointHandles[2]);
                //Vrep.SimGetJointPositionEnd(ClientId, JointHandles[3]);
                Dispatcher.Invoke(
                () =>
                {
                    MoveButton.Enabled = true;

                });
               
            });

           
                //Update labels
              
                
                }
        }
    }
