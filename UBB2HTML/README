#userlog.js 使用说明#
----------------------------
**userlog.js** 为用以统计用户行为，收集用户数据作用的js脚本
该脚本依赖与**jQuery 1.7+**，并且扩展了jQuery的对象方法，可
进行自定义的对象绑定日志记录事件。
##开始使用##
最为简单的使用方法为

1. 引用userlog.js文件
2. 需要开始记录时需要实例化一个该对象

####UserLog 的构造方法####
UserLog 构造方法有三个参数，第一个参数 domain 为log记录
服务器的域。第二个参数setting为对该对象的设置，为json对
象。

setting对象为:

```javascript
{
	logServer:'server Address'
}
```

logServer 为服务器地址(不含http或者https头，有端口号的带端口号)
第三个参数jQuery为jQuery的实例对象

该对象实例化后立即开启日志记录
##UserLog 对象中的成员##
####cookieOpt####
该对象为操作cookie对象
######cookieOpt.createCookie######
创建Cookie,

参数:

* name 为cookie的名字
* expires:cookie的有效时间
* domain:cookie的有效域
* path:cookie的路径
* values:若为单个值，则为一个字符串，若有多个值，则为json对象
######cookieOpt.getCookie######
* name:要获取的cookie的名字
返回值如果只有一个，则为字符串，否则为json对象，找不到则为null
######cookieOpt.removeCookie######
* name:要删除的cookie名字
###数据收集action的值###
* action = jump 时表示页面跳转
* action = load 时表示页面加载
* action = leave 时表示离开页面
* action = click 时表示点击页面的链接或按钮
* action = submit 时表示点击了提交按钮
####setUser####
参数:

* uid:用户ID;
* usrName:用户名
在目标系统登录后设置
####delUser####
在目标系统成功登出用户后调用
####saveInfoToCookie####
参数 

* uid:用户ID;
* usr:用户名;
* ckname:cookie名字

其中扩展了jQuery的对象方法
####bindLogEvent####
为所选择的元素加上点击事件
####bindLogSubmit####
为所选择的元素加上提交事件
######用法######
$(选择器).bindLogEvent();
