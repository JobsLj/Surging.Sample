# Surging Sample

## 开发与运行环境
### IDE
- Visual Stadio 2017/Visual Stadio 2019
- Visual Stadio Core

### Docker 和 Docker-Compose
1. 通过[docker官网](https://hub.docker.com/editions/community/docker-ce-desktop-windows)下载并安装docker for windows,安装完
2. 将docker的容器类型设置为`linux`(电脑右下角鼠标右键点击`Switch to Linux Containers`)
3. 将当前源代码目录所在卷设置为docker的共享卷
4. 将docker的镜像仓库设置为`https://registry.docker-cn.com`
5. 安装docker-compose

## 运行项目

### 1. 打包Surging组件
获取源代码后，进入到nuget目录，通过`pack.ps1`的脚本打包Surging组件
```
cd ./nuget
./pack.ps1
```

### 2. 运行中间件
进入到中间件编排目录,通过docker-compose运行中间件组件
```
cd ./docker-compose/middleware
docker-compose up -d
```
### 3. 执行数据库脚本
使用mysql数据库管理工具nacicat(或是其他数据链接工具),链接到数据库服务，并执行`sql`目录下的数据库脚本,数据库链接的配置如下：
```
hostname: 127.0.0.1
port： 13306
username: root/surging
password: Surging!P4ss

```
### 4. 运行项目
通过visual Stadio打开`Surging.Hl.sln`解决方案,将`docker-compose`项目设置为启动项目，按`F5`启动项目。

## 微服务组件

| 微服务名称 | 说明 |  端口号 | 维护人  | 新增日期 | 备注  |
|:---------|:------|:-------|:------|:-------|:---------|
| Customer | 客户关系管理服务组件 | 18080 | * | * | |
| Identity | 身份认证与授权服务组件 | 18081 | * | * | 用于微服务集群身份认证与授权的服务组件 |
| Order | 订单管理服务组件 | 18082 | * | * |  |
| Product | 产品管理服务组件 | 18083 | * | * | |
| Schedule | 分布式任务调度管理组件 | 18084 | * | * | |
| Stock | 库存管理服务组件 | 18085 | * | * | |
| SystemManager | 系统管理服务组件 | 18086 | * | * | |
