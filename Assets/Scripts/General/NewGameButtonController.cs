using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewGameButtonController : MonoBehaviour {

    public void NewGame()
    {
        PersistencyManager.stateNumber = 0;
        SceneManager.LoadScene(1);

    }
}
