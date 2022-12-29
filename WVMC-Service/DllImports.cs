using System.Runtime.InteropServices;

namespace WVMC_Service
{
    public class DllImports
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);
        
        public const uint WM_QUIT = 0x0012;
    }
}

