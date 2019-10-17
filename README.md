# 中控指纹仪的辅助工具类

## 简介：
	对 libzkfpcsharp.dll 的重写和补充了异步方法。
## 环境：
    0. 要求：安装 ZKFinger SDK(Windows) 5.0.0.20 或以上版本
	1. 使用设备：中控指纹 Live 20R
    2. 类库版本：.Net Standard 2.0
    3. Demo版本：.Net Framework 4.7.2
## 建议：
    0.只有注册指纹和验证已有指纹时，请只使用 `ZkfpHelp.ZkfpTool` 类。
    1.有其它复杂使用场景时，请参照 `ZKFP` 官方文档使用。
## 使用：
[ZkfpHelp.dll 下载地址](https://github.com/View12138/ZkfpHelp/releases)  
    
    1. 在你的 Windows 7+ 上安装 ZKFinger SDK(Windows) 5.0.0.20+
    2. 将 ZkfpHelp.dll 和 ZkfpHelp.xml 放到同一目录下 (确保能有智能提示) 
    3. 在你的项目中添加引用 ZkfpHelp.dll
> 注: 可参照 ZkfpDemo 项目
    调用方式:
```
    // 初始化帮助类
    ZkfpTool Zkfp = new ZkfpTool();

    // 初始化并连接第一个指纹仪
    Zkfp.Init(0);

    // 异步注册指纹
    bool success = await Zkfp.RegisterAsync();
    if(success)
    {
        // 获取刚注册的指纹模板的字符串
        string fp = Zkfp.Fingerprint;
    }

    // 设置你要验证的指纹模板字符串
    Zkfp.Fingerprint= /* 你的指纹模板字符串 */
    
    // 异步验证
    bool success = await Zkfp.IdentifyAsync();
    if(success)
    {
        // 验证成功
    }

    // 释放资源
    Zkfp.Dispose();
```
## 更新：
> 新增可以取消验证指纹，使用方法：
```
    CancellationTokenSource cts = new CancellationTokenSource();
    CancellationToken token;

    /// <summary>
    /// 开始指纹验证
    /// </summary>
    public async void BeginIdentify()
    {
        token = cts.Token;
        bool? result = await App.Zkfp.IdentifyAsync(token);
        if(!result.HasValue)
        {
            //取消验证
        }
        else if(result.Value)
        {
            //验证成功
        }
        else
        {
            //验证失败
        }
    }

    /// <summary>
    /// 取消指纹验证
    /// </summary>
    public void CancekIdentify()
    {
        if (!token.IsCancellationRequested)
        {
            cts.Cancel();
        }
    }

```
