using UnityEngine;
using System.Collections;

public class Golem : Unit {

    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(3, 3, 3);



    }
    protected override void Act1()
    {
        var gameObj = Instantiate(action1, actionPos1.position, transform.localRotation) as GameObject;
        gameObj.tag = tag + "Projectile";
    }

    protected override void Act2()
    {
        var gameObj = Instantiate(action2, actionPos2.position, transform.localRotation) as GameObject;
        gameObj.tag = tag + "Projectile";

    }
}
