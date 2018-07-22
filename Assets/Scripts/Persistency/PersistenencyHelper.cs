using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;

public interface PersistenencyHelper {

    //bool SaveUnits(List<Unit> units);
    //List<Unit> LoadUnits();
    //bool SaveGeneralData(int turncount, int mutationState, int mutationDirection);
    //List<int> LoadGeneralData();

    bool SaveAll(PersistencyModel toSave);
    PersistencyModel LoadAll();
}
