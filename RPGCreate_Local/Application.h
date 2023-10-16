#pragma once

#include "Window.h"

class Application
{
public:
	void Excute();
private:
	Window m_Window;
	Application() {}
public:
	static Application& Instance() 
	{
		static Application instance;
		return instance;
	}
};