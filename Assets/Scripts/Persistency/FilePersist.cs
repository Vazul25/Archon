using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
namespace Assets.Scripts
{
    class FilePersist : PersistenencyHelper
    {
        public string path;
        public PersistencyModel LoadAll()
        {
           

            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.Read,
                                       FileShare.Read))
            {
                Debug.Log(path);
                PersistencyModel toSave = (PersistencyModel)formatter.Deserialize(stream);
               
                stream.Close();
                 
                
                return toSave;
            }
           
        }

        public bool SaveAll(PersistencyModel toSave)
        {
           Debug.Log(String.Format("path={4} mutationState={0}  mutatuibTurn={1} turn={2}  firstUnit={3}", toSave.generalData.mutationState, toSave.generalData.mutationDirection, toSave.generalData.turncount, toSave.units[0].ToString(),path));
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (var stream = new FileStream(path,
                                          FileMode.Create,
                                          FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, toSave);
                     stream.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
