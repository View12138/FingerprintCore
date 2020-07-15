using System;

namespace FingerprintCore.Models
{
    /// <summary>
    /// 指纹信息
    /// </summary>
    public class FingerprintInfo
    {
        // Field
        private byte[] _image = null;
        private byte[] _template = null;

        // Property
        /// <summary>
        /// 指纹图像
        /// </summary>
        public byte[] Image { get => _image; }
        /// <summary>
        /// 指纹模板
        /// </summary>
        public byte[] Template { get => _template; }

        // Constructor
        /// <summary>
        /// 使用指纹模板和指纹图片初始化指纹信息
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="type">初始化的指纹类型</param>
        /// <param name="datas">如果 <c><paramref name="type"/> == <see cref="FingerprintType.Both"/></c>，则第一个参数为 <see cref="Image"/>，第二个参数为 <see cref="Template"/></param>
        public FingerprintInfo(FingerprintType type, params byte[][] datas)
        {
            if (datas == null || datas.Length == 0)
            { throw new ArgumentNullException(nameof(datas)); }
            switch (type)
            {
                case FingerprintType.Image:
                    _image = datas[0];
                    break;
                case FingerprintType.Template:
                    _template = datas[0];
                    break;
                case FingerprintType.Both:
                    _image = datas[0];
                    _template = datas[1];
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
        /// <summary>
        /// 使用指纹模板和指纹图片初始化指纹信息
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="type">初始化的指纹类型</param>
        /// <param name="datas">如果 <c><paramref name="type"/> == <see cref="FingerprintType.Both"/></c>，则第一个参数为 <see cref="Image"/>，第二个参数为 <see cref="Template"/></param>
        public FingerprintInfo(FingerprintType type, params string[] datas)
        {
            if (datas == null || datas.Length == 0)
            { throw new ArgumentNullException(nameof(datas)); }
            switch (type)
            {
                case FingerprintType.Image:
                    _image = Convert.FromBase64String(datas[0]);
                    break;
                case FingerprintType.Template:
                    _template = Convert.FromBase64String(datas[0]);
                    break;
                case FingerprintType.Both:
                    _image = Convert.FromBase64String(datas[0]);
                    _template = Convert.FromBase64String(datas[1]);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        // Destructor
        /// <summary>
        /// 析构
        /// </summary>
        ~FingerprintInfo()
        {
            _image = null;
            _template = null;
        }

        // Method
        /// <summary>
        /// 返回 指纹模板的 base64 字符串
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <returns></returns>
        public override string ToString() => ToString(FingerprintType.Template);
        /// <summary>
        /// 返回指定的数据的 base64 字符串
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="type">指定的类型，Both 等于 Template</param>
        /// <returns></returns>
        public string ToString(FingerprintType type)
        {
            switch (type)
            {
                default:
                case FingerprintType.Template:
                    if (Template == null)
                    { throw new ArgumentNullException(nameof(Template)); }
                    return Convert.ToBase64String(Template);
                case FingerprintType.Image:
                    if (Image == null)
                    { throw new ArgumentNullException(nameof(Image)); }
                    return Convert.ToBase64String(Image);
            }
        }
        /// <summary>
        /// 返回指纹图像、指纹模板的 base64 字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <returns>第一个为 指纹图像，第二个为指纹模板</returns>
        public string[] ToStrings()
        {
            if (Image == null)
            { throw new ArgumentNullException(nameof(Image)); }
            if (Template == null)
            { throw new ArgumentNullException(nameof(Template)); }

            return new string[]
            {
                Convert.ToBase64String(Image),
                Convert.ToBase64String(Template),
            };
        }
    }
}
