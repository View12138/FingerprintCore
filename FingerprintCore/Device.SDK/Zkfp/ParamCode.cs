
using System.ComponentModel;

namespace FingerprintCore.Device.SDK.Zkfp
{
    /// <summary>
    /// 传入参数类型
    /// </summary>
    public enum ParamCode
    {
        /// <summary>
        /// 图像宽
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("图像宽")]
        ImageWidth = 1,
        /// <summary>
        /// 图像高
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("图像高")]
        ImageHeight = 2,
        /// <summary>
        /// 图像 DPI(儿童建议设置750/1000)
        /// <para>属性：读写(目前只有LIVEID20R可写)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("图像 DPI(儿童建议设置750/1000)")]
        ImageDPI = 3,
        /// <summary>
        /// 图像数据大小
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("图像数据大小")]
        ImageDataSize = 106,
        /// <summary>
        /// VID&amp;PID(前 2 字节VID,后 2 字节PID)
        /// <para>属性：只读</para>
        /// <para>数据类型：byte[4]</para>
        /// </summary>
        [Description("VID&PID(前 2 字节VID,后 2 字节PID)")]
        VIDandPID = 1015,
        /// <summary>
        /// 防假开关(1 打开/0关闭)
        /// <para>属性：读写(目前只有LIVEID20R可写)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("防假开关(1 打开/0关闭)")]
        AntiSpoofingSwitch = 2002,
        /// <summary>
        /// 第五位全为1表示真手指(value&amp;31==31)
        /// <para>属性：只读</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("第五位全为1表示真手指(value&31==31)")]
        TrueFinger = 2004,
        /// <summary>
        /// 厂商信息
        /// <para>属性：只读</para>
        /// <para>数据类型：string</para>
        /// </summary>
        [Description("厂商信息")]
        ManufacturerInformation = 1101,
        /// <summary>
        /// 产品名
        /// <para>属性：只读</para>
        /// <para>数据类型：string</para>
        /// </summary>
        [Description("产品名")]
        ProductName = 1102,
        /// <summary>
        /// 设备序列号
        /// <para>属性：只读</para>
        /// <para>数据类型：string</para>
        /// </summary>
        [Description("设备序列号")]
        SerialNumber = 1103,
        /// <summary>
        /// 1 表示闪白灯;0 表示关闭
        /// <para>属性：只写(非 LIVE20R 需要调用关闭)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("1 表示闪白灯;0 表示关闭")]
        WhiteLight = 101,
        /// <summary>
        /// 1 表示闪绿灯;0 表示关闭
        /// <para>属性：只写(非 LIVE20R 需要调用关闭)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("1 表示闪绿灯;0 表示关闭")]
        GreenLight = 102,
        /// <summary>
        /// 1 表示闪红灯;0 表示关闭
        /// <para>属性：只写(非 LIVE20R 需要调用关闭)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("1 表示闪红灯;0 表示关闭")]
        RedLight = 103,
        /// <summary>
        /// 1 表示开启蜂鸣器;0 表示关闭
        /// <para>属性：只写(LIVE20R 不支持)</para>
        /// <para>数据类型：int</para>
        /// </summary>
        [Description("1 表示开启蜂鸣器;0 表示关闭")]
        OpenBuzzer = 104,
    }
}
