# 2D_STG
## 游戏类型
像素STG

## 任务目标
总目标：3-5min，UI完整，架构干净+MVC   
分辨率：不限   
Unity版本：2020 LTS   
素材：主要 https://kenney.nl/assets/pixel-shmup   
设计方面注意：移动与射击样式；敌机掉落分布；强化道具（至少有提升射频）；关卡设计；
### 元素
自机、敌机、残机（为0时game over，可退游戏）、子弹
### UI
UGUI，完整，代码架构要好好设计

## 任务计划
### 核心玩法
自机/敌机的移动、射击，碰撞检测


强化道具(掉落：越往后掉落概率越高，掉落有冷却，玩家残机低时掉落概率高)

### 流程设计
关卡难度流程：易->中->短易->难   
- 敌人的生成与轨迹
- 子弹的生成与轨迹

### UI设计与实现：MVC
UIController处理来自游戏的请求并修改Model，UIModel存储游戏信息，UIView读取Model并显示信息

### 背景地图的生成
TileMap 。暂定手动创建四方连续图，后续考虑设定邻接规则由程序生成（可能造成不必要的资源浪费）

### 声音（8bit）与特效
模型、屏幕抖动；高空的云朵

### 游戏内容进一步细节优化（有时间再做）
- 技能系统
  - （涉及平衡问题，暂弃）招降技能，吸纳敌机（小怪）成为僚机。多种僚机，不同僚机攻击可产生相互作用。
  - 核爆技能
  - 无敌技能
  - 时停
- 核心点受击伤害高，其他部分受击伤害低，用特效区别

# 进度跟踪
### 2021.09.07
完成了基础逻辑。包括自机和敌机的基础移动、简单的循环背景、自机射击、碰撞检测销毁与音效、Game Over的再来一次和退出游戏
### 2021.09.08
设计关卡编辑器，了解UI架构。学习gizmos的用法，以创建敌机的行动路线