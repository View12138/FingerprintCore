using FingerprintCore.Interface;

namespace FingerprintCore.Device.Zkfp
{
    /// <summary>
    /// 指纹仪的 LED 灯状态
    /// </summary>
    public enum LightType : int
    {
        /// <summary>
        /// 闪烁白灯
        /// </summary>
        White = 101,
        /// <summary>
        /// 闪烁绿灯
        /// </summary>
        Green = 102,
        /// <summary>
        /// 闪烁红灯
        /// </summary>
        Red = 103,
    }
}
