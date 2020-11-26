# NetModular基础通用库(Mkh.Utils)

本仓库是17mkh基础通用库，包含常用的一些帮助类、扩展方法等，它们不仅仅适用于17mkh框架，而是任何框架都可以使用，您只通过`NuGet`安装以下任意包，然后通过以下代码在自己的项目中添加注入即可：

```csharp
//此方法会在系统启动时自动注入所有使用`SingletonAttribute`、`TransientAttribute`、`ScopedAttribute`特性方式注入的服务，而下面所有库中的服务都已经采用了该方式
services.AddUtilsServices();
```

目前分为以下几部分：

### 1、Mkh.Utils (基础库)

本库是17mkh最基础的通用库，该库中封装的类或者扩展方法，会贯穿整个应用，从底层数据库，到最上层UI都会用到，而为了减少搭建频繁导入命名空间的操作，有些类以及扩展方式中的命名空间直接采用了`Mkh`，只要您的应用的命名空间前缀也是`Mkh`，那么您就可以很方便的使用这些扩展，比如通用数据转换扩展`CommonExtensions`，它的部分代码如下：

```csharp
using System;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Mkh
{
    /// <summary>
    /// 通用扩展方法
    /// </summary>
    public static class CommonExtensions
    {
         #region ==数据转换扩展==
         ...
         #endregion

         #region ==布尔转换==
         ...
         #endregion
    }
}
```

目前，该库主要包括以下功能：

> 1、数据转换扩展(CommonExtensions.cs)

该类中定义了很多常用的数据转换扩展方法，比如：

```csharp
//字符串转整数
var str = "123";
var i = str.ToInt();//str.ToLong()

//字符串转日期
str = "2020-08-23";
var d = str.ToDateTime();

等等，具体可以看源码，太多这里就不列出来了

```

> 2、依赖注入之特性注入功能

了解 `Spring Boot` 的人，应该都比较喜欢它通过注解进行依赖注入的方式吧，而 `.Net` 官方是没有提供这种方式的，所以我自己做了简单的封装，针对依赖注入的三种生命周期，定义了三个特性类，分别是：

`SingletonAttribute`，单例注入(使用该特性的服务系统会自动注入)

`TransientAttribute`，瞬时注入(使用该特性的服务系统会自动注入)

`ScopedAttribute`，单例注入(使用该特性的服务系统会自动注入)

怎么使用呢？很简单，在需要注入的类的头部添加对应特性即可，比如：

```csharp
[Singleton]
public class MyService
{
}
```

上面的方式相当于 `services.AddSingleton<MyService>()`

该类在系统启动时就会自动使用单例模式注入，如果要注入的服务继承了某个接口，框架也会自动判断，比如：

```csharp
public interface IMyService
{
}

[Singleton]
public class MyService : IMyService
{
}
```

上面的方式相当于 `services.AddSingleton<IMyService,MyService>()`

当然，有时候继承了接口但是还是希望注入服务本身的话也是可以的，那就是通过特性的构造函数设置`Itself`参数为true，如下：

```csharp
public interface IMyService
{
}

[Singleton(true)]
public class MyService : IMyService
{
}
```

上面的方式相当于 `services.AddSingleton<MyService>()`

另外两种生命周期跟上面的是一样的~

> 3、通用返回结果模型 `IResultModel.cs`

这个也是一个很好用的类，常用于方法返回值，用来表示方法是否成功。

### 2、Mkh.Utils.Encrypt (加解密库)

本仓库用于保存加解密操作类，目前包括：

> 1、MD5加密(`Md5Encrypt.cs`)

> 2、3DES加解密(`TripleDESEncrypt.cs`)

> 3、AES加解密(`AESEncrypt.cs`)

所有加解密操作类均采用单例特性`[Singleton]`注入~

### 3、Mkh.Utils.File (文件操作库)

本仓库封装常用的文件操作类

### 4、Mkh.Utils.Image (图片操作库)

本仓库封装常用的图片操作类，目前已包含图片验证码生成功能

### 5、Mkh.Utils.Web (Web相关通用封装)

本仓库封装常用的Web相关功能，目前已封装了文件上传功能，在`FileUploadProvider.cs`里面，同时还有获取请求客户端IP地址等扩展方法

### 6、Mkh.Utils.Excel (Excel操作相关封装)

待封装~

### 7、Mkh.Utils.Pdf (Pdf操作相关封装)

待封装~
