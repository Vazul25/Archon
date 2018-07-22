using UnityEngine;
using System.Collections;
using Assets.Scripts;
/// <summary>
/// Statikus osztály ami az aktuálissan használt perzisztencia kezelést fedi el, állítja be.
/// Ezen felül tartalmaz segéd változókat amik játéktér váltás esetén segítenek az adatok betöltésében</summary>
public static class PersistencyManager
{
    /// <summary>
    /// Játék állapotot tartalmazza, ebből tudja mikor létrejön az új gamecontroller, hogy kell-e betöltenie régi adatokat vagy sem</summary>
    public static int stateNumber = 0;
    /// <summary>
    /// A sakktáblán annak a mezőnek a helye, ahol a harc folyt, ide kell a gyöztes egység prefabját ujra létrehozni</summary>
    public static Vector3 battleLocation;
    /// <summary>
    /// A győztes egység élet pontja, ezzel kell felülírni, az új prefab hp változóját</summary>
    public static int hp;
    /// <summary>
    /// A győztes egység típusa </summary>
    public static string prefabType;

    /// <summary>
    /// A jelenleg használt perzisztenciáért felelős objektum</summary>
    public static PersistenencyHelper CurrentPestistencyManager = new MResidentPersist();
    /// <summary>
    /// A jelenleg használt perzisztenciáért felelős objektumot adja vissza</summary>
    public static PersistenencyHelper GetCurrentPersistor() { return CurrentPestistencyManager; }

    /// <summary>
    /// Kicseréli az aktuálissan használt pezisztenciáért felelős osztályt, fileba perszitálóra.
    /// </summary>
    /// <param name="path">A path ahova menti az adatok (későbbiekben ez conncetion string is lehetne)</param>
    public static void UseLongTermPersist(string path)
    {
        CurrentPestistencyManager = new FilePersist { path = path };

    }
    public static void UseShortTermPersist()
    {
        if (!(CurrentPestistencyManager is MResidentPersist))
            CurrentPestistencyManager = new MResidentPersist();
    }
}
