# Stickman Combat

Unity stickman combat game for desktop and mobile browsers.

## Development

Open the project with Unity **2022.3.57f1c2** and install WebGL Build Support. Start from `Assets/Scenes/MainMenu.unity`. Tutorial, Ranked, and Endless modes use `Assets/Scenes/Game.unity`.

GameObject names use stable English identifiers such as `TutorialButton`, `SettingsButton`, and `WeatherToggle`. Localized text and toggle values belong in UI components, never in object names. Use `Tools > Stickman Combat > Normalize UI Object Names` to normalize the saved UI without regenerating its artwork or layout.

## Website Deployment

1. Run `Build > Build Web Game` in Unity after changing the game. Vercel does not compile Unity source.
2. Run `node scripts/verify-web-build.mjs` to check the generated files.
3. Commit the Unity source and the complete `Builds/WebGL` directory, then push to `main`.
4. Vercel reads `vercel.json`, verifies the existing build, and publishes `Builds/WebGL`.

`Library`, `Temp`, local settings, archived plugins, credentials, and editor-generated solution files are excluded from Git.

The website fills the browser viewport and supports optional browser fullscreen. Unity build assets use Gzip with JavaScript decompression fallback; `.unityweb` files must be served as binary files without a manually added `Content-Encoding` header. Missing asset requests must return 404 instead of the HTML page.

## Domain and CDN

The intended production domain is `https://notcardgod.site`. Add it to the Vercel project and use the DNS values shown by that project. Cloudflare manages DNS and proxies web traffic. Use Full (strict) TLS after the Vercel origin certificate is valid, and enable HTTPS redirection.

Build assets revalidate in browsers and allow an edge cache of one hour. Purge the Cloudflare cache after publishing a replacement build because Unity uses stable filenames. The HTML entry must revalidate so visitors receive current asset references.

Cloudflare's international network does not guarantee availability or latency on every mainland China ISP. Validate without a VPN on mainland mobile and broadband connections before advertising nationwide availability. Guaranteed mainland acceleration requires a service with mainland nodes and the corresponding domain and hosting requirements.
