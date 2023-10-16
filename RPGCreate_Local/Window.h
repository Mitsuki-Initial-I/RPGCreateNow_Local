#pragma once

#include <Windows.h>
#include <string>

class Window 
{
public:
	/// <summary>
	/// ウィンドウ作成
	/// </summary>
	/// <param name="clientWidth">横幅</param>
	/// <param name="clienHeight">縦幅</param>
	/// <param name="titleName">タイトル名</param>
	/// <param name="windowClassName">クラス名</param>
	/// <returns>成功</returns>
	bool Create(int clientWidth, int clienHeight, const std::wstring& titleName, const std::wstring& windowClassName);
	/// <summary>
	/// ウィンドウメッセージ処理
	/// </summary>
	/// <returns>終了メッセージが来たらfalseが来る</returns>
	bool ProcessMessage();
private:
	HWND m_hWnd;	// ウィンドウハンドル
};