# !/bin/bash
# 备份的天数
daysOfBackup=31
# 备份路径
pathOfBackup=/home/data/mysqlDbBackup
# 日期
de=`date +%Y-%m-%d-%H-%M-%S`
# 备份工具
tool=mysqldump
# 数据库用户
userName=******
password=********
# 要备份的数据库
declare -a databases

databases[0]=admindb
databases[1]=adminaccount
databases[2]=iotdb
databases[3]=iotaccount

for name in ${databases[@]}
do
# -d 检查FILE是否存在并且它是一个目录
pathOfBackup=/Users/zhengguo/data/mysqlDbBackup/${name}
if [ ! -d $pathOfBackup ];
then
    # -p 目录名称存在
    mkdir -p $pathOfBackup;
fi

$tool -u $userName -p$password ${name} > $pathOfBackup/${name}_$de.sql

# 删除旧备份，查询最旧的备份
fileToDelete=`ls -l -crt $pathOfBackup/*.sql | awk '{print $9}' | head -1`

# 行数
count=`ls -l -crt $pathOfBackup/*.sql | awk '{print $9}' | wc -l`
if [ $count -ge $daysOfBackup ]
then
    rm $fileToDelete
fi
done