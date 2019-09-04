using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ZkfpHelp
{

    /// <summary>
    /// 中控指纹仪操作类
    /// <para>可异步注册指纹、异步验证已有指纹。</para>
    /// </summary>
    public class ZkfpTool
    {
        // 属性
        /// <summary>
        /// 获取或设置当前用户账户中保存的指纹模板(base64)
        /// </summary>
        public string Fingerprint { get; set; } = string.Empty;
        /// <summary>
        /// 获取用户最新一次验证成功时的指纹图片
        /// </summary>
        public MemoryStream FingrtImage { get; private set; }

        // 字段
        private IntPtr _device = IntPtr.Zero;
        private IntPtr _db = IntPtr.Zero;
        private int _imageWidth = 0;
        private int _imageHeight = 0;

        // 事件 & 委托
        /// <summary>
        /// 注册指纹时有提示时发生
        /// </summary>
        public event StringHandler RegisterEvent;
        /// <summary>
        /// 表示注册指纹时的提示方法
        /// </summary>
        /// <param name="tips">提示内容</param>
        /// <param name="tipsBrush">提示文本的颜色</param>
        public delegate void RegisterTipsHandler(string tips, ColorInfo tipsBrush);
        /// <summary>
        /// 表示将用于处理具有字符串数据的事件方法
        /// </summary>
        /// <param name="message"></param>
        public delegate void StringHandler(string message);

        /// <summary>
        /// 中控指纹仪器的帮助类
        /// </summary>
        public ZkfpTool()
        {
        }

        ~ZkfpTool()
        {
            Dispose();
        }

        // 公共方法
        /// <summary>
        /// 初始化并连接第一个设备
        /// </summary>
        public bool Init()
        {
            Dispose();
            if (ZkfpSDK.Init() == ZkfpErrorCode.ZKFP_ERR_OK)
            {
                if (ZkfpSDK.GetDeviceCount() >= 1)
                {
                    if ((_device = ZkfpSDK.OpenDevice(0)) != IntPtr.Zero)
                    {
                        if ((_db = ZkfpSDK.DBInit()) != IntPtr.Zero)
                        {

                            byte[] paramValue = new byte[4];
                            int size = 4;
                            ZkfpSDK.GetParameters(_device, ZkfpParamCode.ImageWidth, paramValue, ref size);
                            ZkfpSDK.ByteArray2Int(paramValue, ref _imageWidth);

                            size = 4;
                            ZkfpSDK.GetParameters(_device, ZkfpParamCode.ImageHeight, paramValue, ref size);
                            ZkfpSDK.ByteArray2Int(paramValue, ref _imageHeight);

                            return true;
                        }
                        ZkfpSDK.CloseDevice(_device);
                    }
                }
            }
            {
                ZkfpSDK.Terminate();
                return false;
            }
        }
        /// <summary>
        /// 初始化并连接指定的设备
        /// </summary>
        public bool Init(int deviceIndex)
        {
            Dispose();
            if (ZkfpSDK.Init() == ZkfpErrorCode.ZKFP_ERR_OK)
            {
                int count = ZkfpSDK.GetDeviceCount();
                if (count > 0 && deviceIndex < count)
                {
                    if ((_device = ZkfpSDK.OpenDevice(deviceIndex)) != IntPtr.Zero)
                    {
                        if ((_db = ZkfpSDK.DBInit()) != IntPtr.Zero)
                        {

                            byte[] paramValue = new byte[4];
                            int size = 4;
                            ZkfpSDK.GetParameters(_device, ZkfpParamCode.ImageWidth, paramValue, ref size);
                            ZkfpSDK.ByteArray2Int(paramValue, ref _imageWidth);

                            size = 4;
                            ZkfpSDK.GetParameters(_device, ZkfpParamCode.ImageHeight, paramValue, ref size);
                            ZkfpSDK.ByteArray2Int(paramValue, ref _imageHeight);

                            return true;
                        }
                        ZkfpSDK.CloseDevice(_device);
                    }
                }
            }
            {
                ZkfpSDK.Terminate();
                return false;
            }
        }
        /// <summary>
        /// 释放使用的资源
        /// </summary>
        /// <returns></returns>
        public bool Dispose()
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return false; }
            ZkfpSDK.DBClear(_db);
            ZkfpSDK.DBFree(_db);
            ZkfpSDK.CloseDevice(_device);
            ZkfpSDK.Terminate();
            return true;
        }

        /// <summary>
        /// 以同步的方式验证当前用户是否为本人
        /// <para>调用此方法之前对 <see cref="Fingerprint"/> 属性赋值</para>
        /// </summary>
        /// <returns></returns>
        public bool Identify()
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return false; }
            if (Fingerprint == string.Empty) { return false; }
            byte[] imagebuffer = new byte[_imageWidth * _imageHeight];
            int size = 2048;
            byte[] template = new byte[size];
            int code;
            do
            {
                code = ZkfpSDK.AcquireFingerprint(_device, imagebuffer, template, ref size);
                Thread.Sleep(200);
            }
            while (code == ZkfpErrorCode.ZKFP_ERR_CAPTURE);
            if (code == ZkfpErrorCode.ZKFP_ERR_OK)
            {
                byte[] tempAccount = ZkfpSDK.Base64ToBlob(Fingerprint);
                int socre = ZkfpSDK.DBMatch(_db, template, tempAccount);
                if (socre > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    GetBitmap(imagebuffer, _imageWidth, _imageHeight, ref ms);
                    FingrtImage = ms;

                    SetValue(ZkfpParamCode.GreenLight, 1);
                    return true;
                }
            }
            SetValue(ZkfpParamCode.RedLight, 1);
            return false;
        }
        /// <summary>
        /// 以异步的方式验证当前用户是否为本人。
        /// <para>验证成功返回 <see langword="true"/> ，验证失败返回<see  langword="false"/>。</para>
        /// <para>可以通过 <see cref="Fingerprint"/> 属性获取操作成功之后的指纹模板字符串。</para>
        /// <para>可以通过 <see cref="FingrtImage"/> 属性获取操作成功之后的指纹图像。</para>
        /// <para></para>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IdentifyAsync()
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return false; }
            if (Fingerprint == string.Empty) { return false; }
            byte[] imagebuffer = new byte[_imageWidth * _imageHeight];
            int size = 2048;
            byte[] template = new byte[size];
            int code = await ZkfpSDK.AcquireFingerprintAsync(_device, imagebuffer, template);
            if (code > 0)
            {
                byte[] tempAccount = ZkfpSDK.Base64ToBlob(Fingerprint);
                int socre = ZkfpSDK.DBMatch(_db, template, tempAccount);
                if (socre > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    GetBitmap(imagebuffer, _imageWidth, _imageHeight, ref ms);
                    FingrtImage = ms;

                    SetValue(ZkfpParamCode.GreenLight, 1);
                    return true;
                }
            }
            FingrtImage = null;
            SetValue(ZkfpParamCode.RedLight, 1);
            return false;
        }

        /// <summary>
        /// 以同步的方式注册当前用户的指纹信息，并保存到 <see cref="Fingerprint"/> 中。
        /// <para>若需要注册时的提示文本，请注册 <see cref="RegisterEvent"/> 事件。</para>
        /// </summary>
        /// <returns></returns>
        public bool Register()
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return false; }
            RegisterEvent?.Invoke("注册指纹时需要按下同一根手指三次！");
            byte[][] templates = new byte[3][];
            for (int i = 0; i < 3;)
            {
                byte[] imagebuffer = new byte[_imageWidth * _imageHeight];
                int size = 2048;
                byte[] template = new byte[size];
                int code = ZkfpSDK.AcquireFingerprint(_device, imagebuffer, template, ref size);
                if (code == ZkfpErrorCode.ZKFP_ERR_OK)
                {
                    MemoryStream ms = new MemoryStream();
                    GetBitmap(imagebuffer, _imageWidth, _imageHeight, ref ms);
                    FingrtImage = ms;

                    templates[i] = new byte[size];
                    Array.Copy(template, templates[i], size);
                    if (i > 0)
                    {
                        int socre = ZkfpSDK.DBMatch(_db, template, templates[i - 1]);
                        if (socre <= 0)
                        {
                            RegisterEvent?.Invoke("请使用同一根手指！");
                            continue;
                        }
                    }
                    if (i >= 2)
                    {
                        int regSize = 2048;
                        byte[] regTemplate = new byte[size];
                        code = ZkfpSDK.DBMerge(_db, templates[0], templates[1], templates[2], regTemplate, ref regSize);
                        if (code == ZkfpErrorCode.ZKFP_ERR_OK)
                        {
                            Fingerprint = ZkfpSDK.BlobToBase64(regTemplate, regSize);
                            SetValue(ZkfpParamCode.GreenLight, 1);
                            RegisterEvent?.Invoke("当前指纹注册成功！");
                            return true;
                        }
                        else
                        {
                            Fingerprint = null;
                            SetValue(ZkfpParamCode.RedLight, 1);
                            RegisterEvent?.Invoke("当前指纹注册失败！请重试。");
                            return false;
                        }
                    }
                    else { RegisterEvent?.Invoke($"还需按下同一根手指{2 - i}次！"); }
                    i += 1;
                }
            }
            return true;
        }
        /// <summary>
        /// 以异步的方式注册指纹。
        /// <para>注册成功返回 <see  langword="true"/>，注册失败返回 <see langword="false"/>。</para>
        /// <para>可以通过 <see cref="Fingerprint"/> 属性获取操作成功之后的指纹模板字符串。</para>
        /// <para>可以通过 <see cref="FingrtImage"/> 属性获取操作成功之后的指纹图像。</para>
        /// </summary>
        /// <param name="RegisterTips">若需要注册时的提示文本，请传入提示文本处理方法。</param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(RegisterTipsHandler RegisterTips = null)
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return false; }
            RegisterTips?.Invoke("注册指纹时需要按三下手指！", ColorInfo.Success);
            byte[][] templates = new byte[3][];
            for (int i = 0; i < 3;)
            {
                byte[] imagebuffer = new byte[_imageWidth * _imageHeight];
                int size = 2048;
                int getSize = 0;
                byte[] template = new byte[size];
                int code = await ZkfpSDK.AcquireFingerprintAsync(_device, imagebuffer, template);
                if (code > 0)
                {
                    getSize = code;
                    MemoryStream ms = new MemoryStream();
                    GetBitmap(imagebuffer, _imageWidth, _imageHeight, ref ms);
                    FingrtImage = ms;

                    templates[i] = new byte[size];
                    Array.Copy(template, templates[i], getSize);
                    if (i > 0)
                    {
                        int socre = ZkfpSDK.DBMatch(_db, template, templates[i - 1]);
                        if (socre <= 0)
                        {
                            SetValue(ZkfpParamCode.RedLight, 1);
                            RegisterTips?.Invoke("请使用同一根手指！", ColorInfo.Fail);
                            continue;
                        }
                    }
                    if (i >= 2)
                    {
                        int regSize = 2048;
                        byte[] regTemplate = new byte[size];
                        code = ZkfpSDK.DBMerge(_db, templates[0], templates[1], templates[2], regTemplate, ref regSize);
                        if (code == ZkfpErrorCode.ZKFP_ERR_OK)
                        {
                            string s = ZkfpSDK.BlobToBase64(regTemplate, regSize);
                            SetValue(ZkfpParamCode.GreenLight, 1);
                            RegisterTips?.Invoke("当前指纹注册成功！", ColorInfo.Success);
                            Fingerprint = s;
                            return true;
                        }
                        else
                        {
                            SetValue(ZkfpParamCode.RedLight, 1);
                            RegisterTips?.Invoke("当前指纹注册失败！请重试。", ColorInfo.Fail);
                            return false;
                        }
                    }
                    else
                    {
                        string str = "三";
                        if (i == 0) str = "两";
                        if (i == 1) str = "一";
                        if (i >= 2) continue;
                        RegisterTips?.Invoke($"还需按下同一根手指{str}次！", ColorInfo.Success);
                    }
                    i += 1;
                }
            }
            return false;
        }

        // 私有方法
        #region 私有方法
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="code">参数代码</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        private bool SetValue(int code, int value)
        {
            if (_device == IntPtr.Zero || _db == IntPtr.Zero) { return false; }
            int size = 4;
            byte[] paramValue = new byte[size];
            if (ZkfpSDK.Int2ByteArray(value, paramValue))
            {
                int error = ZkfpSDK.SetParameters(_device, code, paramValue, size);
                if (error == 0) { return true; }
            }
            return false;
        }

        #region 指纹图像相关方法
        /// <summary>
        /// 旋转图片，目的是保存和显示的图片与按的指纹方向不同
        /// </summary>
        /// <param name="BmpBuf">旋转前的指纹字符串</param>
        /// <param name="width">旋转后的指纹字符串</param>
        /// <param name="height"></param>
        /// <param name="ResBuf"></param>
        private static void RotatePic(byte[] BmpBuf, int width, int height, ref byte[] ResBuf)
        {
            int RowLoop = 0;
            int ColLoop = 0;
            int BmpBuflen = width * height;

            try
            {
                for (RowLoop = 0; RowLoop < BmpBuflen;)
                {
                    for (ColLoop = 0; ColLoop < width; ColLoop++)
                    {
                        ResBuf[RowLoop + ColLoop] = BmpBuf[BmpBuflen - RowLoop - width + ColLoop];
                    }

                    RowLoop = RowLoop + width;
                }
            }
            catch
            {
                //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                //logger.Append();
            }
        }

        /// <summary>
        /// 将结构体转化成无符号字符串数组
        /// </summary>
        /// <param name="StructObj">被转化的结构体</param>
        /// <param name="Size">被转化的结构体的大小</param>
        /// <returns></returns>
        private static byte[] StructToBytes(object StructObj, int Size)
        {
            int StructSize = Marshal.SizeOf(StructObj);
            byte[] GetBytes = new byte[StructSize];

            try
            {
                IntPtr StructPtr = Marshal.AllocHGlobal(StructSize);
                Marshal.StructureToPtr(StructObj, StructPtr, false);
                Marshal.Copy(StructPtr, GetBytes, 0, StructSize);
                Marshal.FreeHGlobal(StructPtr);

                if (Size == 14)
                {
                    byte[] NewBytes = new byte[Size];
                    int Count = 0;
                    int Loop = 0;

                    for (Loop = 0; Loop < StructSize; Loop++)
                    {
                        if (Loop != 2 && Loop != 3)
                        {
                            NewBytes[Count] = GetBytes[Loop];
                            Count++;
                        }
                    }

                    return NewBytes;
                }
                else
                {
                    return GetBytes;
                }
            }
            catch
            {
                //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                //logger.Append();

                return GetBytes;
            }
        }

        /// <summary>
        /// 将指纹图像 <see cref="byte"/>[] 转换为 <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="ms"></param>
        private static void GetBitmap(byte[] buffer, int nWidth, int nHeight, ref MemoryStream ms)
        {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight * 2];

            try
            {
                BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
                BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
                MASK[] ColorMask = new MASK[m_nColorTableEntries];

                int w = (((nWidth + 3) / 4) * 4);

                //图片头信息
                BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
                BmpInfoHeader.biWidth = nWidth;
                BmpInfoHeader.biHeight = nHeight;
                BmpInfoHeader.biPlanes = 1;
                BmpInfoHeader.biBitCount = m_nBitCount;
                BmpInfoHeader.biCompression = 0;
                BmpInfoHeader.biSizeImage = 0;
                BmpInfoHeader.biXPelsPerMeter = 0;
                BmpInfoHeader.biYPelsPerMeter = 0;
                BmpInfoHeader.biClrUsed = m_nColorTableEntries;
                BmpInfoHeader.biClrImportant = m_nColorTableEntries;

                //文件头信息
                BmpHeader.bfType = 0x4D42;
                BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
                BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
                BmpHeader.bfReserved1 = 0;
                BmpHeader.bfReserved2 = 0;

                ms.Write(StructToBytes(BmpHeader, 14), 0, 14);
                ms.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)), 0, Marshal.SizeOf(BmpInfoHeader));

                //调试板信息
                for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
                {
                    ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                    ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                    ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                    ColorMask[ColorIndex].rgbReserved = 0;

                    ms.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])), 0, Marshal.SizeOf(ColorMask[ColorIndex]));
                }

                //图片旋转，解决指纹图片倒立的问题
                RotatePic(buffer, nWidth, nHeight, ref ResBuf);

                byte[] filter = null;
                if (w - nWidth > 0)
                {
                    filter = new byte[w - nWidth];
                }
                for (int i = 0; i < nHeight; i++)
                {
                    ms.Write(ResBuf, i * nWidth, nWidth);
                    if (w - nWidth > 0)
                    {
                        ms.Write(ResBuf, 0, w - nWidth);
                    }
                }
            }
            catch
            {
                // ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                // logger.Append();
            }
        }

        private struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public int bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public int bfOffBits;
        }

        private struct MASK
        {
            public byte redmask;
            public byte greenmask;
            public byte bluemask;
            public byte rgbReserved;
        }

        private struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 颜色信息
    /// </summary>
    public enum ColorInfo
    {
        /// <summary>
        /// 成功的颜色
        /// </summary>
        Success = 0,
        /// <summary>
        /// 失败的颜色
        /// </summary>
        Fail = 1,
    }
}
