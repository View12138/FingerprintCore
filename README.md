# 中控指纹仪的辅助工具类

## 简介：
	对 libzkfpcsharp.dll 的重写和补充了异步方法。
## 环境：
    0. 要求：安装 ZKFinger SDK(Windows) 5.0.0.20 或以上版本
	1. 使用设备：中控指纹 Live 20R
    2. 类库版本：.Net Standard 2.0
    3. Demo版本：.Net Framework 4.7.2
## 使用：
    可参照 ZkfpDemo 项目
```
    // 初始化帮助类
    ZkfpTool Zkfp = new ZkfpTool();

    // 异步注册指纹
    bool success = await Zkfp.RegisterAsync();
    if(success)
    {
        // 获取指纹模板的字符串
        string fp = Zkfp.Fingerprint;
    }

    // 设置你要验证的指纹模板
    Zkfp.Fingerprint= /* 你的指纹模板 */
    
    // 异步验证
    bool success = await Zkfp.IdentifyAsync();
    if(success)
    {
        // 验证成功
    }
```
