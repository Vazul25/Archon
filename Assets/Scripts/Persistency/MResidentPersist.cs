using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Models;

/// <summary>
/// A memóriában történő mentést megvalósító osztály </summary> 

public class MResidentPersist : PersistenencyHelper
{


    PersistencyModel persistingData;
    /// <summary>
    /// Menti az átadott generikus értékeket, egységek adatait  és igazzal tér vissza ha sikerült</summary>
    /// <param name="turncount">Az aktuális kör száma</param>
    /// <param name="mutationstate">Az aktuális változó mezők állapota</param>
    /// <param name="mutationDirection">Az aktuális változó mezők változásának iránya</param> 
    /// <param name="units">A menteni kivánt egységek adatai</param>
    public bool SaveAll(PersistencyModel toSave)
    {
        persistingData = toSave;
        Debug.Log("Save all called " + persistingData);
        return true;
    }


    public PersistencyModel LoadAll()
    {
        return persistingData;

    }
}
