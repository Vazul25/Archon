using UnityEngine;
using System.Collections;

/// <summary>
/// Ütközéskor sebzést generáló script</summary>
public class CollisionDamage : MonoBehaviour
{

    /// <summary>
    /// Ütközéskor történő megszünést engedélyezi</summary>
    public bool DestroyedOnCollision;

    /// <summary>
    /// Ütközéskor történő sebzés mértékét adja meg</summary>
    public int dmg;
    /// <summary>
    /// Azt befolyásolja, hogy sebzés után állítsa sebezhetetlenre az ellenfelet</summary>
    public bool makeInvulnerable;
    /// <summary>
    /// Triggerel való ütközéskor hívodik meg, megszünteti a gameobjectet amihez tartozik,
    /// és amennyiben ellenséges játékossal ütközik sebzést fejt ki, ha meghal a sebzett játékos, akkor meghívja a StartEnding() függvényt</summary>
    public void OnTriggerEnter(Collider other)
    {

        Debug.Log("On trigger:" + gameObject.tag + " other tag" + other.gameObject.tag);

        if (!gameObject.tag.Contains(other.gameObject.tag) && (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2"))
        {

            Unit tmp = other.gameObject.GetComponent<Unit>();
            if (!tmp.invulnerable)
            {
                Debug.Log("Before:" + tmp.hp);
                tmp.InflictDamage(dmg);

                Debug.Log("After" + tmp.hp);
                if (makeInvulnerable)
                    tmp.setInvulnerable();
                if (tmp.hp <= 0)
                {
                    Debug.Log("tmp.hp<0" + tmp.hp);
                    StartEnding();
                    tmp.participateInBattle = false;
                    //  EndImidiate();
                }
            }


            if (DestroyedOnCollision) Destroy(gameObject);
        }
        if (other.gameObject.tag == "Obsticle" && DestroyedOnCollision) Destroy(gameObject);
    }
    void OnCollisionEnter(Collision col)
    {
        
        if (col.gameObject.tag == "Obsticle") {
            if (DestroyedOnCollision) Destroy(gameObject);
        }

    }
    /// <summary>
    /// Lekéri az arenaController-t és szól neki hogy be kezdje meg a harc befejezését</summary>,

    void StartEnding()
    {


        var controller = GameObject.FindWithTag("ArenaController").GetComponent<ArenaController>();
        controller.StartEnding();
    }
}
