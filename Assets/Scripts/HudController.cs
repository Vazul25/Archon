using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
       Unit tmp= GameObject.FindWithTag(tag.Substring(0,7)).GetComponent<Unit>();
       
        tmp.RegisterHudController(this);
    }

   public void updateHP(float newHp) {
        Debug.Log("Hp updated "+newHp);
        //those who implement the HpBarViewChanger interface will get the message
        BroadcastMessage("HpChanged", newHp);

    }
}
