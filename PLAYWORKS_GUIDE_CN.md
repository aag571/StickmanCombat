# Unity Playworks 可交互广告使用指南

本文根据 Luna Labs Playworks 官方公开文档整理，面向 Windows 上使用 Unity + Rider 开发 Unity Playable Ads 的项目。官方文档会随插件版本更新，安装前应以 [官方文档](https://docs.lunalabs.io/docs/playable/overview) 和当前下载包的说明为准。

## 1. Playworks 是什么

Playworks Plugin 用于把受支持的 Unity 项目转换成可在广告网络中运行的可交互广告。它不是普通 Unity 游戏的发布按钮：插件会编译、裁剪和转换 C#、资源、场景与运行时能力，因此项目必须使用它支持的 Unity 功能子集。

典型流程如下：

```text
Unity 项目
  → 安装 Playworks Plugin
  → 配置 Playable Settings
  → 选择需要导出的场景与资源
  → 创建 Develop Build
  → 浏览器预览、调试、检查体积
  → 创建正式 Build
  → 上传 Unity Playworks
  → 导出目标广告网络格式
```

## 2. 推荐开发环境

### 2.1 Unity 版本

官方快速入门当前列出支持的 Unity 版本包括 **2021.3 LTS、2022.3 LTS 和 6000.0 LTS**。最终以当前 Playworks Plugin 版本的兼容矩阵为准。新项目建议先使用项目要求的受支持 LTS 版本，不要在中途无理由升级 Unity 大版本。

### 2.2 Rider、Visual Studio 与 MSBuild

Rider 和 Visual Studio 都是代码编辑器，**可以使用 Rider 代替 Visual Studio**。官方说明开发者可以使用自己习惯的 IDE；Visual Studio 只是因为安装器便于同时安装 .NET 组件而被推荐。

MSBuild 是独立的构建工具，负责 Playworks 某些代码包的编译，不能因为使用 Rider 就省略。Windows 上可以安装 **Visual Studio Build Tools**，不必安装完整 Visual Studio IDE。

建议组合：

| 工具 | 用途 | 必要性 |
| --- | --- | --- |
| Unity | 编辑场景、资源和运行测试 | 必须 |
| Rider | 编写和调试 C# | Rider 与 Visual Studio 二选一 |
| MSBuild | Playworks 构建阶段编译代码包 | 使用 Playworks 导出时需要 |
| .NET Framework 4.7 开发组件 | 提供插件所需的目标框架、SDK 和引用程序集 | 按插件要求安装 |
| Playworks Plugin | 转换和打包 Unity 项目 | 必须 |

安装 .NET 时要包含对应的 Developer Pack、SDK 和 Targeting Pack。仅安装现代 .NET SDK 不代表已经安装了 .NET Framework 4.7 目标包。

Windows 的 MSBuild 常见路径类似：

```text
C:\Program Files (x86)\Microsoft Visual Studio\<版本>\BuildTools\MSBuild\Current\Bin\MSBuild.exe
```

实际路径取决于 Build Tools 的安装版本。Playworks 找不到 MSBuild 时，先检查环境变量、插件设置和该文件是否存在。

官方依据：[Quickstart](https://docs.lunalabs.io/docs/playable/quickstart)、[Step by step](https://docs.lunalabs.io/docs/playable/getting-started/step-by-step)。

## 3. 创建项目与安装插件

1. 创建一个独立的 Unity 2D 项目，优先使用受支持的 LTS 版本。
2. 不要把完整正式游戏直接复制进广告工程。只保留广告需要的场景、角色、动画、音效、材质和脚本。
3. 从 Unity Playworks 网站下载与 Unity 版本匹配的 Plugin 压缩包。
4. 将插件解压到项目目录以外，避免插件目录被 Unity 项目资产扫描或被版本控制误提交。
5. 在 Unity 的 Package Manager 中选择 **Add package from disk**，打开插件 `script` 目录中的 `package.json`。
6. 等待 Unity 完成导入和脚本编译；解决插件诊断窗口中的错误后再创建 Build。
7. 在 Rider 中打开脚本时，将 Rider 设置为 Unity 的 External Script Editor；这不会替代 MSBuild。

## 4. 项目裁剪原则

可交互广告的包体和加载时间通常受到广告网络严格限制。应从一个小型纵向切片开始，而不是迁移整个游戏架构。

对于左右攻击广告，建议只保留：

- 一个主场景和一个正交相机。
- 一个主角、一个普通敌人和一个精英敌人。
- 左右攻击、命中、受击、死亡和结束页脚本。
- 少量图集、打击特效、背景和音效。
- 一个 `PlayableApi` 或平台桥接层，用于下载跳转和生命周期通知。

应暂时移除或隔离：广告 SDK、支付、联网、排行榜、复杂存档、完整装备库、无关场景、编辑器工具、未使用字体和第三方 DLL。官方限制中明确提示外部插件需要检查支持情况；一般只能使用 C# 源码插件，不能假定任意 DLL 可以转换。

## 5. 支持范围与代码限制

Playworks 会把 C# 游戏代码转换为广告运行环境可以执行的代码，不能把普通 Unity 项目的全部 .NET 和引擎 API 当成可用。

开发时遵循以下规则：

- 先查看 [限制](https://docs.lunalabs.io/docs/playable/getting-started/limitations) 和 [支持特性](https://docs.lunalabs.io/docs/playable/getting-started/supported-features/supported-rendering-features)。
- 避免反射、动态程序集加载、线程、文件系统、网络请求和依赖平台原生 SDK 的逻辑。
- 使用简单的 C# 数据结构、显式状态机和有限对象池。
- 不要让核心流程依赖协程、异步 API 或插件回调，除非该版本文档明确列为支持。
- 只使用需要的 Unity UI、Animator、SpriteRenderer、AudioSource 和基础数学 API，并在 Develop Build 中验证。
- 外部插件需要逐个查看 [Unity Plugins 支持列表](https://docs.lunalabs.io/docs/playable/code/unity-plugins/overview)。支持某个插件不代表插件的全部模块和版本都支持。
- 使用 URP、后处理、Shader、粒子和 Cinemachine 前，先查对应支持页面；不支持或不稳定的功能应在首版广告中移除。

如果某个正式游戏系统无法转换，优先为广告实现一个独立的简化版本，而不是把原系统改到失去正式游戏稳定性。

## 6. 广告工程的推荐结构

```text
Assets/Playable/
├─ Scenes/PlayableScene.unity
├─ Scripts/
│  ├─ PlayableGameController.cs
│  ├─ InputController.cs
│  ├─ PlayerController.cs
│  ├─ EnemyController.cs
│  ├─ CombatResolver.cs
│  ├─ WaveDirector.cs
│  ├─ TutorialController.cs
│  ├─ EndCardController.cs
│  └─ PlayableApi.cs
├─ Config/PlayableConfig.asset
├─ Prefabs/
├─ Textures/
├─ Audio/
└─ UI/
```

玩法状态建议只有：

```text
Loading → TutorialLeft → TutorialRight → Playing → Won / Lost → EndCard
```

`PlayableGameController` 管流程，`InputController` 只产生左右攻击命令，`CombatResolver` 负责命中，`WaveDirector` 负责固定波次，`EndCardController` 负责 CTA。平台跳转代码集中在 `PlayableApi`，不得散落在战斗脚本中。

## 7. Playable Settings 与导出

安装插件后，先打开 Playworks Plugin 的设置页，确认：

- 导出场景只包含广告场景。
- 初始场景顺序明确。
- 开发构建使用正确的本地预览端口或输出目录。
- 广告网络设置与目标渠道一致。
- 包体、预加载器、结束页和压缩设置符合渠道要求。
- 需要注入的游戏变量使用 Playworks 支持的配置方式，不依赖运行时文件读取。

建议先做 **Develop Build**。它用于启动本地服务并在浏览器中预览，可快速定位脚本转换、资源缺失、输入和生命周期问题。不要直接把第一次导出当作正式广告包。

完成以下检查后再创建正式 Build：

1. Unity Console 和 Playworks Diagnostic 没有错误。
2. 浏览器中能从加载进入教学、战斗、胜利和失败结束页。
3. 移动触摸和桌面鼠标预览输入都能工作。
4. CTA 只有用户点击时才调用跳转。
5. 广告失去可见性时停止游戏和声音，恢复后不会跳过大量战斗时间。
6. 重新加载后没有残留对象、重复事件或重复奖励。
7. 通过插件的包体和资源分析，确认没有意外打入无关资产。

Unity Playworks 平台相关页面：[创建 Playable](https://docs.lunalabs.io/docs/playable/unityplayworks/create-playable)、[Playable 设置](https://docs.lunalabs.io/docs/playable/unityplayworks/playable-settings)、[测试](https://docs.lunalabs.io/docs/playable/unityplayworks/playable-testing)、[上传构建](https://docs.lunalabs.io/docs/playable/unityplayworks/upload-build)、[下载 Playable](https://docs.lunalabs.io/docs/playable/unityplayworks/download-playable)。

## 8. 左右攻击广告的实现建议

### 8.1 输入

- 移动端在 Canvas 中划分左右两个透明触控区。
- 桌面预览支持鼠标点击；编辑器可以额外提供 A / D。
- 使用按下事件，而不是抬起事件。
- 每次按下只生成一个方向命令，并提供约 0.1～0.15 秒输入缓冲。
- 触摸 CTA、声音按钮和重玩按钮时，UI 优先消费事件，不触发攻击。
- 结束页显示后禁用战斗触区。

### 8.2 固定波次

广告体验应使用固定波次，保证所有用户在相近时间看到精英和结束页：

```text
左侧教学敌人 → 右侧教学敌人 → 左右交替波次
→ 连续同侧波次 → 精英三次命中 → End Card
```

前两名敌人在合适距离等待，不造成伤害。正式波次才启用玩家生命和失败条件。不要直接复用正式游戏的随机刷怪、装备和排位系统。

### 8.3 结束页和跳转

结束页至少包含游戏名称、胜负结果、主 CTA 和重玩按钮。CTA 文案使用“下载游戏”或“立即体验”等真实含义，点击后才调用平台跳转接口。编辑器预览环境应使用假的 `OpenStore()` 实现并输出日志，避免开发时误打开商店。

Playworks API 与游戏变量参考：[Playable API](https://docs.lunalabs.io/docs/playable/playable-setup/playable-api)、[游戏变量](https://docs.lunalabs.io/docs/playable/playable-setup/configure-game-variables)。

## 9. 资源与包体优化

### 9.1 优先级

1. 删除未引用场景、预制体、材质、动画、字体和音频。
2. 合并并裁剪图集，避免把正式游戏的全部装备图标放进广告。
3. 降低纹理尺寸和压缩格式；背景、角色和 UI 分别评估清晰度。
4. 音频只保留必要的攻击、受击、胜利和按钮音，使用适合广告的压缩格式。
5. 减少粒子数量、材质数量和透明叠加层。
6. 使用对象池，避免战斗过程中频繁创建和销毁敌人。
7. 使用 Playworks 的 [Asset Size Breakdown](https://docs.lunalabs.io/docs/playable/optimise-your-builds/asset-size-breakdown/) 找出最大资源。

### 9.2 动画、粒子和性能

动画使用少量关键动作和复用图集。粒子应限制发射数量、生命周期和材质；参考 [动画优化](https://docs.lunalabs.io/docs/playable/optimise-your-builds/optimising-assets/optimising-animations/) 与 [粒子优化](https://docs.lunalabs.io/docs/playable/optimise-your-builds/optimising-assets/optimising-particles/)。

使用 [Performance Indicator](https://docs.lunalabs.io/docs/playable/optimise-your-builds/performance-indicator) 与实际移动设备检查首次加载、内存、帧率和输入延迟。编辑器运行流畅不能证明导出的 Playable 同样流畅。

## 10. 广告网络与平台适配

不同广告网络的包体、入口、结束页、横竖屏和跳转要求可能不同。先选定一个目标渠道，再按该渠道配置，不能用一个未经验证的 ZIP 直接投放所有渠道。

官方广告网络总览：[Ad Networks](https://docs.lunalabs.io/docs/playable/ad-networks/overview)。

建议至少记录：

| 项目 | 记录内容 |
| --- | --- |
| 渠道 | Unity Ads、AppLovin、Meta、Mintegral 等 |
| 输出格式 | HTML、ZIP 或渠道指定格式 |
| 方向 | 竖屏、横屏或自适应 |
| 体积限制 | 初始包体、压缩后包体、主资源大小 |
| 跳转 | 商店链接、测试跳转、正式跳转 |
| 生命周期 | 可见、隐藏、恢复、结束回调 |
| 审核要求 | 声音、血腥表现、品牌、CTA 和隐私要求 |

## 11. 调试与常见问题

### 找不到 MSBuild

确认已安装 Visual Studio Build Tools、MSBuild、.NET Framework Developer Pack 和 Targeting Pack。检查实际 `MSBuild.exe` 路径，再检查插件设置或系统 PATH。Rider 不能替代 MSBuild。

### C# 编译或转换失败

查看 Playworks Diagnostic 的第一个错误，不要只处理最后一条连带错误。检查是否用了不支持的 .NET API、反射、线程、DLL、外部插件或平台原生调用。将问题代码隔离成最小脚本，再查支持列表。

### 编辑器能运行，Develop Build 不能运行

检查场景是否被导出、资源是否在引用链上、脚本是否被插件转换、运行时是否使用了未支持的 API。不要把 `Resources` 中的全部内容当作自动可用资源。

### 包体过大

先使用 Asset Size Breakdown 定位最大项目，再裁剪字体、纹理、图集、动画和音频。删除资源引用比单纯压缩更有效；不要把整个正式游戏的装备和场景带入广告。

### 浏览器输入或声音异常

确认输入只在广告可见和战斗状态启用。浏览器通常要求用户交互后才允许声音，声音按钮和第一次攻击都应作为解锁声音的用户手势。后台恢复后重新同步状态，不要让计时器一次跳过几十秒。

更多故障排查入口：[Common Issues](https://docs.lunalabs.io/docs/playable/common-issues/overview)。

## 12. 推荐验收顺序

### P0：导出链路

空场景、一个按钮、一个点击事件，成功完成 Develop Build 并在浏览器预览。确认 Rider、MSBuild、.NET 目标包和插件版本都记录下来。

### P1：灰盒玩法

使用几何图形完成左右攻击、固定波次、生命、胜负和 CTA。验证触摸、鼠标、重玩与后台恢复。

### P2：素材与手感

接入主角、普通敌人、精英、动作、命中特效和音效。验证输入缓冲、攻击范围和结束演出。

### P3：优化与平台

执行资源分析、纹理和音频压缩，接入目标广告网络设置，确认包体、方向和跳转。

### P4：发布验收

在目标广告网络预览器和至少一台移动设备验证加载、操作、生命周期、CTA、声音、重玩和最终跳转。保存导出日志、包体分析和版本信息。

## 13. 当前项目的落地建议

对于左右攻击 Playable Ads，建议新建独立 Unity 工程，不迁移原项目的完整 `SceneManager`、商城、联网、装备和排位管理器。先建立一个单场景、单角色、固定敌人波次的最小工程；等 P0 导出验证通过后，再接入现有项目中已经确认的角色和中文资源。

这样可以尽早发现 Playworks 的 API 和资源限制，也避免为了可交互广告破坏正式游戏工程。每次只增加一个功能，并在 Develop Build、目标浏览器和移动设备各验证一次。

## 官方参考

- [Playable Overview](https://docs.lunalabs.io/docs/playable/overview)
- [Quickstart](https://docs.lunalabs.io/docs/playable/quickstart)
- [Step-by-step](https://docs.lunalabs.io/docs/playable/getting-started/step-by-step)
- [Limitations](https://docs.lunalabs.io/docs/playable/getting-started/limitations)
- [Supported Features](https://docs.lunalabs.io/docs/playable/getting-started/supported-features/supported-rendering-features)
- [Unity Playworks](https://docs.lunalabs.io/docs/playable/unityplayworks/overview)
- [Common Issues](https://docs.lunalabs.io/docs/playable/common-issues/overview)
