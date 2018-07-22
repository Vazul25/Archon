/// <summary>
/// Élet megjelenítésének karbantartásához szükséges interface,
/// meghatározza, hogy milyen függvénnyel kell rendelkeznie az osztálynak,
/// hogy megkapja üzenetszórásban az eseményt</summary>
interface HpBarViewChanger
{
    /// <summary>
    /// ezt a függvényt fogva meghívni a unity belső eseménykezelő rendszere,
    /// a broadcastMessage által</summary>
    void HpChanged(float newhp);
}

