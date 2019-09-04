using System;
using System.Collections.Generic;
using System.Text;

namespace ZkfpHelp
{

    /// <summary>
    /// 中控指纹错误代码
    /// </summary>
    public class ZkfpErrorCode
    {
        /// <summary>
        ///  已经初始化
        /// </summary>
        public const int ZKFP_ERR_ALREADY_INIT = 1;
        /// <summary>
        /// 操作成功
        /// </summary>
        public const int ZKFP_ERR_OK = 0;
        /// <summary>
        /// 初始化算法库失败
        /// </summary>
        public const int ZKFP_ERR_INITLIB = -1;
        /// <summary>
        ///  初始化采集库失败
        /// </summary>
        public const int ZKFP_ERR_INIT = -2;
        /// <summary>
        /// 无设备连接
        /// </summary>
        public const int ZKFP_ERR_NO_DEVICE = -3;
        /// <summary>
        ///  接口暂不支持
        /// </summary>
        public const int ZKFP_ERR_NOT_SUPPORT = -4;
        /// <summary>
        /// 无效参数
        /// </summary>
        public const int ZKFP_ERR_INVALID_PARAM = -5;
        /// <summary>
        /// 打开设备失败 
        /// </summary>
        public const int ZKFP_ERR_OPEN = -6;
        /// <summary>
        /// 无效句柄
        /// </summary>
        public const int ZKFP_ERR_INVALID_HANDLE = -7;
        /// <summary>
        ///  取像失败
        /// </summary>
        public const int ZKFP_ERR_CAPTURE = -8;
        /// <summary>
        /// 提取指纹模板失败
        /// </summary>
        public const int ZKFP_ERR_EXTRACT_FP = -9;
        /// <summary>
        /// 中断
        /// </summary>
        public const int ZKFP_ERR_ABSORT = -10;
        /// <summary>
        /// 内存不足 
        /// </summary>
        public const int ZKFP_ERR_MEMORY_NOT_ENOUGH = -11;
        /// <summary>
        /// 当前正在采集
        /// </summary>
        public const int ZKFP_ERR_BUSY = -12;
        /// <summary>
        /// 添加指纹模板失败
        /// </summary>
        public const int ZKFP_ERR_ADD_FINGER = -13;
        /// <summary>
        /// 删除指纹失败
        /// </summary>
        public const int ZKFP_ERR_DEL_FINGER = -14;
        /// <summary>
        /// 操作失败
        /// </summary>
        public const int ZKFP_ERR_FAIL = -17;
        /// <summary>
        /// 取消采集
        /// </summary>
        public const int ZKFP_ERR_CANCEL = -18;
        /// <summary>
        /// 比对指纹失败
        /// </summary>
        public const int ZKFP_ERR_VERIFY_FP = -20;
        /// <summary>
        /// 合并登记指纹模板失败
        /// </summary>
        public const int ZKFP_ERR_MERGE = -22;
        /// <summary>
        /// 设备未打开
        /// </summary>
        public const int ZKFP_ERR_NOT_OPENED = -23;
        /// <summary>
        /// 未初始化
        /// </summary>
        public const int ZKFP_ERR_NOT_INIT = -24;
        /// <summary>
        /// 设备已打开
        /// </summary>
        public const int ZKFP_ERR_ALREADY_OPENED = -25;
        /// <summary>
        /// [未知]
        /// </summary>
        public const int ZKFP_ERR_LOADIMAGE = -26;
        /// <summary>
        /// [未知]
        /// </summary>
        public const int ZKFP_ERR_ANALYSE_IMG = -27;
        /// <summary>
        /// 超时
        /// </summary>
        public const int ZKFP_ERR_TIMEOUT = -28;
    }
}
