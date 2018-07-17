using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Utility.IOs
{
    /// <summary>
    /// 这个类可以让你得到一个在运行中程序的所有键盘事件 并且引发一个带KeyEventArgs和MouseEventArgs参数的.NET事件以便你很容易使用这些信息
    /// </summary>
    public class KeyBoardHook
    {
        #region Fields
        //鼠标常量
        private const int _WH_KEYBOARD_LL = 13;
        private const int _WM_KEYDOWN = 0x100;
        private const int _WM_KEYUP = 0x101;
        private const int _WM_SYSKEYDOWN = 0x104;
        private const int _WM_SYSKEYUP = 0x105;
        //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13
        private static int hKeyboardHook; //声明键盘钩子处理的初始值
        //先前按下的键
        private readonly List<Keys> preKeys = new List<Keys>();
        private HookProc _keyboardHookProcedure;
        #endregion

        #region Constructors
        //声明键盘钩子事件类型.
        /// <summary>
        /// 默认的构造函数构造当前类的实例并自动的运行起来.
        /// </summary>
        public KeyBoardHook()
        {
            Start();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~KeyBoardHook()
        {
            Stop();
        }
        #endregion

        #region Events
        //全局的事件
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        public event KeyEventHandler OnKeyDownEvent;
        /// <summary>
        /// 键盘输入事件
        /// </summary>
        public event KeyPressEventHandler OnKeyPressEvent;
        /// <summary>
        /// 键盘释放事件
        /// </summary>
        public event KeyEventHandler OnKeyUpEvent;
        #endregion

        #region Methods
        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        public void Start()
        {
            if (hKeyboardHook != 0)
                return;

            this._keyboardHookProcedure = KeyboardHookProc;
            //hKeyboardHook = SetWindowsHookEx(_WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            //************************************
            //键盘线程钩子
            //SetWindowsHookEx( 2,KeyboardHookProcedure, IntPtr.Zero, GetCurrentThreadId());//指定要监听的线程idGetCurrentThreadId(),
            //键盘全局钩子,需要引用空间(using System.Reflection;)
            //SetWindowsHookEx( 13,MouseHookProcedure,Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),0);
            //
            //关于SetWindowsHookEx (int idHook, HookProc lpfn, IntPtr hInstance, int threadId)函数将钩子加入到钩子链表中，说明一下四个参数：
            //idHook 钩子类型，即确定钩子监听何种消息，上面的代码中设为2，即监听键盘消息并且是线程钩子，如果是全局钩子监听键盘消息应设为13，
            //线程钩子监听鼠标消息设为7，全局钩子监听鼠标消息设为14。lpfn 钩子子程的地址指针。如果dwThreadId参数为0 或是一个由别的进程创建的
            //线程的标识，lpfn必须指向DLL中的钩子子程。 除此以外，lpfn可以指向当前进程的一段钩子子程代码。钩子函数的入口地址，当钩子钩到任何
            //消息后便调用这个函数。hInstance应用程序实例的句柄。标识包含lpfn所指的子程的DLL。如果threadId 标识当前进程创建的一个线程，而且子
            //程代码位于当前进程，hInstance必须为NULL。可以很简单的设定其为本应用程序的实例句柄。threaded 与安装的钩子子程相关联的线程的标识符
            //如果为0，钩子子程与所有的线程关联，即为全局钩子
            //************************************
            Process curProcess = Process.GetCurrentProcess();
            ProcessModule curModule = curProcess.MainModule;
            hKeyboardHook = SetWindowsHookEx(_WH_KEYBOARD_LL, this._keyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            if (hKeyboardHook != 0)
                return;

            Stop();
            throw new Exception("SetWindowsHookEx ist failed.");
        }

        /// <summary>
        /// 卸载键盘钩子
        /// </summary>
        public void Stop()
        {
            var retKeyboard = true;
            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            //如果卸下钩子失败
            if (!retKeyboard)
                throw new Exception("UnhookWindowsHookEx failed.");
        }

        //下一个钩挂的函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "GetKeyboardState")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private static bool IsCtrlAltShiftKeys(Keys key) =>
            key == Keys.LControlKey || key == Keys.RControlKey || key == Keys.LMenu || key == Keys.RMenu || key == Keys.LShiftKey || key == Keys.RShiftKey;

        //装置钩子的函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //[DllImport("user32", EntryPoint = "GetKeyNameText")]
        //private static extern int GetKeyNameText(int param, StringBuilder lpBuffer, int nSize);
        [DllImport("user32", EntryPoint = "ToAscii")]
        private static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        //卸下钩子的函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        private Keys GetDownKeys(Keys key)
        {
            var rtnKey = Keys.None;
            foreach (Keys keyTemp in this.preKeys)
                switch (keyTemp)
                {
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        rtnKey = rtnKey | Keys.Control;
                        break;
                    case Keys.LMenu:
                    case Keys.RMenu:
                        rtnKey = rtnKey | Keys.Alt;
                        break;
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        rtnKey = rtnKey | Keys.Shift;
                        break;
                    default: break;
                }

            rtnKey = rtnKey | key;
            return rtnKey;
        }

        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0 || OnKeyDownEvent == null && OnKeyUpEvent == null && OnKeyPressEvent == null)
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);

            var myKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
            //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
            //中的对应的键增加
            if ((OnKeyDownEvent != null || OnKeyPressEvent != null) && (wParam == _WM_KEYDOWN || wParam == _WM_SYSKEYDOWN))
            {
                var keyData = (Keys)myKeyboardHookStruct.vkCode;
                if (IsCtrlAltShiftKeys(keyData) && this.preKeys.IndexOf(keyData) == -1)
                    this.preKeys.Add(keyData);
            }

            //引发OnKeyDownEvent
            if (OnKeyDownEvent != null && (wParam == _WM_KEYDOWN || wParam == _WM_SYSKEYDOWN))
            {
                var keyData = (Keys)myKeyboardHookStruct.vkCode;
                OnKeyDownEvent(this, new KeyEventArgs(GetDownKeys(keyData)));
            }

            //引发OnKeyPressEvent
            if (OnKeyPressEvent != null && wParam == _WM_KEYDOWN)
            {
                var keyState = new byte[256];
                GetKeyboardState(keyState);
                var inBuffer = new byte[2];
                if (1 == ToAscii(myKeyboardHookStruct.vkCode, myKeyboardHookStruct.scanCode, keyState, inBuffer, myKeyboardHookStruct.flags))
                    OnKeyPressEvent(this, new KeyPressEventArgs((char)inBuffer[0]));
            }

            //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
            //中的对应的键删除
            if ((OnKeyDownEvent != null || OnKeyPressEvent != null) && (wParam == _WM_KEYUP || wParam == _WM_SYSKEYUP))
            {
                var keyData = (Keys)myKeyboardHookStruct.vkCode;
                if (IsCtrlAltShiftKeys(keyData))
                    for (int i = this.preKeys.Count - 1; i >= 0; i--)
                        if (this.preKeys[i] == keyData)
                            this.preKeys.RemoveAt(i);
            }

            //引发OnKeyUpEvent
            if (OnKeyUpEvent == null || wParam != _WM_KEYUP && wParam != _WM_SYSKEYUP)
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);

            {
                var keyData = (Keys)myKeyboardHookStruct.vkCode;
                OnKeyUpEvent(this, new KeyEventArgs(GetDownKeys(keyData)));
            }

            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
        #endregion

        #region Nested type: HookProc
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        #endregion

        #region Nested type: KeyboardHookStruct
        /// <summary>
        /// 声明键盘钩子的封送结构类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            #region Fields
            /// <summary>
            /// 指定额外信息相关的信息
            /// </summary>
            public int dwExtraInfo;
            /// <summary>
            /// 键标志
            /// </summary>
            public int flags;
            /// <summary>
            /// 表示硬件扫描码
            /// </summary>
            public int scanCode;
            /// <summary>
            /// 指定的时间戳记的这个讯息
            /// </summary>
            public int time;
            /// <summary>
            /// 表示一个在1到254间的虚似键盘码
            /// </summary>
            public int vkCode;
            #endregion
        }
        #endregion
    }
}
