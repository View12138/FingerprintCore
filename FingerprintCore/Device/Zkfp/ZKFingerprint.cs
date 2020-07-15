using System;
using System.Threading;
using System.Threading.Tasks;

using FingerprintCore.Device.SDK.Zkfp;
using FingerprintCore.Interface;
using FingerprintCore.Models;

namespace FingerprintCore.Device.Zkfp
{
    /// <summary>
    /// 中控指纹仪操作类
    /// </summary>
    class ZKFingerprint : IFingerprint
    {
        // Property
        /// <summary>
        /// 获取一个值，指示是否初始化完成
        /// </summary>
        public bool IsInit { get => _isInit; }

        // Field
        private IntPtr _device = IntPtr.Zero;
        private IntPtr _db = IntPtr.Zero;
        private int _imageWidth = 0;
        private int _imageHeight = 0;
        private int _fingerprintSize = 2048;
        private int _waitTime = 200;
        private bool _isInit = false;

        // Constructor
        /// <summary>
        /// 析构
        /// </summary>
        ~ZKFingerprint()
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return; }
            ZkfpSharp.DBClear(_db);
            ZkfpSharp.DBFree(_db);
            ZkfpSharp.CloseDevice(ref _device);
            ZkfpSharp.Terminate();
        }

        // Method
        /// <summary>
        /// 初始化中控指纹仪，并打开第一个设备
        /// </summary>
        /// <returns></returns>
        public bool Initialization() => Initialization(0);
        /// <summary>
        /// 初始化中控指纹仪
        /// </summary>
        /// <param name="deviceIndex"></param>
        /// <returns></returns>
        public bool Initialization(int deviceIndex)
        {
            if (ZkfpSharp.Init() == ErrorCode.OK)
            {
                int count = ZkfpSharp.GetDeviceCount();
                if (count > 0 && deviceIndex < count)
                {
                    if ((_device = ZkfpSharp.OpenDevice(deviceIndex)) != IntPtr.Zero)
                    {
                        if ((_db = ZkfpSharp.DBInit()) != IntPtr.Zero)
                        {
                            byte[] paramValue = new byte[4];

                            ZkfpSharp.GetParameters(_device, ParamCode.ImageWidth, paramValue);
                            _imageWidth = ZkfpSharp.ByteArray2Int(paramValue);

                            ZkfpSharp.GetParameters(_device, ParamCode.ImageHeight, paramValue);
                            _imageHeight = ZkfpSharp.ByteArray2Int(paramValue);

                            _isInit = true;
                            return IsInit;
                        }
                        ZkfpSharp.DBFree(_db);
                    }
                    ZkfpSharp.CloseDevice(ref _device);
                }
            }
            ZkfpSharp.Terminate();
            return IsInit;
        }

        /// <summary>
        /// 获取指纹
        /// </summary>
        /// <returns></returns>
        public FingerprintInfo GetFingerprint()
        {
            byte[] template = new byte[_fingerprintSize];
            byte[] image = new byte[_imageWidth * _imageHeight];

            ErrorCode code;
            do
            {
                code = ZkfpSharp.AcquireFingerprint(_device, image, template);
                Thread.Sleep(_waitTime);
            }
            while (code == ErrorCode.Capture);

            if (code == ErrorCode.OK)
            { return new FingerprintInfo(FingerprintType.Both, image, template); }
            else { return null; }
        }

        /// <summary>
        /// 已异步的方式获取指纹
        /// </summary>
        /// <returns></returns>
        public async Task<FingerprintInfo> GetFingerprintAsync() => await GetFingerprintAsync(new CancellationToken(false));
        /// <summary>
        /// 已异步的方式获取指纹，可取消
        /// </summary>
        /// <returns></returns>
        public async Task<FingerprintInfo> GetFingerprintAsync(CancellationToken cancellation)
        {
            byte[] template = new byte[_fingerprintSize];
            byte[] image = new byte[_imageWidth * _imageHeight];

            ErrorCode code = await Task.Run(() =>
            {
                do
                {
                    code = ZkfpSharp.AcquireFingerprint(_device, image, template);
                    Thread.Sleep(_waitTime);
                }
                while (code == ErrorCode.Capture && !cancellation.IsCancellationRequested);
                return code;
            });

            if (code == ErrorCode.OK)
            { return new FingerprintInfo(FingerprintType.Both, image, template); }
            else { return null; }
        }

        /// <summary>
        /// 将三枚指纹模板合并成一枚指纹模板
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="fingerprints">具有指纹模板的指纹信息</param>
        /// <returns></returns>
        public FingerprintInfo Merge(params FingerprintInfo[] fingerprints)
        {
            if (fingerprints == null || fingerprints.Length != 3)
            { throw new ArgumentNullException(nameof(fingerprints)); }
            foreach (var fingerprint in fingerprints)
            {
                if (fingerprint.Template == null)
                { throw new ArgumentNullException(nameof(fingerprints)); }
            }

            byte[] regTemplate = new byte[_fingerprintSize];
            var code = ZkfpSharp.DBMerge(_db, fingerprints[0].Template, fingerprints[1].Template, fingerprints[2].Template, regTemplate);
            if (code == ErrorCode.OK)
            { return new FingerprintInfo(FingerprintType.Template, regTemplate); }
            else { return null; }
        }

        /// <summary>
        /// 验证两枚指纹模板是否相同
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool Verify(FingerprintInfo left, FingerprintInfo right)
        {
            if (left == null || left.Template == null)
            { throw new ArgumentNullException(nameof(left)); }
            if (right == null || right.Template == null)
            { throw new ArgumentNullException(nameof(right)); }

            int socre = ZkfpSharp.DBMatch(_db, left.Template, right.Template);
            return socre > 0;
        }

        /// <summary>
        /// 使指纹仪 LED 灯闪烁
        /// </summary>
        /// <param name="type">指定的颜色 <see cref="LightType"/></param>
        /// <returns></returns>
        public bool SetLight(Enum type)
        {
            LightType _type = (LightType)type;
            byte[] paramValue = ZkfpSharp.Int2ByteArray(1);
            var error = ZkfpSharp.SetParameters(_device, (ParamCode)_type, paramValue);
            if (error == ErrorCode.OK) { return true; }
            else { return false; }
        }
    }
}
