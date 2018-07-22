using UnityEngine;
using System.Collections;

public class AttachPoison : MonoBehaviour
{
    public int durationInSeconds;
    public int dmgPerSec;
    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        string enemyTag;
        if (tag.Contains("Player1")) enemyTag = "Player2";
        else enemyTag = "Player1";
        if (enemyTag==other.gameObject.tag)
        {

            GameObject player = other.gameObject;
            var poison = player.GetComponent<Poison>();
            if (poison == null)
            {
                Debug.Log(player.tag + " new Poison ");
                var newPoison = player.AddComponent<Poison>();
                newPoison.secElapsed = 0;
                newPoison.durationInSeconds = durationInSeconds;
                newPoison.dmgPerSec = dmgPerSec;
                newPoison.target = player.GetComponent<Unit>();
                newPoison.StartPoisoning();

            }
            else {
                Debug.Log(player.tag + " was alredy poisoned");
                poison.secElapsed = 0;
                poison.dmgPerSec = dmgPerSec;
                poison.durationInSeconds = durationInSeconds;
                poison.ticking = true;
            }
        }
       
    }
   
}
