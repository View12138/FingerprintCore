using System.ComponentModel;

namespace FingerprintCore.Device.SDK.Zkfp
{
    /// <summary>
    /// 错误码
    /// </summary>    
    public enum ErrorCode
    {
        /// <summary>
        /// 已经初始化
        /// </summary>
        [Description("已经初始化")]
        AlreadyInit = 1,
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        OK = 0,
        /// <summary>
        /// 初始化算法库失败
        /// </summary>
        [Description("初始化算法库失败")]
        InitLib = -1,
        /// <summary>
        ///  初始化采集库失败
        /// </summary>
        [Description("初始化采集库失败")]
        Init = -2,
        /// <summary>
        /// 无设备连接
        /// </summary>
        [Description("无设备连接")]
        NoDevice = -3,
        /// <summary>
        ///  接口暂不支持
        /// </summary>
        [Description("接口暂不支持")]
        NotSupport = -4,
        /// <summary>
        /// 无效参数
        /// </summary>
        [Description("无效参数")]
        InvalidParam = -5,
        /// <summary>
        /// 打开设备失败 
        /// </summary>
        [Description("打开设备失败")]
        Open = -6,
        /// <summary>
        /// 无效句柄
        /// </summary>
        [Description("无效句柄")]
        InvalidHandle = -7,
        /// <summary>
        ///  取像失败
        /// </summary>
        [Description("取像失败")]
        Capture = -8,
        /// <summary>
        /// 提取指纹模板失败
        /// </summary>
        [Description("提取指纹模板失败")]
        ExtractFP = -9,
        /// <summary>
        /// 中断
        /// </summary>
        [Description("中断")]
        Absort = -10,
        /// <summary>
        /// 内存不足 
        /// </summary>
        [Description("内存不足")]
        MemoryNotEnough = -11,
        /// <summary>
        /// 当前正在采集
        /// </summary>
        [Description("当前正在采集")]
        Busy = -12,
        /// <summary>
        /// 添加指纹模板失败
        /// </summary>
        [Description("添加指纹模板失败")]
        AddFinger = -13,
        /// <summary>
        /// 删除指纹失败
        /// </summary>
        [Description("删除指纹失败")]
        DeleteFinger = -14,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Fail = -17,
        /// <summary>
        /// 取消采集
        /// </summary>
        [Description("取消采集")]
        Cancel = -18,
        /// <summary>
        /// 比对指纹失败
        /// </summary>
        [Description("比对指纹失败")]
        VerifyFP = -20,
        /// <summary>
        /// 合并登记指纹模板失败
        /// </summary>
        [Description("合并登记指纹模板失败")]
        Merge = -22,
        /// <summary>
        /// 设备未打开
        /// </summary>
        [Description("设备未打开")]
        NotOpened = -23,
        /// <summary>
        /// 未初始化
        /// </summary>
        [Description("未初始化")]
        NotInit = -24,
        /// <summary>
        /// 设备已打开
        /// </summary>
        [Description("设备已打开")]
        AlreadyOpened = -25,
        /// <summary>
        /// [LoadImage]
        /// </summary>
        [Description("[LoadImage]")]
        LoadImage = -26,
        /// <summary>
        /// [AnalyseImage]
        /// </summary>
        [Description("[AnalyseImage]")]
        AnalyseImage = -27,
        /// <summary>
        /// 超时
        /// </summary>
        [Description("超时")]
        Timeout = -28,
    }
}
