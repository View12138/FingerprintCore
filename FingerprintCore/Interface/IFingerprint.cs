using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FingerprintCore.Models;

namespace FingerprintCore.Interface
{
    /// <summary>
    /// 指纹仪操作类
    /// </summary>
    public interface IFingerprint
    {
        /// <summary>
        /// 获取一个值，指示是否初始化完成
        /// </summary>
        bool IsInit { get; }

        /// <summary>
        /// 初始化并打开设备
        /// </summary>
        /// <returns></returns>
        bool Initialization();
        /// <summary>
        /// 初始化并打开指定的设备
        /// </summary>
        /// <param name="deviceIndex"></param>
        /// <returns></returns>
        bool Initialization(int deviceIndex);

        /// <summary>
        /// 获取指纹
        /// </summary>
        /// <returns></returns>
        FingerprintInfo GetFingerprint();

        /// <summary>
        /// 已异步的方式获取指纹
        /// </summary>
        /// <returns></returns>
        Task<FingerprintInfo> GetFingerprintAsync();

        /// <summary>
        /// 已异步的方式获取指纹，可取消
        /// </summary>
        /// <returns></returns>
        Task<FingerprintInfo> GetFingerprintAsync(CancellationToken cancellation);

        /// <summary>
        /// 将三枚指纹模板合并成一枚指纹模板
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="fingerprints">具有指纹模板的指纹信息</param>
        /// <returns></returns>
        FingerprintInfo Merge(params FingerprintInfo[] fingerprints);

        /// <summary>
        /// 验证两枚指纹模板是否相同
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool Verify(FingerprintInfo left, FingerprintInfo right);

        /// <summary>
        /// 使指纹仪 LED 灯闪烁
        /// </summary>
        /// <param name="type">指定的颜色，使用各指纹仪规定的LED枚举</param>
        /// <returns></returns>
        bool SetLight(Enum type);
    }
}
