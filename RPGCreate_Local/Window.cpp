#include "Window.h"
#include <Windows.h>

LRESULT WindowProcdure(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) 
{
	switch (message)
	{
	case WM_DESTROY:
		// OSに対して終了を伝える
		PostQuitMessage(0);
		break;
	default:
		// メッセージのデフォルト処理
		return DefWindowProc(hWnd, message, wParam, lParam);
		break;
	}
}


bool Window::Create(int clientWidth, int clientHeight, const std::wstring& titleName, const std::wstring& windowClassName)
{
	// インスタンスハンドル取得
	HINSTANCE hInst = GetModuleHandle(0);

	// ウィンドウクラスの定義
	WNDCLASSEX wc = {};
	wc.cbSize = sizeof(WNDCLASSEX);					// 構造体のサイズ
	wc.lpfnWndProc = (WNDPROC)DefWindowProc;		// ウィンドウ関数
	wc.lpszClassName = windowClassName.c_str();		// ウィンドウクラス名
	wc.hInstance = hInst;							// インスタンスハンドル

	// ウィンドウクラスの登録
	if (!RegisterClassEx(&wc)) 
	{
		return false;
	}

	// ウィンドうの作成
	m_hWnd = CreateWindow(
		windowClassName.c_str(),				// ウィンドウのクラス名
		titleName.c_str(),						// ウィンドウのタイトル
		WS_OVERLAPPEDWINDOW - WS_THICKFRAME,	// ウィンドウタイプは標準タイプ
		0,										// ウィンドウの位置(X座標)
		0,										// ウィンドウの位置(Y座標)
		clientWidth,							// ウィンドウの幅
		clientHeight,							// ウィンドウの高さ
		nullptr,								// 親ウィンドウのハンドル
		nullptr,								// メニューのハンドル
		hInst,									// インスタンスハンドル
		this
		);

	if (m_hWnd == nullptr) 
	{
		return false;
	}

	// ウィンドウの表示
	ShowWindow(m_hWnd, SW_SHOW);

	// ウィンドウの更新
	UpdateWindow(m_hWnd);

	return true;
}

bool Window::ProcessMessage()
{
	// メッセージ取得
	MSG msg;
	while (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE)) 
	{
		if (msg.message == WM_QUIT) 
		{
			return false;
		}
		// メッセージ処理
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
	return true;
}