#include "Application.h"

#include <Windows.h>
#include <cassert>

int WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR, int) 
{
	Application::Instance().Excute();

	return 0;
}
void Application::Excute() 
{
	if(!m_Window.Create(1280,720,L"FewameworkDX12",L"Window"))
	{
		assert(0 && "ウィンドウ作成失敗");
		return;
	}
	while (true) 
	{
		if (!m_Window.ProcessMessage()) 
		{
			break;
		}
	}
}