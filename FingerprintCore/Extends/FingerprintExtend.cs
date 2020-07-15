using System;
using System.Threading;
using System.Threading.Tasks;

using FingerprintCore.Device.Zkfp;
using FingerprintCore.Interface;
using FingerprintCore.Models;

namespace FingerprintCore.Extends
{
    /// <summary>
    /// 指纹识别扩展类
    /// </summary>
    public static class FingerprintExtend
    {
        static readonly string[] Describes = new string[]
        {
            "注册指纹需要按下三次手指",
            "还需要按下两次手指",
            "还需要按下一次手指",
            "注册成功",
            "请使用同一手指",
        };

        /// <summary>
        /// 注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <returns></returns>
        public static FingerprintInfo Register(this IFingerprint fingerprint) => fingerprint.Register((str, type) => { });
        /// <summary>
        /// 注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <param name="action">提示方法</param>
        /// <returns></returns>
        public static FingerprintInfo Register(this IFingerprint fingerprint, Action<string, MessageType> action) => fingerprint.Register(action, Describes);
        /// <summary>
        /// 注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <param name="action">提示方法</param>
        /// <param name="describes">提示语句</param>
        /// <returns></returns>
        public static FingerprintInfo Register(this IFingerprint fingerprint, Action<string, MessageType> action, string[] describes)
        {
            if (describes == null)
            { throw new ArgumentNullException(nameof(describes)); }

            string[] _describes = new string[5];
            for (int i = 0; i < _describes.Length; i++)
            {
                if (i < describes.Length)
                { _describes[i] = describes[i]; }
                else
                { _describes[i] = Describes[i]; }
            }

            FingerprintInfo[] _fingerprints = new FingerprintInfo[3];
            action(_describes[0], MessageType.info); // 注册指纹需要按下三次手指
            for (int i = 0; i < 3;)
            {
                FingerprintInfo _fingerprint = fingerprint.GetFingerprint();
                if (_fingerprint != null)
                {
                    if (i == 0)
                    {
                        _fingerprints[i] = _fingerprint;
                        i++;
                    }
                    else if (i > 0)
                    {
                        if (fingerprint.Verify(_fingerprint, _fingerprints[i - 1]))
                        {
                            _fingerprints[i] = _fingerprint;
                            i++;
                        }
                        else
                        {
                            action(_describes[4], MessageType.Error);// 请使用同一手指
                        }
                    }
                }
                if (i < 3)
                {
                    action(_describes[i], MessageType.info);
                    // 还需要按下{3 - i}次手指
                }
            }
            action(_describes[3], MessageType.Success);// 请使用同一手指
            return fingerprint.Merge(_fingerprints);
        }

        /// <summary>
        /// 已异步的方式注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <returns></returns>
        public static async Task<FingerprintInfo> RegisterAsync(this IFingerprint fingerprint)
        {
            return await fingerprint.RegisterAsync((tips, type) => { }, Describes, new CancellationToken(false));
        }
        /// <summary>
        /// 已异步的方式注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static async Task<FingerprintInfo> RegisterAsync(this IFingerprint fingerprint, CancellationToken cancellation)
        {
            return await fingerprint.RegisterAsync((tips, type) => { }, Describes, cancellation);
        }
        /// <summary>
        /// 已异步的方式注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <param name="action">提示方法</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static async Task<FingerprintInfo> RegisterAsync(this IFingerprint fingerprint, Action<string, MessageType> action, CancellationToken cancellation)
        {
            return await fingerprint.RegisterAsync(action, Describes, cancellation);
        }
        /// <summary>
        /// 已异步的方式注册指纹
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <param name="action">提示方法</param>
        /// <param name="describes">提示语句</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static async Task<FingerprintInfo> RegisterAsync(this IFingerprint fingerprint, Action<string, MessageType> action, string[] describes, CancellationToken cancellation)
        {
            if (describes == null)
            { throw new ArgumentNullException(nameof(describes)); }

            string[] _describes = new string[5];
            for (int i = 0; i < _describes.Length; i++)
            {
                if (i < describes.Length)
                { _describes[i] = describes[i]; }
                else
                { _describes[i] = Describes[i]; }
            }

            FingerprintInfo[] _fingerprints = new FingerprintInfo[3];
            action(_describes[0], MessageType.info); // 注册指纹需要按下三次手指
            for (int i = 0; i < 3;)
            {
                FingerprintInfo _fingerprint = await fingerprint.GetFingerprintAsync(cancellation);
                if (cancellation.IsCancellationRequested)
                { return null; }
                if (_fingerprint != null)
                {
                    if (i == 0)
                    {
                        _fingerprints[i] = _fingerprint;
                        i++;
                    }
                    else if (i > 0)
                    {
                        if (fingerprint.Verify(_fingerprint, _fingerprints[i - 1]))
                        {
                            _fingerprints[i] = _fingerprint;
                            i++;
                        }
                        else
                        {
                            action(_describes[4], MessageType.Error);// 请使用同一手指
                        }
                    }
                }
                if (i < 3)
                {
                    action(_describes[i], MessageType.info);
                    // 还需要按下{3 - i}次手指
                }
            }
            action(_describes[3], MessageType.Success);// 请使用同一手指
            return fingerprint.Merge(_fingerprints);
        }

        /// <summary>
        /// 将三枚指纹模板合并成一枚指纹模板
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="fingerprint"></param>
        /// <param name="templates">指纹模板</param>
        /// <returns></returns>
        public static byte[] Merge(this IFingerprint fingerprint, params byte[][] templates)
        {
            FingerprintInfo[] fingerprints = new FingerprintInfo[3];
            fingerprints[0] = new FingerprintInfo(FingerprintType.Template, templates[0]);
            fingerprints[1] = new FingerprintInfo(FingerprintType.Template, templates[1]);
            fingerprints[2] = new FingerprintInfo(FingerprintType.Template, templates[2]);

            return fingerprint.Merge(fingerprints).Template;
        }

        /// <summary>
        /// 验证两枚指纹模板是否相同
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="fingerprint"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Verify(this IFingerprint fingerprint, byte[] left, byte[] right)
        {
            var _left = new FingerprintInfo(FingerprintType.Template, left);
            var _right = new FingerprintInfo(FingerprintType.Template, right);

            return fingerprint.Verify(_left, _right);
        }
    }
}
