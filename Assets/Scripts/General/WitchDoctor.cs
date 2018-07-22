using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Helper;

public class WitchDoctor : Unit {
    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(2,2, 2);


        
    }
    protected override void Act1()
    {
        var gameObj=Instantiate(action1, actionPos1.position, transform.localRotation) as GameObject;
        RetagHierarchy.Retag(gameObj.transform, gameObject.tag+"Projectile");
    }

    protected override void Act2()
    {
        transform.localPosition = new Vector3(UnityEngine.Random.Range(60f, 120f), transform.localPosition.y, UnityEngine.Random.Range(100f, 160f));
    }

    public override void onArenaEnter()
    {
        base.onArenaEnter();
        movementType = MovementType.Walking;
    }
}
