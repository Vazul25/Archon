using UnityEngine;
using System.Collections;

public class Poison : MonoBehaviour {
    [HideInInspector]
    public int durationInSeconds;
    [HideInInspector]
    public int dmgPerSec;
    [HideInInspector]
    public int secElapsed;
    [HideInInspector]
    public Unit target;
    [HideInInspector]
    public bool ticking;
 

    // Update is called once per frame
    public void StartPoisoning()
    {

        StartCoroutine(Poisoning());
    }
    IEnumerator Poisoning() {

        while (true)
        {
            Debug.Log("Poison Ticcked on " + target.tag);
            if (secElapsed < durationInSeconds)
            {

                target.InflictDamage(dmgPerSec);
                if (target.hp <= 0)
                {

                    target.participateInBattle = false;
                    StartEnding();
                }
                secElapsed++;
                yield return new WaitForSeconds(1);

            }
            else {
                ticking = false;
                yield return new WaitUntil(() => ticking);
            }
           
           
           
        }
    }
    void StartEnding()
    {


        var controller = GameObject.FindWithTag("ArenaController").GetComponent<ArenaController>();
        controller.StartEnding();
    }
}
