using System;
using System.Collections.Generic;
using System.Text;

namespace ZkfpHelp
{
    /// <summary>
    /// 中控指纹参数代码
    /// </summary>
    public class ZkfpParamCode
    {
        /// <summary>
        /// 图像宽
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int ImageWidth = 1;
        /// <summary>
        /// 图像高
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int ImageHeight = 2;
        /// <summary>
        /// 图像 DPI(儿童建议设置750/1000)
        /// <para>属性：读写(目前只有LIVEID20R可写)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int ImageDPI = 3;
        /// <summary>
        /// 图像数据大小
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int ImageDataSize = 106;
        /// <summary>
        /// VID&amp;PID(前 2 字节VID,后 2 字节PID)
        /// <para>属性：只读</para>
        /// <para>数据类型：byte[4]</para>
        /// </summary>
        public const int VIDandPID = 1015;
        /// <summary>
        /// 防假开关(1 打开/0关闭)
        /// <para>属性：读写(目前只有LIVEID20R可写)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int AntiSpoofingSwitch = 2002;
        /// <summary>
        /// 第五位全为1表示真手指(value&amp;31==31)
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int TrueFinger = 2004;
        /// <summary>
        /// 厂商信息
        /// <para>属性：只读</para>
        /// <para>数据类型：string</para>
        /// </summary>
        public const int ManufacturerInformation = 1101;
        /// <summary>
        /// 产品名
        /// <para>属性：只读</para>
        /// <para>数据类型：string</para>
        /// </summary>
        public const int ProductName = 1102;
        /// <summary>
        /// 设备序列号
        /// <para>属性：只读</para>
        /// <para>数据类型：string</para>
        /// </summary>
        public const int SerialNumber = 1103;
        /// <summary>
        /// 1 表示闪白灯;0 表示关闭
        /// <para>属性：只写(非 LIVE20R 需要调用关闭)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int WhiteLight = 101;
        /// <summary>
        /// 1 表示闪绿灯;0 表示关闭
        /// <para>属性：只写(非 LIVE20R 需要调用关闭)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int GreenLight = 102;
        /// <summary>
        /// 1 表示闪红灯;0 表示关闭
        /// <para>属性：只写(非 LIVE20R 需要调用关闭)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int RedLight = 103;
        /// <summary>
        /// 1 表示开启蜂鸣器;0 表示关闭
        /// <para>属性：只写(LIVE20R 不支持)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        public const int OpenBuzzer = 104;
    }
}
