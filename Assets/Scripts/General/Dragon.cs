using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Helper;

public class Dragon : Unit {
    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(0.075f, 0.075f, 0.075f);
      


    }


    
    /// <summary>
    /// Az egység különleges viselkedését írja le az 1-es cselekvés végrehajtásakor</summary>
    protected override void Act1()
    {
        var instantiated =Instantiate(action1, Vector3.one, Quaternion.identity) as GameObject;
        // instantiated.transform.parent = actionPos1;
        
        instantiated.tag = tag + "Projectile";
        instantiated.transform.localPosition = actionPos1.transform.position;
        instantiated.transform.localRotation = actionPos1.transform.rotation;
        
    }

    /// <summary>
    /// Az egység különleges viselkedését írja le a 2-es cselekvés végrehajtásakor</summary>
    protected override void Act2()
    {
        var instantiated = Instantiate(action2, Vector3.one, Quaternion.identity) as GameObject;
        // instantiated.transform.parent = actionPos1;
        RetagHierarchy.Retag(instantiated.transform, tag + "Projectile");
         
        instantiated.transform.localPosition = actionPos2.transform.position;
        instantiated.transform.localRotation = actionPos2.transform.rotation;
    }
    public override void onArenaEnter() {
        base.onArenaEnter();

        animator.SetBool("inArena", true);

    }
}
