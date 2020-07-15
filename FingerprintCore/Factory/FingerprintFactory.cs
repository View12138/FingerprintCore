using FingerprintCore.Device;
using FingerprintCore.Device.Zkfp;
using FingerprintCore.Interface;

using System;
using System.Collections.Generic;
using System.Text;

namespace FingerprintCore.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class FingerprintFactory
    {
        /// <summary>
        /// 获取指定厂家的指纹一操作类
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IFingerprint CreateFingerprint(Devices device)
        {
            switch (device)
            {
                case Devices.ZKFP: return new ZKFingerprint();
                default: throw new ArgumentException("没有此设备");
            }
        }
    }
}
