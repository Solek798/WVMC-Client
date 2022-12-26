// WVMC-LowLevelKeyboardHook.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include <Windows.h>

LRESULT CALLBACK low_level_keyboard_proc(int code, WPARAM wparam, LPARAM lparam);

void* h_handle;

int main(int argc, char* args[])
{
    std::cout << "Hello C#! Number of arguments: " << argc << std::endl;

    for (int i = 0; i < argc; i++) {
        std::string argument = args[i];
        std::cout << "Arg " << i << ": " << argument << std::endl;
    }

    const int handle = std::atoi(args[1]);
    h_handle = reinterpret_cast<void*>(handle);  // NOLINT(performance-no-int-to-ptr)
    
    //char message[100];
    
    //ReadFile(h_handle, message, 100, NULL, NULL);

    //std::string message_from_host = message;
    //std::cout << "Received message from C#: " << message << std::endl;

    //HHOOK hook = SetWindowsHookEx(WH_KEYBOARD_LL, &low_level_keyboard_proc, 0, 0);


    // Message-Pump
    MSG message;
    while (GetMessage(&message, NULL, NULL, NULL) > 0)
    {
        TranslateMessage(&message);
        DispatchMessage(&message);
    }

    std::cout << "Hook is stopping!" << std::endl;
    //UnhookWindowsHookEx(hook);

    return 0;
}

LRESULT CALLBACK low_level_keyboard_proc(int code, WPARAM wparam, LPARAM lparam)
{
    auto s = reinterpret_cast<KBDLLHOOKSTRUCT*>(lparam);  // NOLINT(performance-no-int-to-ptr)

    if (wparam == WM_KEYDOWN)
    {
        std::cout << "Key: " << s->vkCode << std::endl;
        //WriteFile(h_handle, )
    }

    return CallNextHookEx(NULL, code, wparam, lparam);
}
