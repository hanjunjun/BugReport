/*
	描述：崩溃拦截器
	作者：韩俊俊
	时间：2019-09-11
*/
#include "pch.h"
#include "BugReport.h"
#include <tchar.h>
#include <mutex>

#pragma region 全局异常拦截
//全局互斥锁
std::mutex mtx;
int _timeTick = 0;
//char* _productName;
TCHAR sproductName[MAX_PATH];
//Crash异常回调
long   __stdcall   callback(_EXCEPTION_POINTERS* excp)
{
	printf("拦截到了crash!\n");
	//同步锁，防止多个线程同时崩溃，同时进入异常回调
	mtx.lock();
	MEMORY_BASIC_INFORMATION mbi = { 0 };
	//获取异常模块路径
	WCHAR excInfo[MAX_PATH] = L"";
	TCHAR excInfoBuffer[MAX_PATH] = { 0 };
	memset(excInfoBuffer, 0, sizeof(excInfoBuffer));
	int rtQuery = VirtualQuery(excp->ExceptionRecord->ExceptionAddress, &mbi, sizeof(mbi));
	if (rtQuery) {
		UINT_PTR h_module = (UINT_PTR)mbi.AllocationBase;
		GetModuleFileNameW((HMODULE)h_module, excInfo, MAX_PATH);
		swprintf_s(excInfoBuffer, _T("\"%s\""), excInfo);
		//MessageBox(0, excInfoBuffer, L"错误", MB_OK);
	}
	//获取当前程序目录，不带文件名
	LPTSTR cWinDir = new TCHAR[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, cWinDir);
	//获取当前程序目录，带文件名
	TCHAR szPath[MAX_PATH] = { 0 };
	GetModuleFileName(NULL, szPath, MAX_PATH);

	TCHAR path[MAX_PATH] = { 0 };
	memset(path, 0, sizeof(path));
	//printf("开始获取倒计时\n");
	swprintf_s(path, _T("\"%s\""), szPath);
	//printf("获取当前程序目录：%s\n", szPath);
	//MessageBox(0, path, L"警告", MB_OK);
	//if (!(excInfo && *excInfo != '\0')) {
	//	//获取异常模块失败
	//}

	//倒计时
	TCHAR timeTick[MAX_PATH] = { 0 };
	memset(timeTick, 0, sizeof(timeTick));
	//printf("开始获取倒计时\n");
	swprintf_s(timeTick, _T("%d"), _timeTick);
	//printf("倒计时：%d\n", _timeTick);

	//产品名称_stdcall
	//TCHAR productName[MAX_PATH] = { 0 };
	//memset(productName, 0, sizeof(productName));
	//swprintf_s(productName, _T("%s"), _productName);
	////printf("%s\n", productName);
	//MessageBox(0, productName, L"警告", MB_OK);

	TCHAR excpAddress[MAX_PATH] = { 0 };
	memset(excpAddress, 0, sizeof(excpAddress));
	swprintf_s(excpAddress, _T("0x%04X"), excp->ExceptionRecord->ExceptionAddress);
	//printf("错误偏移量：%s\n", excpAddress);
	//MessageBox(0, excpAddress, L"错误", MB_OK);
//	TCHAR sproductName[MAX_PATH] = { 0 };
//#ifdef UNICODE
//	MultiByteToWideChar(CP_ACP, 0, _productName, -1, sproductName, MAX_PATH);
//#else
//	strcpy(sproductName, _productName);
//#endif
	//MessageBox(0, sproductName, L"警告", MB_OK);

	TCHAR strCmdLine[MAX_PATH] = { 0 };
	memset(strCmdLine, 0, sizeof(strCmdLine));
	//传参给BugReport 当前程序运行目录，错误模块路径，错误偏移量
	swprintf_s(strCmdLine, _T("BugReport.exe %s %s %s %s %s"), path, excInfoBuffer, excpAddress, timeTick, sproductName);
	//MessageBox(0, strCmdLine, L"警告", MB_OK);
	//printf("参数：%s\n", strCmdLine);

	//启动BugReprot.exe
	printf("启动BugReprot.exe");
	STARTUPINFO si = { 0 };
	si.cb = sizeof(si);
	PROCESS_INFORMATION pi;
	BOOL bRet = CreateProcess(
		NULL,   //  指向一个NULL结尾的、用来指定可执行模块的宽字节字符串  
		strCmdLine, // 命令行字符串  
		NULL, //    指向一个SECURITY_ATTRIBUTES结构体，这个结构体决定是否返回的句柄可以被子进程继承。  
		NULL, //    如果lpProcessAttributes参数为空（NULL），那么句柄不能被继承。<同上>  
		false,//    指示新进程是否从调用进程处继承了句柄。   
		0,  //  指定附加的、用来控制优先类和进程的创建的标  
			//  CREATE_NEW_CONSOLE  新控制台打开子进程  
			//  CREATE_SUSPENDED    子进程创建后挂起，直到调用ResumeThread函数  
		NULL, //    指向一个新进程的环境块。如果此参数为空，新进程使用调用进程的环境  
		NULL, //    指定子进程的工作路径  
		&si, // 决定新进程的主窗体如何显示的STARTUPINFO结构体  
		&pi  // 接收新进程的识别信息的PROCESS_INFORMATION结构体  
	);
	if (bRet) {
		WaitForSingleObject(pi.hProcess, INFINITE);//等待帮助进程结束
		DWORD dwExitCode;
		GetExitCodeProcess(pi.hProcess, &dwExitCode);
		printf("BugReport退出码：%s\n", dwExitCode);
	}
	printf("启动成功");
	mtx.unlock();
	return EXCEPTION_EXECUTE_HANDLER;
}

//SetUnhandledExceptionFilter打补丁，在入口处加上汇编代码 ret 4，不让后面的人注册自己的错误回调
void PatchSetUnhandledExceptionFilter()
{
	void* addr = (void*)GetProcAddress(LoadLibrary(L"kernel32.dll"),
		"SetUnhandledExceptionFilter");
	if (addr)
	{
		unsigned char code[16];
		int size = 0;

#ifdef _M_IX86
		code[size++] = 0x33;
		code[size++] = 0xC0;
		code[size++] = 0xC2;
		code[size++] = 0x04;
		code[size++] = 0x00;
#elif _M_X64
		code[size++] = 0x33;
		code[size++] = 0xC0;
		code[size++] = 0xC3;
#else
#error "The following code only works for x86 and x64!"
#endif
		DWORD dwOldFlag, dwTempFlag;
		VirtualProtect(addr, size, PAGE_READWRITE, &dwOldFlag);
		WriteProcessMemory(GetCurrentProcess(), addr, code, size, NULL);
		VirtualProtect(addr, size, dwOldFlag, &dwTempFlag);
	}
	//static BYTE RETURN_CODE[] = { 0xc2, 0x04, 0x00 }; //汇编代码：ret 4 等同 C++： return
	//MEMORY_BASIC_INFORMATION   mbi;
	//DWORD dwOldProtect = 0;
	//DWORD pfnSetFilter = (DWORD)GetProcAddress(GetModuleHandleW(L"kernel32.dll"), "SetUnhandledExceptionFilter");
	//bool virtualQuery1 = VirtualQuery((void*)pfnSetFilter, &mbi, sizeof(mbi));
	//printf("virtualQuery1=%d\n", virtualQuery1);
	//bool virtualProtectEx1 = VirtualProtectEx(GetCurrentProcess(), (void*)pfnSetFilter, sizeof(RETURN_CODE), PAGE_READWRITE, &dwOldProtect);
	//printf("virtualProtectEx1=%d\n", virtualProtectEx1);
	////PAGE_EXECUTE_READWRITE    PAGE_READWRITE
	//bool writeProcessMemory = WriteProcessMemory(GetCurrentProcess(), (void*)pfnSetFilter, RETURN_CODE, sizeof(RETURN_CODE), NULL);
	//printf("writeProcessMemory=%d\n", writeProcessMemory);
	////bool virtualProtect2 = VirtualProtect((void*)pfnSetFilter, sizeof(RETURN_CODE), mbi.Protect, 0);
	////printf("virtualProtect2=%d\n", virtualProtect2);
	//bool flushInstructionCache = FlushInstructionCache(GetCurrentProcess(), (void*)pfnSetFilter, sizeof(RETURN_CODE));
	//printf("flushInstructionCache=%d\n", flushInstructionCache);
}

bool is_str_utf8(const char* str)
{
	unsigned int nBytes = 0;//UFT8可用1-6个字节编码,ASCII用一个字节  
	unsigned char chr = *str;
	bool bAllAscii = true;

	for (unsigned int i = 0; str[i] != '\0'; ++i) {
		chr = *(str + i);
		//判断是否ASCII编码,如果不是,说明有可能是UTF8,ASCII用7位编码,最高位标记为0,0xxxxxxx 
		if (nBytes == 0 && (chr & 0x80) != 0) {
			bAllAscii = false;
		}

		if (nBytes == 0) {
			//如果不是ASCII码,应该是多字节符,计算字节数  
			if (chr >= 0x80) {

				if (chr >= 0xFC && chr <= 0xFD) {
					nBytes = 6;
				}
				else if (chr >= 0xF8) {
					nBytes = 5;
				}
				else if (chr >= 0xF0) {
					nBytes = 4;
				}
				else if (chr >= 0xE0) {
					nBytes = 3;
				}
				else if (chr >= 0xC0) {
					nBytes = 2;
				}
				else {
					return false;
				}

				nBytes--;
			}
		}
		else {
			//多字节符的非首字节,应为 10xxxxxx 
			if ((chr & 0xC0) != 0x80) {
				return false;
			}
			//减到为零为止
			nBytes--;
		}
	}

	//违返UTF8编码规则 
	if (nBytes != 0) {
		return false;
	}

	if (bAllAscii) { //如果全部都是ASCII, 也是UTF8
		return true;
	}

	return true;
}

bool is_str_gbk(const char* str)
{
	unsigned int nBytes = 0;//GBK可用1-2个字节编码,中文两个 ,英文一个 
	unsigned char chr = *str;
	bool bAllAscii = true; //如果全部都是ASCII,  

	for (unsigned int i = 0; str[i] != '\0'; ++i) {
		chr = *(str + i);
		if ((chr & 0x80) != 0 && nBytes == 0) {// 判断是否ASCII编码,如果不是,说明有可能是GBK
			bAllAscii = false;
		}

		if (nBytes == 0) {
			if (chr >= 0x80) {
				if (chr >= 0x81 && chr <= 0xFE) {
					nBytes = +2;
				}
				else {
					return false;
				}

				nBytes--;
			}
		}
		else {
			if (chr < 0x40 || chr>0xFE) {
				return false;
			}
			nBytes--;
		}//else end
	}

	if (nBytes != 0) {		//违返规则 
		return false;
	}

	if (bAllAscii) { //如果全部都是ASCII, 也是GBK
		return true;
	}

	return true;
}

//提供给外部程序调用的导出函数
void _stdcall SetGlobalExceptionFilter(int timeTick, char* productName) {
	//注册回调
	SetUnhandledExceptionFilter(callback);
	//打补丁
	PatchSetUnhandledExceptionFilter();
	//倒计时
	//int timeTick = 10;
	_timeTick = timeTick;
	//_productName = productName;
#ifdef UNICODE
	MultiByteToWideChar(CP_ACP, 0, productName, -1, sproductName, MAX_PATH);
#else
	strcpy(sproductName, _productName);
#endif
	//puts(productName);
	//MessageBox(0, sproductName, L"提示", MB_OK);
	/*bool isz= is_str_utf8(productName);
	if(isz)
		printf("is utf8\n");*/

		//printf("倒计时：%d\n", _timeTick);

		//python字符编码传过来会乱码这边转换一下
		/*isz = is_str_gbk(productName);
		if (isz)
			printf("is gbk\n");*/
			//char* szStr = productName;
			//WCHAR wszClassName[MAX_PATH];
			//memset(wszClassName, 0, sizeof(wszClassName));
			//MultiByteToWideChar(CP_ACP, 0, szStr, strlen(szStr) + 1, wszClassName,
			//	sizeof(wszClassName) / sizeof(wszClassName[0]));
			//
			////产品名称
			//_productName = wszClassName;
			//printf("产品名称：%s\n", _productName);
			//Sleep(200000);
			//MessageBox(0, L"hanjunjun", L"提示", MB_OK);
			//int* ptr = NULL;
			//*ptr = 1;
			//_asm   int   3   //只是为了让程序崩溃
			//MessageBox(0, L"加载异常拦截模块成功！", L"提示", MB_OK);
}

void _stdcall Test() {
	//MessageBox(0, L"my name is 韩俊俊", L"提示", MB_OK);
}
#pragma endregion