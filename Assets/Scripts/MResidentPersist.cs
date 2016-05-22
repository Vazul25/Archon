using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public  class MResidentPersist : PersistenencyHelper {
    List<int> persistingGeneralData;
    List<Unit> persistingUnits;

    public List<int> LoadGeneralData()
    {
        return persistingGeneralData;
        Debug.Log("LoadGeneralData called with: " + persistingGeneralData[0] + " " + persistingGeneralData[1] + " " + persistingGeneralData[2]);
    }

    public List<Unit> LoadUnits()
    {
        return persistingUnits;
    }

    public bool SaveGeneralData(int turncount, int mutationstate, int mutationDirection)
    {
        persistingGeneralData = new List<int>();
        persistingGeneralData.Add(turncount);
        persistingGeneralData.Add(mutationstate);
        persistingGeneralData.Add(mutationDirection);
        Debug.Log("SaveGeneral Data called with: " + persistingGeneralData[0] + " " + persistingGeneralData[1] + " " + persistingGeneralData[2]);
        return true;
    }

    public bool SaveUnits(List<Unit> units)
    {

        persistingUnits = units;
        return true;
    }

 
}
