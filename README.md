# StartbitKit
![StartbitKit](https://gitee.com/startbitkit/startbit-kit/raw/master/Hardware/picture.png "在这里输入图片标题")
#### 介绍
开源的USBCAN设备，包含硬件、嵌入式软件、上位机软件。
主控芯片采用的STM32F105RBT6，硬件有USB、CAN、TF卡等接口，软件有以下功能：
- 报文显示
- 信号值显示（解析dbc显示信号值）
- 信号曲线显示（解析dbc显示信号曲线，使用开源的zedgraph控件）
- 信号发送（周期发送，波形生成）
- 报文记录（asc格式）
- 报文回放（asc文件的报文回放到总线上）
- 总线状态显示（报文统计、负载率、总线状态、错误帧）
- 输入输出控制（io、ad）
- 自动化测试（待细化）
- uds诊断（待细化）
- uds烧写（在线、离线）
- 附加工具（报文记录格式转换、报文记录裁剪）
- 总线干扰（待细化）
- 扩展脚本（使用eclipse for c/c++和mingw编写dll，上位机调用dll）

#### 开发环境
硬件：
- PCB绘制：PADS Logic VX.2.2

嵌入式软件：
- 编译环境：STM32CubeIDE 1.4.0
    
上位机软件：

- 语言：C#（Winform）
- 编译环境：Visual Studio Community 2017

#### 参与贡献

1.  有任何建议、帮助都可以提交issue
2.  QQ群：678397352
