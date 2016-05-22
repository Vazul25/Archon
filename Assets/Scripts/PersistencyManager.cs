using UnityEngine;
using System.Collections;

public static class PersistencyManager {
   public static int stateNumber=0;
    public static Vector3 battleLocation;
    public static int hp;
    public static string prefabType;
    public static PersistenencyHelper CurrentPestistencyManager = new MResidentPersist();
   public static PersistenencyHelper GetCurrentPersistor() { return CurrentPestistencyManager; }
     
}
