using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WpfApp1.Updater2.Windows
{
    //
    // Zusammenfassung:
    //     Gibt Bezeichner an, die den Rückgabewert eines Dialogfelds angeben.
    [ComVisible(true)]
    public enum MessageBoxResult
    {
        //
        // Zusammenfassung:
        //     Nothing wird vom Dialogfeld zurückgegeben. Dies bedeutet, dass das modale Dialogfeld
        //     weiterhin ausgeführt wird.
        None = 0,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist OK (üblicherweise von der Schaltfläche OK
        //     gesendet).
        OK = 1,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist Cancel (üblicherweise von der Schaltfläche
        //     Abbrechen gesendet).
        Cancel = 2,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist Abort (üblicherweise von der Schaltfläche
        //     Abbrechen gesendet).
        Abort = 3,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist Retry (üblicherweise von der Schaltfläche
        //     Wiederholen gesendet).
        Retry = 4,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist Ignore (üblicherweise von der Schaltfläche
        //     Ignorieren gesendet).
        Ignore = 5,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist Yes (üblicherweise von der Schaltfläche
        //     Ja gesendet).
        Yes = 6,
        //
        // Zusammenfassung:
        //     Der Rückgabewert des Dialogfelds ist No (üblicherweise von der Schaltfläche Nein
        //     gesendet).
        No = 7
    }
    //
    // Zusammenfassung:
    //     Ruft den diesem Steuerelement zugeordneten Text ab oder legt diesen fest.
    //
    // Rückgabewerte:
    //     Der diesem Steuerelement zugeordnete Text.
    
}
