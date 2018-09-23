import sys
import shutil
import os
import subprocess
import re
import io

# 查找当前文件下面指定字符的路径


def findFile(dirname, fileName):
    file_tupe = os.walk(dirname)
    for root, dirs, file_names in file_tupe:
        for file_name in file_names:
            if(file_name.find(fileName) != -1):
                return os.path.join(root, file_name)
    return None

# 添加版本


def addVersion(version):
    t_version = str(version).split(".")
    t_version[3] = str(int(t_version[3])+1)
    return ".".join(t_version)


source = "http://localhost:9000/nuget"

# 获取项目路径
project_path = sys.argv[1]
project_spit = project_path.split('\\')
# 获取项目名称
project_name = project_spit[len(project_spit)-2]
project_file = findFile(project_path, project_name+".csproj")
# 查询指定的包的版本
listPackStr = "nuget list %s -source %s" % (project_name, source)
# 读取执行命令后的返回数据
p = subprocess.Popen(listPackStr,  shell=True, stdout=subprocess.PIPE)
p.wait()
out = p.stdout.read()
# 返回中文乱码 使用一下方法解决
# print(out.decode("gbk"))
old_version_re = re.search("(\d.){3}\d", str(out))
if(old_version_re == None):
    pack_version = "1.0.0.1"
else:
    pack_version = addVersion(old_version_re.group())

exeStr = "dotnet pack -c Release %s /p:Version=%s" % (
    project_file, pack_version)
os.system(exeStr)
nugetPacPath = findFile(project_path, project_name+".%s.nupkg" % pack_version)
uploadStr = "nuget push %s 123456 -Source %s " % (nugetPacPath, source)
os.system(uploadStr)
