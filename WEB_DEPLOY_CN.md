# 网页版构建与部署

使用 Unity 2022.3.57f1c2，需安装对应版本的 WebGL Build Support。

## 构建与部署

Unity 菜单选择 `Build > Build Web Game`，输出目录为 `Builds/WebGL`。

将该目录的全部内容上传到静态网站，保留 `index.html`、`Build` 和存在的 `StreamingAssets` 的相对位置。推荐使用 HTTPS。构建采用 Gzip 和 JavaScript 解压回退，`.unityweb` 文件作为二进制文件返回即可，无需设置 Content-Encoding。服务器不要将资源请求重定向为 HTML 页面。

本地测试：在 `Builds/WebGL` 执行 `python -m http.server 8080`，访问 `http://localhost:8080`。不能直接双击 HTML 文件运行。

## 全屏与操作

画布铺满浏览器可视区域，随窗口尺寸调整。右上角“全屏”按钮进入浏览器全屏，Esc 退出。浏览器要求用户点击后才允许全屏；不支持全屏 API 的设备仍然铺满可视区域。在 iframe 中部署时，宿主添加 `allowfullscreen` 和 `allow="fullscreen"`。

A / 左方向键向左攻击，D / 右方向键向右攻击；鼠标点击或触摸画面的左右区域也可以攻击，右上区域为暂停。

Player Settings 已关闭 Unity 启动画面和 Unity Logo，网页加载仅显示文字与进度。Unity 2022 的启动画面设置受编辑器许可证影响。如果许可证强制启用启动画面，需要使用允许关闭该画面的许可证或迁移到允许关闭该画面的 Unity 版本，并重新构建验证。

原生手机广告和内购 SDK 不提供网站广告和支付服务，网站商业化需要另行接入网页平台。
