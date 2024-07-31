# CustomControl.WPF

一些实用的WPF控件，并和MVVM结合。

## 环境和配置

- VS2022
- csproj改成新版本的，参考.NET Upgrade Assistant
- csproj里在`<PropertyGroup>`里添加`<LangVersion>8.0</LangVersion>`
- NuGet管理器里给项目安装CommunityToolkit.Mvvm

如果可以看到项目-依赖项-分析器，那应该是配置好了。如果自动生成的代码里自动生成了2份导致歧义，可以删除自动生成文件和obj文件夹，关闭VS，重新打开。

## 使用的框架

- WPF
- CommunityToolkit.Mvvm
