using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {

    public float speed;
    
    public void Start() {

    }
	// Update is called once per frame
	void Update () {
        transform.localPosition +=new Vector3(0,0,speed * Time.deltaTime) ;
	}
}
