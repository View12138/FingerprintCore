using System;
using FingerprintCore.Models;
using FingerprintCore.Interface;
using FingerprintCore.Factory;
using FingerprintCore.Device;
using FingerprintCore.Extends;
using FingerprintCore.Device.Zkfp;
using System.Threading;

namespace ZkfpDemo
{
    class Program
    {
        static FingerprintInfo fingerprint = null;
        static IFingerprint zkfp = FingerprintFactory.CreateFingerprint(Devices.ZKFP);
        static CancellationTokenSource source = new CancellationTokenSource();

        static void Main(string[] args)
        {
            UserZkfpTool();

            Console.WriteLine("按 Q键 结束指纹识别操作");
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    source.Cancel();
                    break;
                }
            }
            Console.WriteLine("按任意键退出");
            Console.ReadKey(false);
        }



        public async static void UserZkfpTool()
        {
            zkfp.Initialization();
            // 初始化工具类
            do
            {
                if (!zkfp.IsInit)// 初始化指纹仪
                {
                    zkfp.Initialization();
                    Console.WriteLine("请连接指纹仪器后按任意键重试。");
                    Console.ReadKey(false);
                }
                else
                {
                    if (fingerprint == null)
                    {// 未注册
                        FingerprintInfo[] fingerprints = new FingerprintInfo[3];
                        fingerprint = await zkfp.RegisterAsync((tips, type) =>
                        {
                            switch (type)
                            {
                                default:
                                case MessageType.info:
                                    Console.ResetColor();
                                    zkfp.SetLight(LightType.White);
                                    break;
                                case MessageType.Success:
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    zkfp.SetLight(LightType.Green);
                                    break;
                                case MessageType.Error:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    zkfp.SetLight(LightType.Red);
                                    break;
                            }
                            Console.WriteLine(tips);
                        }, source.Token);
                        if (source.Token.IsCancellationRequested)
                        { break; }
                    }
                    else
                    {
                        // 已注册，调用验证
                        FingerprintInfo _fingerprint = await zkfp.GetFingerprintAsync(source.Token);
                        if (source.Token.IsCancellationRequested)
                        { break; }

                        if (zkfp.Verify(fingerprint, _fingerprint))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("验证成功");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("验证失败，请重试");
                        }
                        Console.ResetColor();
                    }
                }
            }
            while (true);
        }
    }
}
