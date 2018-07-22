using UnityEngine;
using System.Collections;
/// <summary>
/// A hpbar gameobjecthez tartozik, figyel az életpont változását jelző üzenetre, 
/// és frissíti a hud életcsíkját az új adat szerint</summary>
public class HPbarUpdater : MonoBehaviour, HpBarViewChanger
{
    /// <summary>
    /// Az életcsík kinézetéért(szélesség) felelős komponens</summary>
    RectTransform hpbar;
    /// <summary>
    /// Inicializálást végez, lekéri a RectTransform komponenst</summary>
    void Awake()
    {
        hpbar = GetComponent<RectTransform>();
    }
    /// <summary>
    /// A HpBarViewChanger által definiált függvény, ezt a függvényt fogva meghívni a unity belső eseménykezelő rendszere,
    /// a broadcastMessage által</summary>
    public void HpChanged(float newhp)
    {
        if (newhp <= 0) hpbar.localScale = new Vector3(0, 1, 1);
        else hpbar.localScale = new Vector3(newhp / 2, 1, 1);
    }
}
