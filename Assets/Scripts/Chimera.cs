using UnityEngine;
using System.Collections;
using System;

public class Chimera : Unit {

    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(5, 5, 5);


    }
    protected override void Act1()
    {
        Instantiate(action1, actionPos1.position, transform.localRotation);
    }

    protected override void Act2()
    {
        
    }
 
}
