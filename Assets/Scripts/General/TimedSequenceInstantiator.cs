using UnityEngine;
using System.Collections;

public class TimedSequenceInstantiator : MonoBehaviour {

    public GameObject ObjectToInstantiate;
    public float timeBetweenInstantiate;
    public int timesToInstantiate;

    public void Start()
    {
        
        StartCoroutine(InstantiateObjects());
    }


    IEnumerator InstantiateObjects()
    {
 
        for (int i = 0; i < timesToInstantiate; i++)
            {
            var instantiatedObject=Instantiate(ObjectToInstantiate) as GameObject;
        
            instantiatedObject.tag = tag;
        
            instantiatedObject.transform.parent = transform;
            instantiatedObject.transform.localPosition= new Vector3(0, 0, 0);
            instantiatedObject.transform.localRotation = Quaternion.identity;
           
            yield return new WaitForSeconds(timeBetweenInstantiate);
            }
          
    }
}
