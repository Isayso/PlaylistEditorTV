// GNU GENERAL PUBLIC LICENSE                       Version 3, 29 June 2007
using System;
using System.Runtime.InteropServices;

namespace PlaylistEditor
{
    class NativeMethods
    {

        // DLL libraries used to manage hotkeys http://frasergreenroyd.com/c-global-keyboard-listeners-implementation-of-key-hooks/
        //https://ourcodeworld.com/articles/read/573/how-to-register-a-single-or-multiple-global-hotkeys-for-a-single-key-in-winforms
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        

    }
}
