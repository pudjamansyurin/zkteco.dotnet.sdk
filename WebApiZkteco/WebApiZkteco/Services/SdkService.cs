using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiZkteco.Models;

namespace WebApiZkteco.Services
{
    public class SdkService
    {

        private zkemkeeper.CZKEMClass axCZKEM1;
        private string Ip;
        private Int32 Port;
        private bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

        public SdkService (string ip, Int32 port)
        {
            Ip = ip;
            Port = port;
            //Create Standalone SDK class dynamicly.
            axCZKEM1 = new zkemkeeper.CZKEMClass();
        }

        public bool Connect()
        {
            int idwErrorCode = 0;

            axCZKEM1.PullMode = 1;
            SetConnectState(axCZKEM1.Connect_Net(Ip, Port));
            if (GetConnectState() == true)
            {
                SetMachineNumber(1); //In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                axCZKEM1.RegEvent(GetMachineNumber(), 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
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


        public bool GetConnectState()
        {
            return bIsConnected;
        }

        public void SetConnectState(bool state)
        {
            bIsConnected = state;
        }

        public int GetMachineNumber()
        {
            return iMachineNumber;
        }

        public void SetMachineNumber(int Number)
        {
            iMachineNumber = Number;
        }

        public int GetDeviceInfo(ref DeviceInfo info)
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

            axCZKEM1.EnableDevice(GetMachineNumber(), false);//disable the device

            axCZKEM1.GetSysOption(GetMachineNumber(), "~ZKFPVersion", out strTemp);
            info.iFPAlg = Convert.ToInt32(strTemp);

            axCZKEM1.GetSysOption(GetMachineNumber(), "ZKFaceVersion", out strTemp);
            info.iFaceAlg = Convert.ToInt32(strTemp);

            /*
            axCZKEM1.GetDeviceInfo(GetMachineNumber(), 72, ref iFPAlg);
            axCZKEM1.GetDeviceInfo(GetMachineNumber(), 73, ref iFaceAlg);
            */

            axCZKEM1.GetVendor(ref sProducter);
            info.sProducter = sProducter;

            axCZKEM1.GetProductCode(GetMachineNumber(), out sDeviceName);
            info.sDeviceName = sDeviceName;

            axCZKEM1.GetDeviceMAC(GetMachineNumber(), ref sMac);
            info.sMac = sMac;

            axCZKEM1.GetFirmwareVersion(GetMachineNumber(), ref sFirmver);
            info.sFirmver = sFirmver;

            /*
            if (sta_GetDeviceType() == 1)
            {
                axCZKEM1.GetDeviceFirmwareVersion(GetMachineNumber(), ref sFirmver);
            }
             */
            //lblOutputInfo.Items.Add("[func GetDeviceFirmwareVersion]Temporarily unsupported");

            axCZKEM1.GetPlatform(GetMachineNumber(), ref sPlatform);
            info.sPlatform = sPlatform;

            axCZKEM1.GetSerialNumber(GetMachineNumber(), out sSN);
            info.sSN = sSN;

            axCZKEM1.GetDeviceStrInfo(GetMachineNumber(), 1, out sProductTime);
            info.sProductTime = sProductTime;

            axCZKEM1.EnableDevice(GetMachineNumber(), true);//enable the device

            iRet = 1;
            return iRet;
        }

        public void GetUserInfo(ref List<UserInfo> users)
        {
            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            int idwFingerIndex;
            string sTmpData = "";
            int iTmpLength = 0;
            int iFlag = 0;

            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            axCZKEM1.EnableDevice(GetMachineNumber(), false);

            axCZKEM1.ReadAllUserID(GetMachineNumber());//read all the user information to the memory
            axCZKEM1.ReadAllTemplate(GetMachineNumber());//read all the users' fingerprint templates to the memory
            while (axCZKEM1.SSR_GetAllUserInfo(GetMachineNumber(), out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (axCZKEM1.GetUserTmpExStr(GetMachineNumber(), sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                    {
                        UserInfo user = new UserInfo();
                        user.sdwEnrollNumber = sdwEnrollNumber;
                        user.sName = sName;
                        user.idwFingerIndex = idwFingerIndex;
                        user.sData = sTmpData;
                        user.iPrivilege = iPrivilege;
                        user.sPassword = sPassword;
                        user.bEnabled = bEnabled;
                        user.iFlag = iFlag;

                        users.Add(user);
                    }
                }
            }

            axCZKEM1.EnableDevice(GetMachineNumber(), true);
        }
    }
}
