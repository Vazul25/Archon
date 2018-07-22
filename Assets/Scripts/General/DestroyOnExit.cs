using UnityEngine;
 

public class DestroyOnExit : MonoBehaviour {

	 
	void Update () {
        if (Input.GetKey("escape"))
            Destroy(gameObject);
    }
}
