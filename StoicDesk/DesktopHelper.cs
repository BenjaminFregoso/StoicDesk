using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace StoicDesk
{
    public static class DesktopHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter,
            string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam,
            uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        private const uint WM_SPAWN_WORKER = 0x052C;
        private const uint SMTO_NORMAL = 0x0000;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;

        public static void EmbedToDesktop(Window window)
        {
            var helper = new WindowInteropHelper(window);
            IntPtr hwnd = helper.Handle;

            if (hwnd == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("[DesktopHelper] hwnd es Zero, abortando");
                return;
            }

            IntPtr progman = FindWindow("Progman", null);
            System.Diagnostics.Debug.WriteLine($"[DesktopHelper] Progman = {progman}");

            // 1. Pedir a Progman que cree la ventana WorkerW
            SendMessageTimeout(progman, WM_SPAWN_WORKER, IntPtr.Zero, IntPtr.Zero,
                SMTO_NORMAL, 1000, out _);

            // 2. Buscar el WorkerW que está detrás de los iconos
            IntPtr workerw = IntPtr.Zero;
            EnumWindows((tophandle, topparamhandle) =>
            {
                IntPtr shelldll_defview = FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shelldll_defview != IntPtr.Zero)
                {
                    workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                    System.Diagnostics.Debug.WriteLine($"[DesktopHelper] Encontrado WorkerW = {workerw}");
                }
                return true;
            }, IntPtr.Zero);

            if (workerw == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("[DesktopHelper] WorkerW no encontrado, usando Progman");
                workerw = progman;
            }

            // 3. Hacer que nuestra ventana sea hija de WorkerW
            IntPtr result = SetParent(hwnd, workerw);
            System.Diagnostics.Debug.WriteLine($"[DesktopHelper] SetParent resultado = {result}, error = {Marshal.GetLastWin32Error()}");

            // 4. Colocarla al fondo y forzar visibilidad
            SetWindowPos(hwnd, new IntPtr(1),
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
        }
    }
}