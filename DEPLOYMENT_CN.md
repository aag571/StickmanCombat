# 网站部署记录

部署日期：2026-10-15。

## 已创建的资源

- 游戏网址：https://notcardgod.site
- GitHub 私有仓库：https://github.com/aag571/StickmanCombat
- Vercel 项目：https://vercel.com/aag571s-projects/stickman-combat
- 生产分支：main
- 发布目录：Builds/WebGL
- Unity 版本：2022.3.57f1c2

仓库保存 Unity 源码、资源、项目设置和可直接部署的 WebGL 构建。Library、Temp、用户设置和已移除插件归档不上传。

## Cloudflare 配置

notcardgod.site 已使用 Cloudflare 权威 DNS。根域名 CNAME 指向 Vercel 为本项目提供的 784b4d934c43f374.vercel-dns-017.com，并开启代理。

- SSL/TLS：Full (strict)。
- Always Use HTTPS：开启。
- /Build/ 路径：允许 Cloudflare 缓存，遵循源站一小时边缘 TTL 和浏览器重新验证策略。
- / 与 /index.html：绕过 Cloudflare 缓存，以获取最新入口。
- .unityweb：application/octet-stream，使用 Unity 的 JavaScript Gzip 解压回退。

已验证 HTTPS 返回 200、HTTP 跳转 HTTPS、WebGL 构建文件返回 200、Cloudflare 返回 CF-Cache-Status: HIT，以及不存在的资源返回 404。浏览器已完整下载构建并显示游戏主菜单。

## 后续更新

1. 在 Unity 中修改游戏并保存场景。
2. 执行 Build > Build Web Game，重新生成 Builds/WebGL。
3. 执行 node scripts/verify-web-build.mjs。
4. 提交修改并推送 main，Vercel 会自动验证并发布已有 WebGL 文件。
5. Vercel 部署成功后，在 Cloudflare 中清理该站缓存。Unity 构建文件名固定，清理缓存可以避免一小时内继续获取旧构建。

Vercel 不会直接编译 Unity 项目；只上传 C# 或场景修改而不重新构建，网站游戏不会更新。

## 国内访问验证范围

已验证自定义域名和 Cloudflare 代理链路，并在本机通过禁用 curl 显式代理的方式请求成功。测试响应来自 Cloudflare SEA 节点；这不等同于中国大陆多运营商实测，也不能排除系统级 VPN/TUN 的影响。

Cloudflare 国际网络无法承诺国内全部地区和运营商始终稳定访问。如需明确的大陆可用性保证，应安排大陆电信、联通、移动和手机实际测试，并根据结果选择具备大陆节点的 CDN 或其他托管方案。
