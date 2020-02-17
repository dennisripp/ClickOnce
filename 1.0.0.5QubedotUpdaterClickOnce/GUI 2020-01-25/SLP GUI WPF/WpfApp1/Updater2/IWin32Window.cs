using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WpfApp1.Updater2
{
    //
    // Zusammenfassung:
    //     Stellt eine Schnittstelle bereit, um Win32-HWND-Handles verfügbar zu machen.
    [ComVisible(true)]
    [Guid("458AB8A2-A1EA-4d7b-8EBE-DEE5D3D9442C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWin32Window
    {
        //
        // Zusammenfassung:
        //     Ruft das Handle für das Fenster ab, das von der Implementierung dargestellt wird.
        //
        // Rückgabewerte:
        //     Ein Handle für das Fenster, das von der Implementierung dargestellt wird.
        IntPtr Handle { get; }
    }
}