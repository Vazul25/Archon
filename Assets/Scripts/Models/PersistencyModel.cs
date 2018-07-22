using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    [Serializable()]
    public class PersistencyModel
    {
        public List<UnitData> units;
        public GeneralData generalData;

    }
    [Serializable()]
    public class GeneralData
    {
        public int turncount;
        public int mutationState;
        public int mutationDirection;
    }
    [Serializable()]
    public class UnitData
    {
        public int hp;
      
        public float coordx;
        public float coordy;
        public float coordz;
        public string prefabType;
        public override string ToString() { return String.Format("hp:{0} coords:{1} {2} {3} prefabType:{4}", hp, coordx, coordy, coordz, prefabType); }
    }
}
