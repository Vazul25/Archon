using UnityEngine;
using System.Collections;

public class Registerer : MonoBehaviour {

    // Use this for initialization
    private GameController Controller;
	void Start () {
       Controller.RegisterTile(this.gameObject);
        
	}
    void Awake() {
        Controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }
	 
}
