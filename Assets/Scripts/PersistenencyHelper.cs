using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface PersistenencyHelper {

    bool SaveUnits(List<Unit> units);
    List<Unit> LoadUnits();
    bool SaveGeneralData(int turncount, int mutationstate, int mutationDirection);
    List<int> LoadGeneralData();


}
