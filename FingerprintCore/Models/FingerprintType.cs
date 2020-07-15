namespace FingerprintCore.Models
{
    /// <summary>
    /// 指纹信息类型
    /// </summary>
    public enum FingerprintType
    {
        /// <summary>
        /// 指纹模板
        /// </summary>
        Template = 0,
        /// <summary>
        /// 指纹图像
        /// </summary>
        Image,
        /// <summary>
        /// 两者兼有
        /// </summary>
        Both,
    }
}
