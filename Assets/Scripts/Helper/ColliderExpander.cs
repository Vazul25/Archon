using UnityEngine;
using System.Collections;

public class ColliderExpander : MonoBehaviour {
    public Vector3 speed;
    public  BoxCollider c;
	 
	
	// Update is called once per frame
	void Update () {
        c.size += speed * Time.deltaTime;
	}
}
