using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadButtonController : MonoBehaviour
{



    public   void LoadOnClick()
    {
        var text = GetComponentInChildren<UnityEngine.UI.Text>();
        var path=Path.Combine(Path.Combine(Application.dataPath, "Saves").Replace('/', Path.DirectorySeparatorChar), text.text);
        Debug.Log("Loadin " + path);
        PersistencyManager.stateNumber = 1;
        PersistencyManager.UseLongTermPersist(path);
        SceneManager.LoadScene(2);
    }


}
