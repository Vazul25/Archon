using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Helper;

public class Scorpion : Unit {
    protected override void Act1()
    {
        var gameObj = Instantiate(action1, actionPos1.position, transform.localRotation) as GameObject;
        RetagHierarchy.Retag(gameObj.transform, tag + "Projectile");
    }

    protected override void Act2()
    {
        var gameObj = Instantiate(action2, actionPos2.position, transform.localRotation) as GameObject;
        RetagHierarchy.Retag(gameObj.transform, tag + "Projectile");
    }

    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(1.5f,1.5f,1.5f);



    }
}
