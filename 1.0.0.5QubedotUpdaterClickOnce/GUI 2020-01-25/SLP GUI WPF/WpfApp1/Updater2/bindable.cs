using System;
using System.ComponentModel;
using System.Runtime;

namespace System.ComponentModel
{
    //
    // Zusammenfassung:
    //     Gibt an, wann eine Komponenteneigenschaft an eine Anwendungseinstellung gebunden
    //     werden kann.
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingsBindableAttribute : Attribute
    {
        //
        // Zusammenfassung:
        //     Gibt an, dass eine Eigenschaft zum Binden von Eigenschaften geeignet ist.
        public static readonly SettingsBindableAttribute Yes;
        //
        // Zusammenfassung:
        //     Gibt an, dass eine Eigenschaft nicht zum Binden von Einstellungen geeignet ist.
        public static readonly SettingsBindableAttribute No;

        //
        // Zusammenfassung:
        //     Initialisiert eine neue Instanz der System.ComponentModel.SettingsBindableAttribute-Klasse.
        //
        // Parameter:
        //   bindable:
        //     true, um anzugeben, dass eine Eigenschaft zum Binden von Einstellungen geeignet
        //     ist, andernfalls false.
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SettingsBindableAttribute(bool bindable);

        //
        // Zusammenfassung:
        //     Ruft einen Wert ab, der angibt, ob eine Eigenschaft zum Binden von Einstellungen
        //     geeignet ist.
        //
        // Rückgabewerte:
        //     true, wenn die Eigenschaft zum Binden von Einstellungen geeignet ist, andernfalls
        //     false.
        public bool Bindable { get; }

        //
        // Zusammenfassung:
        //     Gibt einen Wert zurück, der angibt, ob diese Instanz gleich einem angegebenen
        //     Objekt ist.
        //
        // Parameter:
        //   obj:
        //     Ein System.Object für den Vergleich mit dieser Instanz oder ein Nullverweis (Nothing
        //     in Visual Basic).
        //
        // Rückgabewerte:
        //     true, wenn obj dem Typ und dem Wert dieser Instanz entspricht, andernfalls false.
        public override bool Equals(object obj);
        //
        // Zusammenfassung:
        //     Gibt den Hashcode für diese Instanz zurück.
        //
        // Rückgabewerte:
        //     Ein 32-Bit-Hashcode als ganze Zahl mit Vorzeichen.
        public override int GetHashCode();
    }
}