# 相机替换与依赖更新

主场景已使用项目自有相机脚本，不再依赖 Pro Camera 2D。新实现只使用 UnityEngine API，适用于当前 Unity 2022.3 项目。

## 相机行为

- `Assets/Scripts/CombatCameraFollow.cs`：在 LateUpdate 中平滑跟随主场景原有目标，保留目标偏移 `(0, 2.5)`、水平与垂直平滑时间 `0.15`，以及装备界面使用的 `OverallOffset`。
- `Assets/Scripts/CombatCameraShake.cs`：根据持续时间、强度和振动次数产生衰减震屏，支持使用不受时间缩放影响的时间。震屏偏移与跟随位置分别计算，避免震屏引起累计漂移。
- 相机 Z 轴位置继续由 `SceneManager.targetCameraZ` 的原有缩放逻辑控制。剧情和终结技继续通过移动、重新挂接原有跟随目标切换焦点。
- `SceneManager.shaker` 和 `CharacterManager.proCam` 仍引用主场景原组件位置，对应脚本已替换；序列化的 `shakePreset` 参数已保留。
- 新震屏算法使用程序生成的振动曲线，视觉手感可能与旧插件不同，可在主相机的 `CombatCameraShake.defaultSettings` 中调整。

## EDM4U

已从本地 `unity-jar-resolver-1.2.190` 下载目录安装 1.2.190 正式插件文件，位置为 `Assets/ExternalDependencyManager`，并安装发布包中的 `Assets/PlayServicesResolver` 兼容标记。安装文件已与对应 `.unitypackage` 内容逐一核对。

完整下载目录移至 `ThirdPartyDownloads/unity-jar-resolver-1.2.190`，避免其中的源码、测试 DLL 和多份插件副本被 Unity 重复导入。

旧资源保留在 `ThirdPartyArchive/ProCamera2D` 和 `ThirdPartyArchive/EDM4U-1.2.161`，均位于 Assets 外，不参与游戏编译与资源导入。

## 验证状态

场景脚本 GUID、组件引用、跟随目标引用和 Pro Camera 2D 命名空间移除检查通过。使用临时编译项目排除已归档的插件源码并包含新相机脚本后，未出现相机相关编译错误。

`MonetizationManager.cs` 已改用 Unity Ads 4.4.2 的加载、播放监听接口，加载或播放失败会关闭加载提示，只有完整观看才发放奖励，并忽略重复的完成回调。Unity Ads 当前没有配置 Game ID；实际接入广告前，需要初始化 SDK，并在组件中填写后台对应的激励广告单元 ID。未初始化时会输出中文提示，不会发放奖励。

`dotnet build StickmanCombat.sln --no-restore --verbosity quiet` 已验证通过：120 个项目，0 个错误。仍有旧插件 API、未使用字段等警告。日志位于 `Temp/FullProjectBuild.log`。

`Assets/Editor/SolutionProjectNames.cs` 为生成的 Player 项目显示名称添加 `.Player` 后缀，避免解决方案项目重名；`Directory.Build.props` 按项目名称隔离编译中间目录，避免编辑器版和 Player 版同名程序集相互覆盖。上述设置仅用于外部 C# 工程构建。

当前尚未完成游戏运行画面的验证。进入运行模式后，应检查开始游戏、左右移动、装备界面、剧情聚焦、终结技缩放、震屏结束以及暂停恢复。
