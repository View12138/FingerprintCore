using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkfpHelp;

namespace ZkfpDemo
{
    /**
     * 简单的使用示例
     */
    class Program
    {
        static void Main(string[] args)
        {
            UserZkfpTool();
            Console.ReadKey(false);
        }

        public async static void UserZkfpTool()
        {
            // 初始化工具类
            ZkfpTool Zkfp = new ZkfpTool();
            while (true)
            {
                if (!Zkfp.Init())// 初始化指纹仪
                {
                    Console.WriteLine("请连接指纹仪器后按任意键重试。");
                    Console.ReadKey(false);
                }
                else
                {
                    if (string.IsNullOrEmpty(Zkfp.Fingerprint))
                    {// 未注册
                        bool success = await Zkfp.RegisterAsync((tips, color) =>
                        {
                            if (color == ColorInfo.Success)
                                Console.ForegroundColor = ConsoleColor.Green;
                            else
                                Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(tips);
                        });
                        if (success)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("注册成功");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("注册失败，请重试");
                        }
                        Console.ResetColor();
                    }
                    else
                    {// 已注册，调用验证
                        bool success = await Zkfp.IdentifyAsync();
                        if (success)
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
        }
    }
}
