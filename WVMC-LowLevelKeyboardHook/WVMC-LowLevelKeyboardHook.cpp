// WVMC-LowLevelKeyboardHook.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include <Windows.h>

int main(int argc, char* args[])
{
    std::cout << "Hello C#! Number of arguments: " << argc << std::endl;

    for (int i = 0; i < argc; i++) {
        std::string argument = args[i];
        std::cout << "Arg " << i << ": " << argument << std::endl;
    }

    const int handle = std::atoi(args[1]);
    const auto h_handle = reinterpret_cast<void*>(handle);  // NOLINT(performance-no-int-to-ptr)
    
    char message[100];
    
    ReadFile(h_handle, message, 100, nullptr, nullptr);

    std::string message_from_host = message;
    std::cout << "Received message from C#: " << message << std::endl;
}

