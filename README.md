# OPQBot-Native
[OPQBot](https://github.com/OPQBOT/OPQ) 酷Q插件 兼容框架，借用了[Native.FrameWork](https://github.com/Jie2GG/Native.Framework)的CQ码处理函数
## 写在前面
这是一个非常粗糙的程序，目前这个框架只能应付不是特别复杂的应用，大部分的事件与函数都没有经过深入的测试，也没有长时间的挂机稳定性测试，感觉会出很多的问题。希望得到大家对于代码层面上的支援。项目将会持续维护，多多包涵！<br>
此框架仅可以运行编译得到的dll与json，不支持**cpk**以及从**cpk导出的dll**
## 食用教程
1. 下载一个[release](https://github.com/Hellobaka/OPQBot-Native/releases)包并解压
2. 运行[OPQBot](https://github.com/OPQBOT/OPQ)服务端并[登录](https://github.com/OPQBOT/OPQ/wiki/%E5%AE%89%E8%A3%85%E6%8C%87%E5%8D%97#%E7%99%BB%E5%BD%95)
3. 将要运行的酷Q插件的dll以及json放到plugins文件夹内
4. 如果插件有数据配置，可以直接复制到data文件夹下
5. 运行解压后的Launcher.exe
6. 按照配置输入相关配置即可
## 事件实现进度
- [x] id: 1 私聊消息处理
- [x] id: 2 群消息处理
- [ ] id: 3 讨论组消息处理(讨论组不存在,将不会实现)
- [x] id: 4 群文件上传事件处理
- [x] id: 5 群管理变动事件处理(无法获取操作者QQ，似乎是接口未提供)
- [x] id: 6 群成员减少事件处理(无法获取操作者QQ，以及主动退群还是被踢)
- [x] id: 7 群成员增加事件处理(无法获取操作者QQ)
- [x] id: 8 群禁言事件处理
- [x] id: 10 好友已添加事件处理
- [x] id: 11 好友添加请求处理
- [x] id: 12 群添加请求处理(暂时不能获取是被邀请还是主动进群)
- [x] id: 1001 酷Q启动事件
- [ ] id: 1002 酷Q关闭事件
- [x] id: 1003 应用已被启用
- [ ] id: 1004 应用将被停用
- [x] 呼出窗口
## CQ码解析进度
- [x] 图片(CQ:image)
- [x] at(CQ:at)
- [x] 音频(CQ:record)(已知可用的格式 amr silk，其中silk格式的音频在电脑端无法正常听，在手机端可以正常听，但是不能转文本)
- [ ] 地图(CQ:loaction)
- [ ] xml消息(CQ:rich)
- [x] 表情(CQ:face)
- [x] emoji(CQ:emoji)
- [ ] 原创表情(CQ:bface)
- [ ] 红包(CQ:hb)
- [ ] 签到(CQ:sign)
- [ ] 群聊/个人名片分享(CQ:contact)
- [ ] 音乐(CQ:music)
- [ ] 猜拳(CQ:rps)
- [ ] 筛子(CQ:dice)
- [ ] 戳一戳(CQ:shake)
## TODO
- [ ] 继续完善事件以及CQ码解析
- [ ] 实现事件的优先级排序
- [ ] 实现authcode对调用函数的限制
- [ ] 插件的启用、禁用以及重载
- [ ] 解决WPF窗口呼出的线程问题
- [ ] 实现消息队列
- [x] GUI界面(在画了在画了)
- [ ] 多进程加载插件，防止插件互相冲突
- [ ] 日志通过sqlite写入本地数据库
- [ ] 使用 .net core实现跨平台(
- [ ] 优化代码逻辑，加速执行速度
**......**
## 已知的问题
* 特殊字符的编码问题
* 非C#插件(非托管代码)爆炸时主程序也可能会爆炸
* 受限于OPQBot的WebApi并发频率限制，只能一秒一条
* C#插件窗口闪退时会带着主程序闪退
* 呼起的普通WinForm有显示问题
* 无法处理入群请求事件
* 断网重连问题，无法获取程序是否真的重连上服务器，表现为不能获取服务端消息推送
* emoji解析问题，只能解析双字符宽度的emoji
* 整理中……逐步寻找解决方案来修复
