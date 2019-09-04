using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZkfpHelp
{
    /// <summary>
    /// 中控指纹API
    /// </summary>
    public class ZkfpSDK
    {
        #region 导入函数
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_Init();
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_Terminate();
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_GetDeviceCount();
        [DllImport("libzkfp.dll")]
        private static extern IntPtr ZKFPM_OpenDevice(int index);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_CloseDevice(IntPtr handle);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_GetCaptureParamsEx(IntPtr handle, ref int width, ref int height, ref int dpi);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_SetParameters(IntPtr handle, int nParamCode, IntPtr paramValue, int cbParamValue);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_GetParameters(IntPtr handle, int nParamCode, IntPtr paramValue, ref int cbParamValue);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_AcquireFingerprint(IntPtr handle, IntPtr fpImage, uint cbFPImage, IntPtr fpTemplate, ref int cbTemplate);
        [DllImport("libzkfp.dll")]
        private static extern IntPtr ZKFPM_CreateDBCache();
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_CloseDBCache(IntPtr hDBCache);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_GenRegTemplate(IntPtr hDBCache, IntPtr temp1, IntPtr temp2, IntPtr temp3, IntPtr regTemp, ref int cbRegTemp);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_AddRegTemplateToDBCache(IntPtr hDBCache, uint fid, IntPtr fpTemplate, uint cbTemplate);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_DelRegTemplateFromDBCache(IntPtr hDBCache, uint fid);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_ClearDBCache(IntPtr hDBCache);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_GetDBCacheCount(IntPtr hDBCache, ref int count);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_Identify(IntPtr hDBCache, IntPtr fpTemplate, uint cbTemplate, ref int FID, ref int score);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_VerifyByID(IntPtr hDBCache, uint fid, IntPtr fpTemplate, uint cbTemplate);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_MatchFinger(IntPtr hDBCache, IntPtr fpTemplate1, uint cbTemplate1, IntPtr fpTemplate2, uint cbTemplate2);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_ExtractFromImage(IntPtr hDBCache, string FilePathName, int DPI, IntPtr fpTemplate, ref int cbTemplate);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_AcquireFingerprintImage(IntPtr hDBCache, IntPtr fpImage, uint cbFPImage);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_Base64ToBlob(string src, IntPtr blob, uint cbBlob);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_BlobToBase64(IntPtr src, uint cbSrc, StringBuilder dst, uint cbDst);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_DBSetParameter(IntPtr handle, int nParamCode, int paramValue);
        [DllImport("libzkfp.dll")]
        private static extern int ZKFPM_DBGetParameter(IntPtr handle, int nParamCode, ref int paramValue);
        #endregion

        /// <summary>
        /// 初始化库
        /// </summary>
        /// <returns></returns>
        public static int Init()
        {
            return ZkfpSDK.ZKFPM_Init();
        }
        /// <summary>
        /// 释放库资源
        /// </summary>
        /// <returns></returns>
        public static int Terminate()
        {
            return ZkfpSDK.ZKFPM_Terminate();
        }
        /// <summary>
        /// 获取连接设备数
        /// </summary>
        /// <returns></returns>
        public static int GetDeviceCount()
        {
            return ZkfpSDK.ZKFPM_GetDeviceCount();
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="index">设备索引(0~n,n 为设备数-1)</param>
        /// <returns>返回设备句柄</returns>
        public static IntPtr OpenDevice(int index)
        {
            return ZkfpSDK.ZKFPM_OpenDevice(index);
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <returns></returns>
        public static int CloseDevice(IntPtr devHandle)
        {
            return ZkfpSDK.ZKFPM_CloseDevice(devHandle);
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="code">参数代码(见附录说明)</param>
        /// <param name="pramValue">参数值</param>
        /// <param name="size">参数数据长度</param>
        /// <returns></returns>
        public static int SetParameters(IntPtr devHandle, int code, byte[] pramValue, int size)
        {
            if (IntPtr.Zero == devHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            if (pramValue == null || pramValue.Length < size || size <= 0)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_PARAM;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(pramValue, 0, intPtr, size);
            int result = ZkfpSDK.ZKFPM_SetParameters(devHandle, code, intPtr, size);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="code">参数代码(见附录说明)</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="size">返回参数数据长度</param>
        /// <returns></returns>
        public static int GetParameters(IntPtr devHandle, int code, byte[] paramValue, ref int size)
        {
            if (IntPtr.Zero == devHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            size = paramValue.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            int num = ZkfpSDK.ZKFPM_GetParameters(devHandle, code, intPtr, ref size);
            if (ZkfpErrorCode.ZKFP_ERR_OK == num)
            {
                Marshal.Copy(intPtr, paramValue, 0, size);
            }
            Marshal.FreeHGlobal(intPtr);
            return num;
        }
        /// <summary>
        /// 采集指纹
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="imgBuffer">返回图像（数组大小为 imageWidth*imageHeight）</param>
        /// <param name="template">返回指纹模板(建议预分配 2048Bytes)</param>
        /// <param name="size">[in]template 数组长度 [out]实际返回指纹模板长度</param>
        /// <returns></returns>
        public static int AcquireFingerprint(IntPtr devHandle, byte[] imgBuffer, byte[] template, ref int size)
        {
            if (IntPtr.Zero == devHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            size = template.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(imgBuffer.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(size);
            int num = ZkfpSDK.ZKFPM_AcquireFingerprint(devHandle, intPtr, (uint)imgBuffer.Length, intPtr2, ref size);
            if (ZkfpErrorCode.ZKFP_ERR_OK == num)
            {
                Marshal.Copy(intPtr, imgBuffer, 0, imgBuffer.Length);
                Marshal.Copy(intPtr2, template, 0, size);
            }
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            return num;
        }
        /// <summary>
        /// 以异步方式采集指纹。
        /// <para>采集成功则返回指纹的大小，采集失败则返回 <see cref="ZkfpErrorCode"/> 中对应的错误代码。</para>
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="imgBuffer">返回图像（数组大小为 imageWidth*imageHeight）</param>
        /// <param name="template">返回指纹模板(建议预分配 2048Bytes)</param>
        /// <returns></returns>
        public async static Task<int> AcquireFingerprintAsync(IntPtr devHandle, byte[] imgBuffer, byte[] template)
        {
            if (IntPtr.Zero == devHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            int size = template.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(imgBuffer.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(size);
            int num = 0;
            await Task.Run(() =>
            {
                do
                {
                    num = ZkfpSDK.ZKFPM_AcquireFingerprint(devHandle, intPtr, (uint)imgBuffer.Length, intPtr2, ref size);
                    Thread.Sleep(200);
                }
                while (num == ZkfpErrorCode.ZKFP_ERR_CAPTURE);
            });
            if (ZkfpErrorCode.ZKFP_ERR_OK == num)
            {
                Marshal.Copy(intPtr, imgBuffer, 0, imgBuffer.Length);
                Marshal.Copy(intPtr2, template, 0, size);
            }
            else
            {
                size = num;
            }
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            return size;
        }
        /// <summary>
        /// 采集指纹图像
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="imgbuf">返回图像（数组大小为 imageWidth*imageHeight）</param>
        /// <returns></returns>
        public static int AcquireFingerprintImage(IntPtr devHandle, byte[] imgbuf)
        {
            if (IntPtr.Zero == devHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(imgbuf.Length);
            int num = ZkfpSDK.ZKFPM_AcquireFingerprintImage(devHandle, intPtr, (uint)imgbuf.Length);
            if (ZkfpErrorCode.ZKFP_ERR_OK == num)
            {
                Marshal.Copy(intPtr, imgbuf, 0, imgbuf.Length);
            }
            Marshal.FreeHGlobal(intPtr);
            return num;
        }
        /// <summary>
        /// 初始化算法库
        /// </summary>
        /// <returns>算法操作句柄</returns>
        public static IntPtr DBInit()
        {
            return ZkfpSDK.ZKFPM_CreateDBCache();
        }
        /// <summary>
        /// 释放算法库
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <returns></returns>
        public static int DBFree(IntPtr dbHandle)
        {
            return ZkfpSDK.ZKFPM_ClearDBCache(dbHandle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHandle"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DBSetParameter(IntPtr dbHandle, int code, int value)
        {
            return ZkfpSDK.ZKFPM_DBSetParameter(dbHandle, code, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHandle"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DBGetParameter(IntPtr dbHandle, int code, ref int value)
        {
            return ZkfpSDK.ZKFPM_DBGetParameter(dbHandle, code, ref value);
        }
        /// <summary>
        /// 将三枚预登记指纹模板合并成一枚登记模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="temp1">预登记指纹模板 1</param>
        /// <param name="temp2">预登记指纹模板 2</param>
        /// <param name="temp3">预登记指纹模板 3</param>
        /// <param name="regTemp">返回登记模板</param>
        /// <param name="regTempLen">[in]regTemp 数组长度,[out]实际返回指纹模板长度</param>
        /// <returns></returns>
        public static int DBMerge(IntPtr dbHandle, byte[] temp1, byte[] temp2, byte[] temp3, byte[] regTemp, ref int regTempLen)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            regTempLen = regTemp.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(temp1.Length);
            Marshal.Copy(temp1, 0, intPtr, temp1.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(temp2.Length);
            Marshal.Copy(temp2, 0, intPtr2, temp2.Length);
            IntPtr intPtr3 = Marshal.AllocHGlobal(temp3.Length);
            Marshal.Copy(temp3, 0, intPtr3, temp3.Length);
            IntPtr intPtr4 = Marshal.AllocHGlobal(regTemp.Length);
            int num = ZkfpSDK.ZKFPM_GenRegTemplate(dbHandle, intPtr, intPtr2, intPtr3, intPtr4, ref regTempLen);
            if (ZkfpErrorCode.ZKFP_ERR_OK == num)
            {
                Marshal.Copy(intPtr4, regTemp, 0, regTempLen);
            }
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            Marshal.FreeHGlobal(intPtr3);
            Marshal.FreeHGlobal(intPtr4);
            return num;
        }
        /// <summary>
        /// 添加一枚登记模板到内存
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="fid">指纹 ID(1:N 识别成功返回指纹 ID)</param>
        /// <param name="regTemp">登记模板</param>
        /// <returns></returns>
        public static int DBAdd(IntPtr dbHandle, int fid, byte[] regTemp)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(regTemp.Length);
            Marshal.Copy(regTemp, 0, intPtr, regTemp.Length);
            int result = ZkfpSDK.ZKFPM_AddRegTemplateToDBCache(dbHandle, (uint)fid, intPtr, (uint)regTemp.Length);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }
        /// <summary>
        /// 从内存中删除一枚登记模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="fid">指纹 ID(1:N 识别成功返回指纹 ID)</param>
        /// <returns></returns>
        public static int DBDel(IntPtr dbHandle, int fid)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            return ZkfpSDK.ZKFPM_DelRegTemplateFromDBCache(dbHandle, (uint)fid);
        }
        /// <summary>
        /// 清空内存中所有指纹模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <returns></returns>
        public static int DBClear(IntPtr dbHandle)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            return ZkfpSDK.ZKFPM_ClearDBCache(dbHandle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHandle"></param>
        /// <returns></returns>
        public static int DBCount(IntPtr dbHandle)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            int result = 0;
            ZkfpSDK.ZKFPM_GetDBCacheCount(dbHandle, ref result);
            return result;
        }
        /// <summary>
        /// 1:N 识别
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="temp">比对模板</param>
        /// <param name="fid">返回指纹 ID</param>
        /// <param name="score">返回比对分数</param>
        /// <returns></returns>
        public static int DBIdentify(IntPtr dbHandle, byte[] temp, ref int fid, ref int score)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(temp.Length);
            Marshal.Copy(temp, 0, intPtr, temp.Length);
            int result = ZkfpSDK.ZKFPM_Identify(dbHandle, intPtr, (uint)temp.Length, ref fid, ref score);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }
        /// <summary>
        /// 1:1 比对两枚指纹
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="temp1">比对模板 1</param>
        /// <param name="temp2">比对模板 2</param>
        /// <returns>>=0 表示比对分数，其他失败（见错误代码说明）</returns>
        public static int DBMatch(IntPtr dbHandle, byte[] temp1, byte[] temp2)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_INVALID_HANDLE;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(temp1.Length);
            Marshal.Copy(temp1, 0, intPtr, temp1.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(temp2.Length);
            Marshal.Copy(temp2, 0, intPtr2, temp2.Length);
            int result = ZkfpSDK.ZKFPM_MatchFinger(dbHandle, intPtr, (uint)temp1.Length, intPtr2, (uint)temp2.Length);
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            return result;
        }
        /// <summary>
        /// 从 BMP 或者 JPG 文件提取模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="FileName">文件全路径</param>
        /// <param name="DPI">图像 DPI</param>
        /// <param name="template">返回指纹模板(建议预分配 2048Bytes)</param>
        /// <param name="size">[in]template 数组长度, [out] 实际返回指纹模板长度</param>
        /// <returns></returns>
        public static int ExtractFromImage(IntPtr dbHandle, string FileName, int DPI, byte[] template, ref int size)
        {
            if (IntPtr.Zero == dbHandle)
            {
                return ZkfpErrorCode.ZKFP_ERR_NOT_INIT;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(template.Length);
            int num = ZkfpSDK.ZKFPM_ExtractFromImage(dbHandle, FileName, DPI, intPtr, ref size);
            if (ZkfpErrorCode.ZKFP_ERR_OK == num)
            {
                Marshal.Copy(intPtr, template, 0, size);
            }
            Marshal.FreeHGlobal(intPtr);
            return num;
        }
        /// <summary>
        /// Base64 字符串转 byte[]
        /// </summary>
        /// <param name="base64Str">Base64 字符串</param>
        /// <returns>byte[]数组</returns>
        public static byte[] Base64ToBlob(string base64Str)
        {
            //if (base64Str == null || base64Str.Length <= 0 || base64Str.Length % 4 != 0)
            //{
            //    return null;
            //}
            ////Zkfp2.ZKFPM_Base64ToBlob(base64Str,)
            //return Convert.FromBase64String(base64Str);
            if (base64Str == null || base64Str.Length <= 0 || base64Str.Length % 4 != 0)
            {
                return null;
            }
            byte[] array = Convert.FromBase64String(base64Str);
            //int num = (int)array[8] << 8 & 65280;
            //num += (int)array[9];
            //if (num > 2048 || array.Length < num)
            //{
            //    return null;
            //}
            return array;
        }
        /// <summary>
        /// byte[]转 Base64 字符串
        /// </summary>
        /// <param name="blob">blob 数据</param>
        /// <param name="nDataLen">长度</param>
        /// <returns>返回 Base64 字符串</returns>
        public static string BlobToBase64(byte[] blob, int nDataLen)
        {
            if (blob == null || blob.Length <= 0 || nDataLen <= 0 || blob.Length < nDataLen)
            {
                return "";
            }
            int num = blob.Length / 3 * 4 + 1;
            StringBuilder stringBuilder = new StringBuilder(num);
            IntPtr intPtr = Marshal.AllocHGlobal(blob.Length);
            Marshal.Copy(blob, 0, intPtr, blob.Length);
            int num2 = ZkfpSDK.ZKFPM_BlobToBase64(intPtr, (uint)nDataLen, stringBuilder, (uint)num);
            Marshal.FreeHGlobal(intPtr);
            if (num2 > 0)
            {
                return stringBuilder.ToString();
            }
            return "";
        }
        /// <summary>
        /// 4 字节 byte 数组转 Int
        /// </summary>
        /// <param name="buf">byte 数组</param>
        /// <param name="value">返回数据</param>
        /// <returns>true 成功;false 失败</returns>
        public static bool ByteArray2Int(byte[] buf, ref int value)
        {
            if (buf.Length < 4)
            {
                return false;
            }
            value = BitConverter.ToInt32(buf, 0);
            return true;
        }
        /// <summary>
        /// Int 转 4 字节 byte 数组
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="buf">byte 数组</param>
        /// <returns>true 成功;false 失败</returns>
        public static bool Int2ByteArray(int value, byte[] buf)
        {
            if (buf == null)
            {
                return false;
            }
            if (buf.Length < 4)
            {
                return false;
            }
            buf[0] = (byte)(value & 255);
            buf[1] = (byte)((value & 65280) >> 8);
            buf[2] = (byte)((value & 16711680) >> 16);
            buf[3] = (byte)(value >> 24 & 255);
            return true;
        }
    }
}
