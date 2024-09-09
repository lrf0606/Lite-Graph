# Lite Graph Frame

## 一、项目介绍

一套可视化编辑逻辑框架，通过增加节点、节点间连线实现游戏逻辑，功能类似UE的Blueprint。

项目源码使用C#编写，图形界面使用Unity接口实现，除图形界面外的功能不依赖于Unity环境，即在Unity编辑器中生成一份文件后，在客户端或服务端通过C#在运行时解析并执行。

框架优点：

- 使用简单，安装无需复杂操作，项目自定义节点添加简单，源码易读易修改。
- 双端通用，运行时不依赖Unity环境。

## 二、使用方法

### （一）示例项目

下载项目后直接作为Unity项目打开，在Unity编辑器Project面板中打开Asset/LiteGraphFiles/Example.litegraph样例文件，修改后点击左上角"Save"按钮保存，运行项目后检查运行时效果。

### （二）正常接入

1.拷贝Asset/Scripts/LiteGraphFrame文件夹到项目代码中。

2.修改LiteGraphFrame/Editor/CodeGenerate/GenerateInterface中的"BuiltinNodeRuntimeDirectory"和"CustomNodeRuntimeDirectory"两个配置路径，指定内置节点和自定义节点的存放路径。

3.Unity编辑器Project面板中右键"Create/Lite Graph File"创建一份litegraph文件，点击“Generate Node”按钮会在根据第2步中配置的路径生成一个“LiteGraphFiles"文件夹，建议在此文件夹中存放后续litegraph文件和自定义节点代码。

4.声明自定义节点，具体格式可参考示例项目的Asset/LiteGraphFiles/Editor中的代码；打开任一litegraph文件点击“Generate Node”按钮，在"CustomNodeRuntimeDirectory"路径下会生成自定义节点的实现代码，编写代码实现想要的逻辑，可以参考示例项目的Asset/LiteGraphFiles/Nodes中的代码。

5.打开任一litegraph文件编辑内容，右键选择"Create Node"创建节点，连线节点之间的端口控制流程和数据输入，编辑完成后点击"Save"按钮保存文件。

6.运行时测试，代码可以参考"Asset/RunLiteGraphExample.cs"。

### （三）名词说明

1.节点

- 事件节点：入口节点，通过流程端口连接其他功能节点，需要为每个事件节点设置唯一的事件ID，在代码中埋点调用。
- 数据节点：获取、构建某些数据，并通过数据端口传递给功能节点。
- 功能节点：具体功能的实现，通过数据端口接收数据节点传递的数据，根据数据实现某些逻辑，功能节点之间以流程端口连接。
- 自定义节点：各项目根据需求定制化的节点，包括事件节点、数据节点、功能节点。
- 内置节点：各项目通用的节点，例如For循环、If等流程控制功能，And、Or等逻辑判断功能。

2.端口

- 流程端口：负责节点的运行顺序控制。
- 数据端口：负责节点间的数据传递。‘

3.litegraph文件

- 把节点、端口、端口间连线等内容序列化存储得到一份json格式文件，运行时反序列化重新构建节点结构。

- 一般以模块划分文件，例如skill1001.litegraph、skill1002.litegraph、skill1003.litegraph，buff001.litegraph、buff002.litegraph。

### （四）节点拓展

1.自定义节点拓展



2.内置节点拓展

## 三、源码实现

## 四、后续计划

