using UnityEngine;
/// <summary>
/// Adott idő eltelével megsemisíti a hozzá tartozó gameobjectet
/// </summary>
public class TimedSelfDestruct : MonoBehaviour {
    /// <summary>
    /// Mennyi idő múlva semmisítse meg a gameobjectet
    /// </summary>
    public float lifeTime;

    /// <summary>
    /// Meghívja a Destroy függvényt az adott lifeTime-al, az első update előtt
    /// </summary>
    void Start () {
        Destroy(gameObject, lifeTime);
    }
	 
}
