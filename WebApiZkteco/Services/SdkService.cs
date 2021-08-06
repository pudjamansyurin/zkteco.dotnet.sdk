using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiZkteco.Models;

namespace WebApiZkteco.Services
{
    public interface ISdkService
    {
        bool Connect();
        void Disconnect();
        int GetDeviceInfo(ref Device info);
        void GetUser(string sUserID, ref User user);
        void GetUsers(ref List<User> users);
        void SetUser(User user);
        void DeleteUser(User user);
    }

    public class SdkService : ISdkService
    {

        private string Ip;
        private Int32 Port;
        private bool bIsConnected;//the boolean value identifies whether the device is connected
        private int iMachineNumber;//the serial number of the device.After connecting the device ,this value will be changed.
        private zkemkeeper.CZKEMClass axCZKEM1;

        public SdkService(string ip, Int32 port)
        {
            Ip = ip;
            Port = port;
            bIsConnected = false;
            iMachineNumber = 1;
            axCZKEM1 = new zkemkeeper.CZKEMClass(); //Create Standalone SDK class dynamicly.

            // Connect to device
            try
            {
                Connect();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Connect()
        {
            int idwErrorCode = 0;

            axCZKEM1.PullMode = 1;
            SetConnectState(axCZKEM1.Connect_Net(Ip, Port));
            if (GetConnectState() == true)
            {
                iMachineNumber = 1; //In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                throw new Exception("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString());
            }
            return GetConnectState();
        }

        public void Disconnect()
        {
            axCZKEM1.Disconnect();
            SetConnectState(false);
        }

        private bool GetConnectState()
        {
            return bIsConnected;
        }

        private void SetConnectState(bool state)
        {
            bIsConnected = state;
        }

        public int GetDeviceInfo(ref Device info)
        {
            int iRet = 0;

            string sFirmver = "";
            string sMac = "";
            string sPlatform = "";
            string sSN = "";
            string sProductTime = "";
            string sDeviceName = "";
            //int iFPAlg = 0;
            //int iFaceAlg = 0;
            string sProducter = "";
            string strTemp = "";

            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device

            axCZKEM1.GetSysOption(iMachineNumber, "~ZKFPVersion", out strTemp);
            info.iFPAlg = Convert.ToInt32(strTemp);

            axCZKEM1.GetSysOption(iMachineNumber, "ZKFaceVersion", out strTemp);
            info.iFaceAlg = Convert.ToInt32(strTemp);

            /*
            axCZKEM1.GetDeviceInfo(iMachineNumber, 72, ref iFPAlg);
            axCZKEM1.GetDeviceInfo(iMachineNumber, 73, ref iFaceAlg);
            */

            axCZKEM1.GetVendor(ref sProducter);
            info.sProducter = sProducter;

            axCZKEM1.GetProductCode(iMachineNumber, out sDeviceName);
            info.sDeviceName = sDeviceName;

            axCZKEM1.GetDeviceMAC(iMachineNumber, ref sMac);
            info.sMac = sMac;

            axCZKEM1.GetFirmwareVersion(iMachineNumber, ref sFirmver);
            info.sFirmver = sFirmver;

            /*
            if (sta_GetDeviceType() == 1)
            {
                axCZKEM1.GetDeviceFirmwareVersion(iMachineNumber, ref sFirmver);
            }
             */
            //lblOutputInfo.Items.Add("[func GetDeviceFirmwareVersion]Temporarily unsupported");

            axCZKEM1.GetPlatform(iMachineNumber, ref sPlatform);
            info.sPlatform = sPlatform;

            axCZKEM1.GetSerialNumber(iMachineNumber, out sSN);
            info.sSN = sSN;

            axCZKEM1.GetDeviceStrInfo(iMachineNumber, 1, out sProductTime);
            info.sProductTime = sProductTime;

            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

            iRet = 1;
            return iRet;
        }

        public void GetUser(string sUserID, ref User user)
        {
            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            if (sUserID == null)
            {
                throw new Exception("Invalid user ID");
            }

            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            int idwErrorCode = 0;

            axCZKEM1.EnableDevice(iMachineNumber, false);
            if (axCZKEM1.SSR_GetUserInfo(iMachineNumber, sUserID, out sName, out sPassword, out iPrivilege, out bEnabled)) //get user' information from the memory
            {
                user.sUserID = sUserID;
                user.sName = sName;
                user.sPassword = sPassword;
                user.iPrivilege = iPrivilege;
                user.bEnabled = bEnabled;

                GetFinger(ref user);
                GetFace(ref user);
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                axCZKEM1.EnableDevice(iMachineNumber, true);
                throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
            }
            axCZKEM1.EnableDevice(iMachineNumber, true);
        }

        //Download user's 9.0 or 10.0 arithmetic fingerprint templates(in strings)
        //Only TFT screen devices with firmware version Ver 6.60 version later support function "GetUserTmpExStr" and "GetUserTmpEx".
        //'While you are using 9.0 fingerprint arithmetic and your device's firmware version is under ver6.60,you should use the functions "SSR_GetUserTmp" or 
        //"SSR_GetUserTmpStr" instead of "GetUserTmpExStr" or "GetUserTmpEx" in order to download the fingerprint templates.
        public void GetUsers(ref List<User> users)
        {
            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            string sUserID = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            axCZKEM1.EnableDevice(iMachineNumber, false);
            axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
            axCZKEM1.ReadAllTemplate(iMachineNumber);//read all the users' fingerprint templates to the memory
            while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sUserID, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                User user = new User();
                user.sUserID = sUserID;
                user.sName = sName;
                user.sPassword = sPassword;
                user.iPrivilege = iPrivilege;
                user.bEnabled = bEnabled;

                if (user.sUserID != null)
                {
                    GetFinger(ref user);
                    GetFace(ref user);
                }
                users.Add(user);
            }
            axCZKEM1.EnableDevice(iMachineNumber, true);
        }


        //Upload the 9.0 or 10.0 fingerprint arithmetic templates one by one(in strings)
        //Only TFT screen devices with firmware version Ver 6.60 version later support function "SetUserTmpExStr" and "SetUserTmpEx".
        //While you are using 9.0 fingerprint arithmetic and your device's firmware version is under ver6.60,you should use the functions "SSR_SetUserTmp" or 
        //"SSR_SetUserTmpStr" instead of "SetUserTmpExStr" or "SetUserTmpEx" in order to upload the fingerprint templates.
        public void SetUser(User user)
        {
            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            if (user.sUserID == null)
            {
                throw new Exception("Invalid user ID");
            }

            int idwErrorCode = 0;

            axCZKEM1.EnableDevice(iMachineNumber, false);
            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, user.sUserID, user.sName, user.sPassword, user.iPrivilege, user.bEnabled))//upload user information to the device
            {
                // upload finger
                if (user.iFingerLen > 0 && user.sFingerData != null)
                {
                    if (axCZKEM1.SetUserTmpExStr(iMachineNumber, user.sUserID, user.idwFingerIndex, user.iFingerFlag, user.sFingerData))//upload templates information to the device
                        Console.WriteLine("Successfully upload fingerprint template");
                }
                // upload face
                // if (user.iFaceLen > 0 && user.sFaceData != null)
                // {
                //    if (axCZKEM1.SetUserFaceStr(iMachineNumber, user.sUserID, user.iFaceIndex, user.sFaceData, user.iFaceLen))//upload face templates information to the device
                //        Console.WriteLine("Successfully upload face template");
                //    else
                //    {
                //        axCZKEM1.GetLastError(ref idwErrorCode);
                //        axCZKEM1.EnableDevice(iMachineNumber, true);
                //        throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
                //    }
                //}
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                axCZKEM1.EnableDevice(iMachineNumber, true);
                throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
            }
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(iMachineNumber, true);

        }

        //Delete a certain user's fingerprint template of specified index
        //You shuold input the the user id and the fingerprint index you will delete
        //The difference between the two functions "SSR_DelUserTmpExt" and "SSR_DelUserTmp" is that the former supports 24 bits' user id.
        public void DeleteUser(User user)
        {
            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            if (user.sUserID == null)
            {
                throw new Exception("Invalid user ID");
            }

            int idwErrorCode = 0;


            //if (user.iFaceIndex > 0)
            //{
            //    if (axCZKEM1.DelUserFace(iMachineNumber, user.sUserID, user.iFaceIndex))
            //    {
            //        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            //        Console.WriteLine("SSR_DelUserTmpExt,UserID:" + user.sUserID + " FaceIndex:" + user.iFaceIndex);
            //    }
            //    else
            //    {
            //        axCZKEM1.GetLastError(ref idwErrorCode);
            //        throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
            //    }
            //}

            // Check if finger already deleted 
            axCZKEM1.EnableDevice(iMachineNumber, false);
            if (!GetFinger(ref user))
                throw new Exception("Finger already deleted before");

            if (user.idwFingerIndex > 0)
            {
                if (axCZKEM1.SSR_DelUserTmpExt(iMachineNumber, user.sUserID, user.idwFingerIndex))
                {
                    axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                    Console.WriteLine("SSR_DelUserTmpExt,UserID:" + user.sUserID + " FingerIndex:" + user.idwFingerIndex);
                }
                else
                {
                    axCZKEM1.GetLastError(ref idwErrorCode);
                    axCZKEM1.EnableDevice(iMachineNumber, true);
                    throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
                }
            }
            axCZKEM1.EnableDevice(iMachineNumber, true);
        }
        private bool GetFace(ref User user)
        {
            string sTmpData = "";
            int iTmpLength = 0;
            int iFaceIndex = 50;//the only possible parameter value
            bool found = false;

            // get face data
            if (axCZKEM1.GetUserFaceStr(iMachineNumber, user.sUserID, iFaceIndex, ref sTmpData, ref iTmpLength))//get the face templates from the memory
            {
                found = true;
                user.iFaceIndex = iFaceIndex;
                user.sFaceData = sTmpData;
                user.iFaceLen = iTmpLength;
            }
            return found;
        }

        private bool GetFinger(ref User user)
        {
            int idwFingerIndex;
            //int iFlag = 0;
            string sTmpData = "";
            int iTmpLength = 0;
            bool found = false;

            // get finger data
            for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
            {
                if (axCZKEM1.SSR_GetUserTmpStr(iMachineNumber, user.sUserID, idwFingerIndex, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                {
                    found = true;
                    user.idwFingerIndex = idwFingerIndex;
                    //user.iFingerFlag = iFlag;
                    user.sFingerData = sTmpData;
                    user.iFingerLen = iTmpLength;
                }
            }
            return found;
        }
    }
}
