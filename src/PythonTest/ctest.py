# -*- coding: utf-8 -*-
import sys
from ctypes import *

print("python hellor world ")
# 注册全局异常过滤器
# C++函数原型
dll_path = fileUtil.getRootPath() + "\\BugReportLib.dll"
print("加载BugReportLib：{}".format(dll_path))
dll = windll.LoadLibrary(dll_path)
error_filter = dll.SetGlobalExceptionFilter
error_filter.argtypes = [c_int, POINTER(c_char)]  # 传入参数为字符指针
# 1.倒计时
# 2.产品名称参数
product_name = "Python测试程序"
product_name = (c_char * 100)(*bytes(product_name, 'gbk'))  # 把一组100个的字符定义为STR
cast(product_name, POINTER(c_char))
# 5秒自动重启
error_filter(15, product_name)
sys.stdin.readline()
