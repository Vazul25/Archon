using UnityEngine;
/// <summary>
/// Adott egységhez történő beregisztálás után, ha változik az élete az egységnek, akkor arról eseményt küld szét az összes hieararchiában alatta lévő gameobject számára.
/// Az adott játékos életcsíkjáért felelős gameobject-hez kell társítani </summary>

public class HudController : MonoBehaviour
{

    // Use this for initialization
    /// <summary>
    /// Felkutatja azt a játékost akinek a tag-je megegyezik a saját tagjének első 7 karakterével, és beregisztrálja magát hozzá</summary>
    void Awake()
    {
        Unit tmp = GameObject.FindWithTag(tag.Substring(0, 7)).GetComponent<Unit>();
        Debug.Log(tag.Substring(0, 7));
        Debug.Log( gameObject.tag);
        Debug.Log(tmp.tag);
        tmp.RegisterHudController(this);
    }
    /// <summary>
    /// Küld egy üzenetet a gyerekeinek, unity specifikus BroadcastMessage-el </summary>
    /// <param name="newHp">Az új megjelenítendő életpont</param>
    public void updateHP(float newHp)
    {
        Debug.Log("Hp updated " + newHp);
        //those who implement the HpBarViewChanger interface will get the message
        BroadcastMessage("HpChanged", newHp);

    }
}
