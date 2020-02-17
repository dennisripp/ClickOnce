using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Updater2.Windows
{
    //
    // Zusammenfassung:
    //     Gibt Konstanten an, die definieren, welche Schaltflächen in System.Windows.Forms.MessageBox
    //     angezeigt werden.
    public interface IWin32Window
    {
        IntPtr Handle { get; }
    }
    public enum MessageBoxButtons
    {
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält die Schaltfläche OK.
        OK = 0,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält die Schaltflächen OK und Abbrechen.
        OKCancel = 1,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält die Schaltflächen Abbrechen, Wiederholen und Ignorieren.
        AbortRetryIgnore = 2,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält die Schaltflächen Ja, Nein und Abbrechen.
        YesNoCancel = 3,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält die Schaltflächen Ja und Nein.
        YesNo = 4,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält die Schaltflächen Wiederholen und Abbrechen.
        RetryCancel = 5
    }
    //
    // Zusammenfassung:
    //     Gibt Konstanten an, die definieren, welche Informationen angezeigt werden.
    public enum MessageBoxIcon
    {
        //
        // Zusammenfassung:
        //     Das Nachrichtenfeld enthält keine Symbole.
        None = 0,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus einem weißen X in einem Kreis mit
        //     rotem Hintergrund besteht.
        Hand = 16,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus einem weißen X in einem Kreis mit
        //     rotem Hintergrund besteht.
        Stop = 16,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus einem weißen X in einem Kreis mit
        //     rotem Hintergrund besteht.
        Error = 16,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol mit einem Fragezeichen in einem Kreis. Das
        //     Fragezeichen-Nachrichtensymbol wird nicht mehr empfohlen, da es keinen bestimmten
        //     Nachrichtentyp eindeutig darstellt. Außerdem kann die Formulierung einer Nachricht
        //     als Frage auf alle Nachrichtentypen angewendet werden. Darüber hinaus können
        //     Benutzer das Fragezeichensymbol mit dem Symbol für Hilfeinformationen verwechseln.
        //     Verwenden Sie dieses Fragezeichen-Nachrichtensymbol daher nicht in Ihren Nachrichtenfeldern.
        //     Das System unterstützt seine Einbindung nur aus Gründen der Abwärtskompatibilität.
        Question = 32,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus einem Ausrufezeichen in einem Dreieck
        //     mit gelben Hintergrund besteht.
        Exclamation = 48,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus einem Ausrufezeichen in einem Dreieck
        //     mit gelben Hintergrund besteht.
        Warning = 48,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus dem Kleinbuchstaben „i“ in einem
        //     Kreis besteht.
        Asterisk = 64,
        //
        // Zusammenfassung:
        //     Das Meldungsfeld enthält ein Symbol, das aus dem Kleinbuchstaben „i“ in einem
        //     Kreis besteht.
        Information = 64
    }
    public enum MessageBoxDefaultButton
    {
        //
        // Zusammenfassung:
        //     Die erste Schaltfläche im Meldungsfeld ist die Standardschaltfläche.
        Button1 = 0,
        //
        // Zusammenfassung:
        //     Die zweite Schaltfläche im Meldungsfeld ist die Standardschaltfläche.
        Button2 = 256,
        //
        // Zusammenfassung:
        //     Die dritte Schaltfläche im Meldungsfeld ist die Standardschaltfläche.
        Button3 = 512
    }
    
}