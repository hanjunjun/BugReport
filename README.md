# 桌面程序崩溃拦截模块（暂时只支持Win7）

## 1.项目结构
>```
>├─dist					项目编译输出目录
>├─Document				文档
>│  └─x86注入工具		 主动使程序崩溃的注入工具
>└─src					
>    ├─BugReport			错误报告UI
>    │  ├─Common
>    │  ├─lib
>    ├─BugReportLib		崩溃拦截模块
>    │  ├─build			
>    ├─DotNetTest		C#窗体测试程序
>    └─PythonTest		Python窗体测试程序
>```

## 2.开发环境配置

* 工程建议使用vs2019打开
* 安装net framework 4.0
* 安装PyCharm python开发工具

## 3.项目编译

> C#的比较简单直接重新生成整个解决方案即可。
> python的需要编译成exe，命令如下：
> pyinstaller ctest.py

##  4.Demo演示

> 这边演示的是C#在win7下崩溃拦截的步骤

![架构图](https://selfservice-doc.oss-cn-shanghai.aliyuncs.com/resources/4.0%E7%A1%AC%E4%BB%B6%E6%9C%8D%E5%8A%A1%E6%A6%82%E8%A6%81%E8%AE%BE%E8%AE%A1/%E7%A1%AC%E4%BB%B6%E6%9C%8D%E5%8A%A1%E6%9E%B6%E6%9E%84%E5%9B%BE.png)

> python的也是一样的步骤。不过要先把py代码编译成exe