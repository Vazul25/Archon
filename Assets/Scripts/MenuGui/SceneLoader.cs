using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public   class SceneLoader  : MonoBehaviour
{


    public  void LoadSceneById(int id)
    {
        SceneManager.LoadScene(id);

    }
}
