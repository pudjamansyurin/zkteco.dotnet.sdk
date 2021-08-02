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


        public bool GetConnectState()
        {
            return bIsConnected;
        }

        public void SetConnectState(bool state)
        {
            bIsConnected = state;
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

        //Download user's 9.0 or 10.0 arithmetic fingerprint templates(in strings)
        //Only TFT screen devices with firmware version Ver 6.60 version later support function "GetUserTmpExStr" and "GetUserTmpEx".
        //'While you are using 9.0 fingerprint arithmetic and your device's firmware version is under ver6.60,you should use the functions "SSR_GetUserTmp" or 
        //"SSR_GetUserTmpStr" instead of "GetUserTmpExStr" or "GetUserTmpEx" in order to download the fingerprint templates.
        public void GetUserInfo(ref List<UserInfo> users)
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

            int idwFingerIndex;
            int iFlag = 0;
            string sTmpData = "";
            int iTmpLength = 0;

            int iFaceIndex = 50;//the only possible parameter value

            axCZKEM1.EnableDevice(iMachineNumber, false);

            axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
            axCZKEM1.ReadAllTemplate(iMachineNumber);//read all the users' fingerprint templates to the memory
            while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sUserID, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                UserInfo user = new UserInfo();
                user.sUserID = sUserID;
                user.sName = sName;
                user.sPassword = sPassword;
                user.iPrivilege = iPrivilege;
                user.bEnabled = bEnabled;

                // get finger data
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (axCZKEM1.GetUserTmpExStr(iMachineNumber, sUserID, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                    {
                        user.idwFingerIndex = idwFingerIndex;
                        user.iFingerFlag = iFlag;
                        user.sFingerData = sTmpData;
                        user.iFingerLen = iTmpLength;

                    }
                }

                // get face data
                if (axCZKEM1.GetUserFaceStr(iMachineNumber, sUserID, iFaceIndex, ref sTmpData, ref iTmpLength))//get the face templates from the memory
                {
                    user.iFaceIndex = iFaceIndex;
                    user.sFaceData = sTmpData;
                    user.iFaceLen = iTmpLength;
                }

                if (user.sUserID != null)
                    users.Add(user);
            }

            axCZKEM1.EnableDevice(iMachineNumber, true);
        }


        //Upload the 9.0 or 10.0 fingerprint arithmetic templates to the device(in strings) in batches.
        //Only TFT screen devices with firmware version Ver 6.60 version later support function "SetUserTmpExStr" and "SetUserTmpEx".
        //While you are using 9.0 fingerprint arithmetic and your device's firmware version is under ver6.60,you should use the functions "SSR_SetUserTmp" or 
        //"SSR_SetUserTmpStr" instead of "SetUserTmpExStr" or "SetUserTmpEx" in order to upload the fingerprint templates.
        //public void BatchUpdate(List<UserInfo> users)
        //{
        //    if (GetConnectState() == false)
        //    {
        //        throw new Exception("*Please connect first!");
        //    }

        //    if (users.Count == 0)
        //    {
        //        throw new Exception("There is no data to upload!");
        //    }

        //    int idwErrorCode = 0;

        //    string sdwEnrollNumber = "";
        //    string sName = "";
        //    int idwFingerIndex = 0;
        //    string sTmpData = "";
        //    int iPrivilege = 0;
        //    string sPassword = "";
        //    bool bEnabled = false;
        //    int iFlag = 1;

        //    int iUpdateFlag = 1;

        //    axCZKEM1.EnableDevice(iMachineNumber, false);
        //    if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))//create memory space for batching data
        //    {
        //        string sLastEnrollNumber = "";//the former enrollnumber you have upload(define original value as 0)
        //        for (int i = 0; i < users.Count; i++)
        //        {
        //            UserInfo user = users.ElementAt(i);
        //            sdwEnrollNumber = user.sUserID;
        //            sName = user.sName;
        //            sPassword = user.sPassword;
        //            iPrivilege = user.iPrivilege;
        //            bEnabled = user.bEnabled;

        //            idwFingerIndex = user.idwFingerIndex;
        //            sTmpData = user.sFingerData;
        //            iFlag = Convert.ToInt32(user.iFingerFlag);

        //            if (sdwEnrollNumber != sLastEnrollNumber)//identify whether the user information(except fingerprint templates) has been uploaded
        //            {
        //                if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the memory
        //                {
        //                    axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the memory
        //                }
        //                else
        //                {
        //                    axCZKEM1.GetLastError(ref idwErrorCode);
        //                    axCZKEM1.EnableDevice(iMachineNumber, true);
        //                    throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
        //                }
        //            }
        //            else//the current fingerprint and the former one belongs the same user,that is ,one user has more than one template
        //            {
        //                axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);
        //            }
        //            sLastEnrollNumber = sdwEnrollNumber;//change the value of iLastEnrollNumber dynamicly
        //        }
        //    }
        //    axCZKEM1.BatchUpdate(iMachineNumber);//upload all the information in the memory
        //    axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        //    axCZKEM1.EnableDevice(iMachineNumber, true);

        //    Console.WriteLine("Successfully upload fingerprint templates in batches , " + "total:" + users.Count, "Success");
        //}

        //Upload the 9.0 or 10.0 fingerprint arithmetic templates one by one(in strings)
        //Only TFT screen devices with firmware version Ver 6.60 version later support function "SetUserTmpExStr" and "SetUserTmpEx".
        //While you are using 9.0 fingerprint arithmetic and your device's firmware version is under ver6.60,you should use the functions "SSR_SetUserTmp" or 
        //"SSR_SetUserTmpStr" instead of "SetUserTmpExStr" or "SetUserTmpEx" in order to upload the fingerprint templates.
        public void SetUserInfo(List<UserInfo> users)
        {
            if (GetConnectState() == false)
            {
                throw new Exception("*Please connect first!");
            }

            if (users.Count == 0)
            {
                throw new Exception("There is no data to upload!");
            }

            int idwErrorCode = 0;

            string sUserID = "";
            string sName = "";
            int iPrivilege = 0;
            string sPassword = "";
            bool bEnabled = false;
            int idwFingerIndex = 0;
            string sTmpData = "";
            int iFlag = 0;
            int iFaceIndex = 0;
            int iLength = 0;

            axCZKEM1.EnableDevice(iMachineNumber, false);
            for (int i = 0; i < users.Count; i++)
            {
                UserInfo user = users[i];
                sUserID = user.sUserID;
                sName = user.sName;
                sPassword = user.sPassword;
                iPrivilege = user.iPrivilege;
                bEnabled = user.bEnabled;

                if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sUserID, sName, sPassword, iPrivilege, bEnabled))//upload user information to the device
                {
                    // upload finger
                    idwFingerIndex = user.idwFingerIndex;
                    sTmpData = user.sFingerData;
                    iFlag = user.iFingerFlag;
                    axCZKEM1.SetUserTmpExStr(iMachineNumber, sUserID, idwFingerIndex, iFlag, sTmpData);//upload templates information to the device

                    // upload face
                    iFaceIndex = user.iFaceIndex;
                    sTmpData = user.sFaceData;
                    iLength = user.iFaceLen;
                    axCZKEM1.SetUserFaceStr(iMachineNumber, sUserID, iFaceIndex, sTmpData, iLength);//upload face templates information to the device
                }
                else
                {
                    axCZKEM1.GetLastError(ref idwErrorCode);
                    axCZKEM1.EnableDevice(iMachineNumber, true);
                    throw new Exception("Operation failed,ErrorCode=" + idwErrorCode.ToString());
                }
            }
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(iMachineNumber, true);

            Console.WriteLine("Successfully upload fingerprint templates in batches , " + "total:" + users.Count, "Success");
        }

        //        //Delete a certain user's fingerprint template of specified index
        //        //You shuold input the the user id and the fingerprint index you will delete
        //        //The difference between the two functions "SSR_DelUserTmpExt" and "SSR_DelUserTmp" is that the former supports 24 bits' user id.
        //        private void btnSSR_DelUserTmpExt_Click(object sender, EventArgs e)
        //        {
        //            if (bIsConnected == false)
        //            {
        //                MessageBox.Show("Please connect the device first!", "Error");
        //                return;
        //            }

        //            if (cbUserIDTmp.Text.Trim() == "" || cbFingerIndex.Text.Trim() == "")
        //            {
        //                MessageBox.Show("Please input the UserID and FingerIndex first!", "Error");
        //                return;
        //            }
        //            int idwErrorCode = 0;

        //            string sUserID = cbUserIDTmp.Text.Trim();
        //            int iFingerIndex = Convert.ToInt32(cbFingerIndex.Text.Trim());

        //            Cursor = Cursors.WaitCursor;
        //            if (axCZKEM1.SSR_DelUserTmpExt(iMachineNumber, sUserID, iFingerIndex))
        //            {
        //                axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        //                MessageBox.Show("SSR_DelUserTmpExt,UserID:" + sUserID + " FingerIndex:" + iFingerIndex.ToString(), "Success");
        //            }
        //            else
        //            {
        //                axCZKEM1.GetLastError(ref idwErrorCode);
        //                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        //            }
        //            Cursor = Cursors.Default;
        //        }

        //        //Clear all the fingerprint templates in the device(While the parameter DataFlag  of the Function "ClearData" is 2 )
        //        private void btnClearDataTmps_Click(object sender, EventArgs e)
        //        {
        //            if (bIsConnected == false)
        //            {
        //                MessageBox.Show("Please connect the device first!", "Error");
        //                return;
        //            }
        //            int idwErrorCode = 0;

        //            int iDataFlag = 2;

        //            Cursor = Cursors.WaitCursor;
        //            if (axCZKEM1.ClearData(iMachineNumber, iDataFlag))
        //            {
        //                axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        //                MessageBox.Show("Clear all the fingerprint templates!", "Success");
        //            }
        //            else
        //            {
        //                axCZKEM1.GetLastError(ref idwErrorCode);
        //                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        //            }
        //            Cursor = Cursors.Default;
        //        }

        //        //Delete all the user information in the device,while the related fingerprint templates will be deleted either. 
        //        //(While the parameter DataFlag  of the Function "ClearData" is 5 )
        //        private void btnClearDataUserInfo_Click(object sender, EventArgs e)
        //        {
        //            if (bIsConnected == false)
        //            {
        //                MessageBox.Show("Please connect the device first!", "Error");
        //                return;
        //            }
        //            int idwErrorCode = 0;

        //            int iDataFlag = 5;

        //            Cursor = Cursors.WaitCursor;
        //            if (axCZKEM1.ClearData(iMachineNumber, iDataFlag))
        //            {
        //                axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        //                MessageBox.Show("Clear all the UserInfo data!", "Success");
        //            }
        //            else
        //            {
        //                axCZKEM1.GetLastError(ref idwErrorCode);
        //                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        //            }
        //            Cursor = Cursors.Default;
        //        }

        //        //Delete a kind of data that some user has enrolled
        //        //The range of the Backup Number is from 0 to 9 and the specific meaning of Backup number is described in the development manual,pls refer to it.
        //        private void btnDeleteEnrollData_Click(object sender, EventArgs e)
        //        {
        //            if (bIsConnected == false)
        //            {
        //                MessageBox.Show("Please connect the device first!", "Error");
        //                return;
        //            }

        //            if (cbUserIDDE.Text.Trim() == "" || cbBackupDE.Text.Trim() == "")
        //            {
        //                MessageBox.Show("Please input the UserID and BackupNumber first!", "Error");
        //                return;
        //            }
        //            int idwErrorCode = 0;

        //            string sUserID = cbUserIDDE.Text.Trim();
        //            int iBackupNumber = Convert.ToInt32(cbBackupDE.Text.Trim());

        //            Cursor = Cursors.WaitCursor;
        //            if (axCZKEM1.SSR_DeleteEnrollData(iMachineNumber, sUserID, iBackupNumber))
        //            {
        //                axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        //                MessageBox.Show("DeleteEnrollData,UserID=" + sUserID + " BackupNumber=" + iBackupNumber.ToString(), "Success");
        //            }
        //            else
        //            {
        //                axCZKEM1.GetLastError(ref idwErrorCode);
        //                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        //            }
        //            Cursor = Cursors.Default;
        //        }

        //        //Delete a certain user's face template according to its id
        //        private void btnDelUserFace_Click(object sender, EventArgs e)
        //        {
        //            if (bIsConnected == false)
        //            {
        //                MessageBox.Show("Please connect the device first!", "Error");
        //                return;
        //            }

        //            if (cbUserID3.Text.Trim() == "")
        //            {
        //                MessageBox.Show("Please input the UserID first!", "Error");
        //                return;
        //            }
        //            int idwErrorCode = 0;

        //            string sUserID = cbUserID3.Text.Trim();
        //            int iFaceIndex = 50;

        //            Cursor = Cursors.WaitCursor;
        //            if (axCZKEM1.DelUserFace(iMachineNumber, sUserID, iFaceIndex))
        //            {
        //                axCZKEM1.RefreshData(iMachineNumber);
        //                MessageBox.Show("DelUserFace,UserID=" + sUserID, "Success");
        //            }
        //            else
        //            {
        //                axCZKEM1.GetLastError(ref idwErrorCode);
        //                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        //            }
        //            Cursor = Cursors.Default;

        //        }

    }
}
