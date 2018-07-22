using UnityEngine;
using System.Collections;
using Assets.Scripts.Helper;

public class InstantiateOnActRelease : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    private bool released = false;
  
    void Update()
    {


        if (gameObject.tag != "Untagged" && released == false)
        {
            if (!Input.GetButton(InputSettings.getAction1Button(gameObject.tag.Substring(0, 7))))
            {
                gameObject.SetActive(false);
                var newGameObject= Instantiate(prefabToInstantiate, gameObject.transform.localPosition, gameObject.transform.localRotation) as GameObject;
                
                RetagHierarchy.Retag(newGameObject.transform, gameObject.tag);
                Destroy(gameObject);
            }
         
        }


    }
}
