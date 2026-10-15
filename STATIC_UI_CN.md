# GameFlow 静态界面

游戏对象使用固定英文名，例如 TutorialButton、SettingsButton、WeatherToggle。中文只用于界面显示文字，开关状态不得写入对象名。可通过 Tools > Stickman Combat > Normalize UI Object Names 同步已有预制体和两个场景中的命名，不会重新生成布局。

带文字的按钮背景统一参考 MainMenu/新手教程；设置入口仅显示设置图标，不含文字。设置面板中的开关、关闭和返回按钮仍使用普通文字按钮背景。样式已保存到 GameFlowUI 预制体及 MainMenu、Game 场景实例。

CombatSceneRoot 中的旧菜单、旧教程图示、旧对话框和提示框 UI 已清理。设置状态保留在 SettingsState，天气粒子及音频仍保留实际引用。当前界面统一位于 GameFlow/GameFlowUI 下。

MainMenu 和 Game 场景的 GameFlow 下均包含 GameFlowUI 预制体实例。
共享预制体：Assets/UI/GameFlowUI.prefab。

展开 SafeArea 可以编辑 MainMenu、Settings、Equipment、Result、Unlocks、
MobileControls，以及 HUD、当前装备和设置入口。隐藏面板可在编辑模式下
临时启用预览；游戏启动时会按当前场景重设显示状态。

修改共享样式时直接编辑预制体；只想修改一个场景时使用实例覆盖。
装备列表固定保存 22 个 EquipmentSlot，图标、名称、介绍、状态与按钮均
为真实组件。运行时只填入装备数据并绑定事件，不创建或销毁卡片。

运行时变化仅包括文本、音量与进度、交互状态、面板显隐、安全区域、
手机操作按钮显隐，以及设置入口位置。按钮事件在 Initialize 中绑定。

Tools > Stickman Combat > Create Static UI 可重新生成默认布局。
重新生成会覆盖预制体及场景中的 UI 实例修改，操作前会弹出确认框。
正常编辑不需要重新生成，也不需要运行游戏。
