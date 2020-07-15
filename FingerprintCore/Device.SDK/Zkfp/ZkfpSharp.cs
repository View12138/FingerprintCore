using System;
using System.Runtime.InteropServices;

namespace FingerprintCore.Device.SDK.Zkfp
{
    /// <summary>
    /// C# 版本的
    /// </summary>
    static class ZkfpSharp
    {
        /// <summary>
        /// 初始化库
        /// </summary>
        /// <returns>0 成功，其他失败（见错误代码说明）</returns>
        public static ErrorCode Init()
        {
            return (ErrorCode)ZkfpPlus.ZKFPM_Init();
        }
        /// <summary>
        /// 释放库资源
        /// </summary>
        /// <returns>0 成功，其他失败（见错误代码说明）</returns>
        public static ErrorCode Terminate()
        {
            return (ErrorCode)ZkfpPlus.ZKFPM_Terminate();
        }
        /// <summary>
        /// 获取连接设备数
        /// </summary>
        /// <returns>返回设备数</returns>
        public static int GetDeviceCount()
        {
            return ZkfpPlus.ZKFPM_GetDeviceCount();
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="index">设备索引(0~n,n 为设备数-1)</param>
        /// <returns>返回设备句柄</returns>
        public static IntPtr OpenDevice(int index)
        {
            return ZkfpPlus.ZKFPM_OpenDevice(index);
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <returns></returns>
        public static ErrorCode CloseDevice(ref IntPtr devHandle)
        {
            int code = ZkfpPlus.ZKFPM_CloseDevice(devHandle);
            devHandle = IntPtr.Zero;
            return (ErrorCode)code;
        }
        /// <summary>
        /// 采集指纹
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="imgBuffer">返回图像（数组大小为 imageWidth*imageHeight）</param>
        /// <param name="template">返回指纹模板(建议预分配 2048Bytes)</param>
        /// <returns></returns>
        public static ErrorCode AcquireFingerprint(IntPtr devHandle, byte[] imgBuffer, byte[] template)
        {
            if (IntPtr.Zero == devHandle)
            { return ErrorCode.InvalidHandle; }

            int size = template.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(imgBuffer.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(size);
            int num = ZkfpPlus.ZKFPM_AcquireFingerprint(devHandle, intPtr, (uint)imgBuffer.Length, intPtr2, ref size);
            if (num == (int)ErrorCode.OK)
            {
                Marshal.Copy(intPtr, imgBuffer, 0, imgBuffer.Length);
                Marshal.Copy(intPtr2, template, 0, size);
            }
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            return (ErrorCode)num;
        }

        /// <summary>
        /// 采集指纹图像
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="imgbuf">返回图像（数组大小为 imageWidth*imageHeight）</param>
        /// <returns></returns>
        public static ErrorCode AcquireFingerprintImage(IntPtr devHandle, byte[] imgbuf)
        {
            if (IntPtr.Zero == devHandle)
            { return ErrorCode.InvalidHandle; }

            IntPtr intPtr = Marshal.AllocHGlobal(imgbuf.Length);
            int num = ZkfpPlus.ZKFPM_AcquireFingerprintImage(devHandle, intPtr, (uint)imgbuf.Length);
            if (num == (int)ErrorCode.OK)
            {
                Marshal.Copy(intPtr, imgbuf, 0, imgbuf.Length);
            }
            Marshal.FreeHGlobal(intPtr);
            return (ErrorCode)num;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="code">参数代码(见附录说明)</param>
        /// <param name="pramValue">参数值</param>
        /// <returns></returns>
        public static ErrorCode SetParameters(IntPtr devHandle, ParamCode code, byte[] pramValue)
        {
            if (devHandle == IntPtr.Zero)
            { return ErrorCode.InvalidHandle; }
            if (pramValue == null)
            { return ErrorCode.InvalidParam; }

            IntPtr intPtr = Marshal.AllocHGlobal(pramValue.Length);
            Marshal.Copy(pramValue, 0, intPtr, pramValue.Length);
            int result = ZkfpPlus.ZKFPM_SetParameters(devHandle, (int)code, intPtr, pramValue.Length);
            Marshal.FreeHGlobal(intPtr);
            return (ErrorCode)result;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="devHandle">设备句柄</param>
        /// <param name="code">参数代码(见附录说明)</param>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public static ErrorCode GetParameters(IntPtr devHandle, ParamCode code, byte[] paramValue)
        {
            if (IntPtr.Zero == devHandle)
            { return ErrorCode.InvalidHandle; }
            int size = paramValue.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            int num = ZkfpPlus.ZKFPM_GetParameters(devHandle, (int)code, intPtr, ref size);
            if (num == (int)ErrorCode.OK)
            {
                Marshal.Copy(intPtr, paramValue, 0, size);
            }
            Marshal.FreeHGlobal(intPtr);
            return (ErrorCode)num;
        }

        /// <summary>
        /// 初始化算法库
        /// </summary>
        /// <returns>算法操作句柄</returns>
        public static IntPtr DBInit()
        {
            return ZkfpPlus.ZKFPM_CreateDBCache();
        }

        /// <summary>
        /// 释放算法库
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <returns></returns>
        public static ErrorCode DBFree(IntPtr dbHandle)
        {
            return (ErrorCode)ZkfpPlus.ZKFPM_ClearDBCache(dbHandle);
        }

        /// <summary>
        /// 将三枚预登记指纹模板合并成一枚登记模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="temp1">预登记指纹模板 1</param>
        /// <param name="temp2">预登记指纹模板 2</param>
        /// <param name="temp3">预登记指纹模板 3</param>
        /// <param name="regTemp">返回登记模板</param>
        /// <returns></returns>
        public static ErrorCode DBMerge(IntPtr dbHandle, byte[] temp1, byte[] temp2, byte[] temp3, byte[] regTemp)
        {
            if (IntPtr.Zero == dbHandle)
            { return ErrorCode.InvalidHandle; }

            int regTempLen = regTemp.Length;
            IntPtr intPtr = Marshal.AllocHGlobal(temp1.Length);
            Marshal.Copy(temp1, 0, intPtr, temp1.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(temp2.Length);
            Marshal.Copy(temp2, 0, intPtr2, temp2.Length);
            IntPtr intPtr3 = Marshal.AllocHGlobal(temp3.Length);
            Marshal.Copy(temp3, 0, intPtr3, temp3.Length);
            IntPtr intPtr4 = Marshal.AllocHGlobal(regTemp.Length);
            int num = ZkfpPlus.ZKFPM_GenRegTemplate(dbHandle, intPtr, intPtr2, intPtr3, intPtr4, ref regTempLen);
            if (num == (int)ErrorCode.OK)
            {
                Marshal.Copy(intPtr4, regTemp, 0, regTempLen);
            }
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            Marshal.FreeHGlobal(intPtr3);
            Marshal.FreeHGlobal(intPtr4);
            return (ErrorCode)num;
        }

        /// <summary>
        /// 添加一枚登记模板到内存
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="fid">指纹 ID(1:N 识别成功返回指纹 ID)</param>
        /// <param name="regTemp">登记模板</param>
        /// <returns></returns>
        public static ErrorCode DBAdd(IntPtr dbHandle, int fid, byte[] regTemp)
        {
            if (IntPtr.Zero == dbHandle)
            { return ErrorCode.InvalidHandle; }

            IntPtr intPtr = Marshal.AllocHGlobal(regTemp.Length);
            Marshal.Copy(regTemp, 0, intPtr, regTemp.Length);
            int result = ZkfpPlus.ZKFPM_AddRegTemplateToDBCache(dbHandle, (uint)fid, intPtr, (uint)regTemp.Length);
            Marshal.FreeHGlobal(intPtr);
            return (ErrorCode)result;
        }

        /// <summary>
        /// 从内存中删除一枚登记模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <param name="fid">指纹 ID(1:N 识别成功返回指纹 ID)</param>
        /// <returns></returns>
        public static ErrorCode DBDel(IntPtr dbHandle, int fid)
        {
            if (IntPtr.Zero == dbHandle)
            { return ErrorCode.InvalidHandle; }

            return (ErrorCode)ZkfpPlus.ZKFPM_DelRegTemplateFromDBCache(dbHandle, (uint)fid);
        }

        /// <summary>
        /// 清空内存中所有指纹模板
        /// </summary>
        /// <param name="dbHandle">算法操作句柄</param>
        /// <returns></returns>
        public static ErrorCode DBClear(IntPtr dbHandle)
        {
            if (IntPtr.Zero == dbHandle)
            { return ErrorCode.InvalidHandle; }

            return (ErrorCode)ZkfpPlus.ZKFPM_ClearDBCache(dbHandle);
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
            { return (int)ErrorCode.InvalidHandle; }

            IntPtr intPtr = Marshal.AllocHGlobal(temp1.Length);
            Marshal.Copy(temp1, 0, intPtr, temp1.Length);
            IntPtr intPtr2 = Marshal.AllocHGlobal(temp2.Length);
            Marshal.Copy(temp2, 0, intPtr2, temp2.Length);
            int result = ZkfpPlus.ZKFPM_MatchFinger(dbHandle, intPtr, (uint)temp1.Length, intPtr2, (uint)temp2.Length);
            Marshal.FreeHGlobal(intPtr);
            Marshal.FreeHGlobal(intPtr2);
            return result;
        }

        /// <summary>
        /// 4 字节 byte 数组转 Int
        /// </summary>
        /// <param name="buffer">byte 数组</param>
        /// <returns>true 成功;false 失败</returns>
        public static int ByteArray2Int(byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Int 转 4 字节 byte 数组
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>true 成功;false 失败</returns>
        public static byte[] Int2ByteArray(int value)
        {
            byte[] buffer = new byte[4];
            buffer[0] = (byte)(value & 255);
            buffer[1] = (byte)((value & 65280) >> 8);
            buffer[2] = (byte)((value & 16711680) >> 16);
            buffer[3] = (byte)(value >> 24 & 255);
            return buffer;
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
        public static ErrorCode ExtractFromImage(IntPtr dbHandle, string FileName, int DPI, byte[] template, ref int size)
        {
            if (IntPtr.Zero == dbHandle)
            { return ErrorCode.NotInit; }

            IntPtr intPtr = Marshal.AllocHGlobal(template.Length);
            int num = ZkfpPlus.ZKFPM_ExtractFromImage(dbHandle, FileName, DPI, intPtr, ref size);
            if (num == (int)ErrorCode.OK)
            {
                Marshal.Copy(intPtr, template, 0, size);
            }
            Marshal.FreeHGlobal(intPtr);
            return (ErrorCode)num;
        }
    }
}
