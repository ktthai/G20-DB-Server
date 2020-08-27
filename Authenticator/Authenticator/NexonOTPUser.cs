//using Authenticator.com.nexon.securelogin;
using System;

namespace Authenticator
{
    public class NexonOTPUser
    {
        private long nexonOID;

        private bool isTestMode;

        public NexonOTPUser(long _nexonOID, bool _isTestMode)
        {
            nexonOID = _nexonOID;
            isTestMode = _isTestMode;
        }

        public bool IsOTPUser(out bool _isNewOTP)
        {
            _isNewOTP = false;
            //if (isTestMode)
            //{
            if (nexonOID >= 1000)
            {
                return true;
            }
            return false;

            /*
                        WorkSession.WriteStatus("NexonOTPUser.IsOTPUser(\"" + nexonOID + "\") : enter");
                        try
                        {
                            mabinogigame mabinogigame = new mabinogigame();
                            string ErrorMessage;
                            bool UsingOTP;
                            int num = mabinogigame.CheckOTP(5063, nexonOID, out ErrorMessage, out UsingOTP, out _isNewOTP);
                            WorkSession.WriteStatus("NexonOTPUser.IsOTPUser(\"" + nexonOID + "\") : result is : " + num + " / " + UsingOTP + " / " + ErrorMessage);
                            return UsingOTP;
                        }
                        catch (Exception ex)
                        {
                            WorkSession.WriteStatus("NexonOTPUser.IsOTPUser(\"" + nexonOID + "\") : Error : " + ex.Message);
                            return false;
                        }
            */
        }
    }
}
