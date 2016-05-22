using UnityEngine;
using System.Collections;
using System;

public class DarkPawn : Unit
{
    protected override void Awake() 
    {
        base.Awake();
        PreferredArenaSize = new Vector3(1, 1, 1);


        animationLegacy["Act1"].speed = 2.0f;
    }
    protected override void Act1()
    {
        Instantiate(action1, actionPos1.position, transform.localRotation);
    }

    protected override void Act2()
    {

    }
}
