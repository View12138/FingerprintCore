
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FingerprintCore.Device.SDK.Zkfp
{
    static class ZkfpPlus
    {
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_Init();
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_Terminate();
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_GetDeviceCount();
        [DllImport("libzkfp.dll")]
        public static extern IntPtr ZKFPM_OpenDevice(int index);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_CloseDevice(IntPtr handle);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_GetCaptureParamsEx(IntPtr handle, ref int width, ref int height, ref int dpi);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_SetParameters(IntPtr handle, int nParamCode, IntPtr paramValue, int cbParamValue);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_GetParameters(IntPtr handle, int nParamCode, IntPtr paramValue, ref int cbParamValue);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_AcquireFingerprint(IntPtr handle, IntPtr fpImage, uint cbFPImage, IntPtr fpTemplate, ref int cbTemplate);
        [DllImport("libzkfp.dll")]
        public static extern IntPtr ZKFPM_CreateDBCache();
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_CloseDBCache(IntPtr hDBCache);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_GenRegTemplate(IntPtr hDBCache, IntPtr temp1, IntPtr temp2, IntPtr temp3, IntPtr regTemp, ref int cbRegTemp);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_AddRegTemplateToDBCache(IntPtr hDBCache, uint fid, IntPtr fpTemplate, uint cbTemplate);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_DelRegTemplateFromDBCache(IntPtr hDBCache, uint fid);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_ClearDBCache(IntPtr hDBCache);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_GetDBCacheCount(IntPtr hDBCache, ref int count);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_Identify(IntPtr hDBCache, IntPtr fpTemplate, uint cbTemplate, ref int FID, ref int score);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_VerifyByID(IntPtr hDBCache, uint fid, IntPtr fpTemplate, uint cbTemplate);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_MatchFinger(IntPtr hDBCache, IntPtr fpTemplate1, uint cbTemplate1, IntPtr fpTemplate2, uint cbTemplate2);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_ExtractFromImage(IntPtr hDBCache, string FilePathName, int DPI, IntPtr fpTemplate, ref int cbTemplate);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_AcquireFingerprintImage(IntPtr hDBCache, IntPtr fpImage, uint cbFPImage);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_Base64ToBlob(string src, IntPtr blob, uint cbBlob);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_BlobToBase64(IntPtr src, uint cbSrc, StringBuilder dst, uint cbDst);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_DBSetParameter(IntPtr handle, int nParamCode, int paramValue);
        [DllImport("libzkfp.dll")]
        public static extern int ZKFPM_DBGetParameter(IntPtr handle, int nParamCode, ref int paramValue);
    }
}
