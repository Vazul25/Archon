using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour
{
    public bool DestroyedOnCollision;
    // Use this for initialization
    void Start()
    {

    }
    public int dmg;
    public bool canDamage;
    void OnTriggerEnter(Collider other)
    {

        Debug.Log("On trigger:" + gameObject.tag + " other tag" + other.gameObject.tag);
        if (canDamage)
        {
            if (!gameObject.tag.Contains(other.gameObject.tag) && (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2"))
            {

                Unit tmp = other.gameObject.GetComponent<Unit>();
                if (!tmp.invulnerable)
                {
                    Debug.Log("Before:" + tmp.hp);
                    tmp.InflictDamage(dmg);

                    Debug.Log("After" + tmp.hp);
                    tmp.setInvulnerable();
                    if (tmp.hp <= 0)
                    {
                        Debug.Log("tmp.hp<0" + tmp.hp);
                        StartEnding();
                        tmp.participateInBattle = false;
                      //  EndImidiate();
                    }
                }
            }

            if (DestroyedOnCollision) Destroy(gameObject);
        }
    }
    void StartEnding()
    {

        
        var controller = GameObject.FindWithTag("ArenaController").GetComponent<ArenaController>();
        controller.StartEnding();
    }
}
