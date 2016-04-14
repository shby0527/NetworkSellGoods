/*********************************************************
    该文件是Mysql，.Net 网上订餐系统数据库数据表建立脚本
    该文件中并未含有测试数据，也无数据库名称，仅仅提供所需数据表
    的建立，数据库设计参照本路径下的的E-R图设计
    数据库使用 Mysql 5.5+ innodb 数据引擎，以及utf-8字符集
    数据表以及各个字段的含义由各个表字段上方的注释给出
    使用该脚本创建数据表时，请先登录mysql数据库
    mysql -uusername -p
    mysql>create database databasename;
    mysql>use databasename;
    mysql>\. database.sql
    即可，测试数据的添加将使用特殊编制的程序进行添加
    由于password 字段的加密存在，手动添加一个用户名将会有难度
**********************************************************/

-- 用户表段
create table `info_user`(
    -- 用户UID
    `uid`        int unsigned not null auto_increment,
    -- 用户名
    `username`   varchar(20) not null unique,
    -- 密码（兼容性加密存储，使用一个密码及相关信息集合体对象）
    `password`   varbinary(4096) not null,
    -- 用户昵称
    `nick`       varchar(20) not null,
    -- 用户头像，这里仅保存存储路径，不存放头像数据
    `avator`     varchar(2048) null,
    -- 注册时间（用户到达时间）
    `regTime`    datetime not null,
    -- 最后登录时间，刚刚注册的用户最后登录时间同注册时间
    `lastLogin`  datetime not null,
    -- 用户目前可用状态,可选值为（1：正常使用，2：已经被封禁，其他值为保留值）
    `status`     tinyint unsigned not null default 1,
    -- 用户等级（根据算法计算得出）
    `level`      tinyint unsigned not null default 0,
    -- -----------------------------------------------------------
    -- 键索引及约束
    primary key(`uid`),
    index (`nick`)
)engine=innodb default charset=utf8;

/* 所有权限定义说明
 * 0x0001：浏览菜单以及搜索
 * 0x0002：下订单（包括撤销订单）
 * 0x0004：更改订单状态
 * 0x0008：发布评论
 * 0x0010：删除评论
 * 0x0020：添加菜单
 * 0x0040：删除菜单
 * 0x0080：添加分类
 * 0x0100：删除分类
 * 0x0200：用户管理
 * 0x0400：权限授予
 * 0x0800：权限撤销
 * 以上任意权限可用位或运算任意组合
 */

 -- 用户组
create table `info_userGroup`(
    -- 组ID
    `gid`         int unsigned not null auto_increment,
    -- 组名
    `gname`       varchar(20) not null unique,
    -- 组权限
    `permission`  int unsigned not null,
    -- 组说明
    `commit`      varchar(50) not null,
    -- -----------------------------------------------
    primary key (`gid`)
)engine=innodb default charset=utf8;

-- 对应表
create table `rel_uglink`(
    `uid`      int unsigned not null,
    `gid`      int unsigned not null,
    primary key (`uid`,`gid`),
    foreign key (`uid`) references `info_user`(`uid`),
    foreign key (`gid`) references `info_userGroup`(`gid`)
)engine=innodb default charset=utf8;

-- --------------------------------------------------------------------------
-- 用户及其对应权限状态部分定义结束，下面为用户扩展信息以及用户密码保护信息
create table `info_realinfo`(
    `uid`         int unsigned not null,
    -- 用户真实姓名
    `name`        varchar(8) not null,
    -- 用户性别 1:男 2： 女
    `sex`         tinyint unsigned not null,
    -- 用户证件类型
    `licensetype` tinyint unsigned not null,
    -- 用户证件号码
    `licenseid`   varchar(40) not null,
    -- 用户手机号码
    `phonenumber` varchar(16) not null,
    -- ----------------------------------
    primary key (`uid`),
    foreign key (`uid`) references `info_user`(`uid`)
)engine=innodb default charset=utf8;

-- 邮箱认证信息
create table `info_email`(
    `uid`       int unsigned not null,
    -- 用户邮箱地址
    `address`   varchar(50) not null,
    -- 认证代码
    `vailcode`  char(64) not null,
    -- 认证码生成时间
    `maketime`  datetime not null,
    -- 完成标记 0:未完成 1:已完成 2:已失效 其他为预留值
    `sign`      tinyint unsigned not null default 0,
    -- ----------------------------------------------
    primary key (`uid`),
    foreign key (`uid`)  references `info_user`(`uid`)
)engine=innodb default charset=utf8;

-- ---------------------------------------------------------
/*
 * 下面定义商品信息
 * 以下的表信息为商城网络商品的分类表
 * 以及其商品表
 * 以及对该商品的评论信息
 */
 
 
-- 商品分类信息
create table `info_type`(
     `tid`     int unsigned not null auto_increment,
     -- 分类名
     `tname`   varchar(20) not null unique,
     -- 描述
     `commit`  varchar(255) not null,
     -- 父分类id
     `ptid`    int unsigned null default null,
	 -- 状态 0 正常 1 删除
	 `status`  tinyint unsigned not null default 0,
     -- ------------------------------------
     primary key (`tid`),
     foreign key(`ptid`) references `info_type`(`tid`)
)engine=innodb default charset=utf8;
 
-- 商品信息
create table `info_foods`(
    `gid`      int unsigned not null auto_increment,
    -- 商品名
    `gname`    varchar(30) not null,
    -- 商品图片(存放地址)
    `gpic`     varchar(2048) not null,
    -- 商品说明
    `gcommit`  varchar(255) not null,
	-- 单价
	`gprice`   int unsigned not null,
	-- 状态 0 正常 1 下架/删除
	`status`   tinyint unsigned not null default 0,
    -- -------------------------------
    primary key(`gid`),
    index (`gname`)
)engine=innodb default charset=utf8;

-- 商品更多图片信息
create table `info_moreimg`(
    `miid`    int unsigned not null auto_increment,
    -- 图片存放地址
    `mipic`   varchar(2048) not null,
    -- 对应的商品信息
    `gid`     int unsigned not null,
    -- -------------------------------------
    primary key(`miid`),
    foreign key(`gid`) references `info_foods`(`gid`)
)engine=innodb default charset=utf8;

-- 关联信息(分类<->商品)
create table `rel_tflink`(
    `tid`      int unsigned not null,
    `gid`      int unsigned not null,
    primary key(`tid`,`gid`),
    foreign key(`tid`) references `info_type`(`tid`),
    foreign key(`gid`) references `info_foods`(`gid`)
)engine=innodb default charset=utf8;

-- 对商品的评论
create table `info_repforgood`(
    `rid`        int unsigned not null auto_increment,
    -- 评论内容
    `rcontent`   varchar(1024) not null,
    -- 评论发布时间
    `rstime`     datetime not null,
    -- 评论发布状态 1:正常 2:删除 其他预留
    `rstatus`    tinyint unsigned not null default 1,
    -- 评论商品
    `gid`        int unsigned not null,
    -- 评论用户
    `uid`        int unsigned not null,
    -- -----------------------------------
    primary key(`rid`),
    foreign key(`gid`) references `info_foods`(`gid`),
    foreign key(`uid`) references `info_user`(`uid`)
)engine=innodb default charset=utf8;
 
-- -------------------------------------------------------
/**
 * 用户订单信息开始
 * 内容包括 用户 联系地址信息
 * 购物车
 * 订单
 * 对订单的评论（只有晒单之后才可大家对该单的评论否则仅自己可见）
 * 晒单
 * 以及其他的一些用户订单相关数据
 */

-- 用户通信地址
create table `info_address`(
    `aid`               int unsigned not null auto_increment,
    -- 通信地址备注
    `commit`            varchar(50) not null,
    -- 通信地址详细信息
    `information`       varchar(2048) not null,
    -- 联系人姓名
    `callperson`        varchar(10) not null,
    -- 联系人手机
    `callnumber`        varchar(16) not null,
    -- 对应的用户
    `uid`               int unsigned not null,
    -- 状态 0 正常 1 删除
    `status`            tinyint unsigned not null default 0,
    -- ---------------------------------------------------------
    primary key(`aid`),
    foreign key(`uid`) references `info_user`(`uid`)
)engine=innodb default charset=utf8;

-- 用户购物车(当购买付款后，相应的购物车商品将会从购物车中删除)
create table `info_cart`(
    `csign`            int unsigned not null auto_increment unique,
    -- 用户ID
    `uid`              int unsigned not null,
    -- 商品ID
    `gid`              int unsigned not null,
    -- 商品数量
    `count`            int unsigned not null,
    -- ------------------------------------------------------------
    primary key(`uid`,`gid`),
    foreign key(`uid`) references `info_user`(`uid`),
    foreign key(`gid`) references `info_foods`(`gid`)
)engine=innodb default charset=utf8;

-- 用户订单
create table `info_indent`(
    -- 订单号
    `bid`         varchar(50) not null,
    -- 订单创建时间
    `createtime`  datetime not null,
    -- 订单说明
    `commit`      varchar(1024) not null,
    -- 订单价格
    `piace`       int unsigned not null,
    -- 订单状态 1:已创建 2:已付款 3:已出货 4:已完成(未评分) 
    -- 5:已完成(已评分) 6:已删除 7:已晒单 其他为定义
    `status`      tinyint unsigned not null default 1,
    -- 评价指数(0~10)
    `star`        tinyint unsigned not null default 0,
    -- 关联的用户ID
    `uid`         int unsigned not null,
    -- 关联的通信地址
    `aid`         int unsigned not null,
    -- ----------------------------------------------------------
    primary key(`bid`),
    foreign key(`uid`) references `info_user`(`uid`),
    foreign key(`aid`) references `info_address`(`aid`)
)engine=innodb default charset=utf8;

-- 订单中包含的商品
create table `info_indentgoods`(
    -- 订单号
    `bid`        varchar(50) not null,
    -- 商品id
    `gid`        int unsigned not null,
    -- 商品数量
    `count`      int unsigned not null,
    -- --------------------------------------
    primary key(`bid`,`gid`),
    foreign key(`bid`) references `info_indent`(`bid`),
    foreign key(`gid`) references `info_foods`(`gid`)
)engine=innodb default charset=utf8;

-- 对已完成订单的评价
create table `info_indentreply`(
    -- 回复号
    `rid`        int unsigned not null auto_increment,
    -- 回复内容
    `content`    varchar(2048) not null,
    -- 回复时间
    `rtime`      datetime not null,
    -- 回复状态 1:正常 2:已删除
    `status`     tinyint unsigned not null default 1,
    -- 回复人
    `uid`        int unsigned not null,
    -- 回复订单号
    `bid`        varchar(50) not null,
    -- -------------------------------------
    primary key(`rid`),
    foreign key(`uid`) references `info_user`(`uid`),
    foreign key(`bid`) references `info_indent`(`bid`)
)engine=innodb default charset=utf8;
