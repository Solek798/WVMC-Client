// WVMC-LowLevelKeyboardHook.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include <Windows.h>

bool get_handle(int argc, char* args[], OUT long& handle);
LRESULT CALLBACK low_level_keyboard_proc(int code, WPARAM wparam, LPARAM lparam);
void send_quit_confirmation();

void* h_handle;


int main(const int argc, char* args[])
{
    std::cout << "Hello C#! Number of arguments: " << argc << std::endl;

    for (int i = 0; i < argc; i++) {
        std::string argument = args[i];
        std::cout << "Arg " << i << ": " << argument << std::endl;
    }

    long handle;
    const bool is_handle_valid = get_handle(argc, args, handle);

    if (!is_handle_valid)
    {
        std::cerr << "No valid handle! Abort!" << std::endl;
        return EXIT_FAILURE;
    }

    std::cout << "Handle: " << handle << std::endl;
    
    h_handle = reinterpret_cast<void*>(handle);  // NOLINT(performance-no-int-to-ptr)

    HHOOK hook = SetWindowsHookEx(WH_KEYBOARD_LL, &low_level_keyboard_proc, nullptr, 0);


    // Message-Pump
    MSG message;
    while (GetMessage(&message, nullptr, NULL, NULL) > 0)
    {
        TranslateMessage(&message);
        DispatchMessage(&message);
    }

    std::cout << "Hook is stopping!" << std::endl;
    UnhookWindowsHookEx(hook);

    send_quit_confirmation();

    return EXIT_SUCCESS;
}

bool get_handle(const int argc, char* args[], OUT long& handle)
{
    if (argc < 2)
        return false;

    // clear errno
    errno = 0;
    
    char* conv_end_pos;
    const long result = std::strtol(args[1], &conv_end_pos, 0);

    if (errno == ERANGE || result == 0)
        return false;

    handle = result;
    return true;
}

LRESULT CALLBACK low_level_keyboard_proc(int code, WPARAM wparam, LPARAM lparam)
{
    const auto hook_struct = reinterpret_cast<KBDLLHOOKSTRUCT*>(lparam);  // NOLINT(performance-no-int-to-ptr)

    if (wparam == WM_KEYDOWN)
    {
        std::cout << "Key Code: " << hook_struct->vkCode << std::endl;

        const UINT ascii_code = MapVirtualKey(hook_struct->vkCode, MAPVK_VK_TO_CHAR);
        WriteFile(h_handle, &ascii_code, sizeof(UINT), nullptr, nullptr);
    }

    return CallNextHookEx(nullptr, code, wparam, lparam);
}

void send_quit_confirmation()
{
    constexpr char quit_code = 0;
    WriteFile(h_handle, &quit_code, 1, nullptr, nullptr);
}
