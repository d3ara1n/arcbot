# Arcbot

适用于 HyperaiShell 的插件集合， 提供一些没什么乱用的体验

## 部署 | Deployment

```bash
dotnet pack
cp *.nupkg [HyperaiShell/Plugins]
```
打包之后把包复制进 HyperaiShell 的 `plugins` 文件夹就完事了

## 依赖 | Requirements

**需要以下包同时存在于 `plugins` 目录下。**
- [octokit](https://www.nuget.org/packages/octokit/)
- [Wupoo](https://www.nuget.org/packages/Wupoo/)
- [SauceNET](https://www.nuget.org/packages/SauceNET/)

## 功能 | Features

以后写