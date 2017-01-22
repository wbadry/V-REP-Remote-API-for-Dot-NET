# V-REP Remote API for Dot NET
A repository for my attempt to port **[coppeliarobotics](http://www.coppeliarobotics.com/)** V-REP [remote API](http://www.coppeliarobotics.com/helpFiles/en/remoteApiOverview.htm) to .NET developers.

**Managed** functions were added to make it easier for developers to use streaming functions like **simxGetFloatSignal**.

I haven't ported all functions yet, but will spend weekly hours trying to add more.

**Functions List**
------------------

simxAddStatusbarMessage

SimAddStatusbarMessage **(Managed)**

SimxGetConnectionId

simxStart

simxFinish

simxGetObjectHandle

SimGetObjectHandle **(Managed)**

simxSetJointTargetPosition

SimSetJointTargetPosition **(Managed)**

simxGetJointPosition

SimGetJointPositionInit **(Managed)**

SimGetJointPositionEnd **(Managed)**

SimGetJointPositionRadian **(Managed)**

SimGetJointPositionDegrees **(Managed)**

simxSetJointTargetVelocity

SimSetJointTargetVelocity **(Managed)**

simxGetFloatSignal

SimGetFloatSignalInit **(Managed)**

SimGetFloatSignalEnd **(Managed)**

SimGetFloatSignal **(Managed)**

simxGetIntegerSignal

SimGetIntegerSignalInit **(Managed)**

SimGetIntegerSignalEnd **(Managed)**

SimGetIntegerSignal **(Managed)**

simxGetStringSignal

Library was created using **.NET framework 4.5**.
 
