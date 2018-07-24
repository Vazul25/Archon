using UnityEngine;
using System.Collections;
using Assets.Scripts.Helper;

public class Samurai : Unit {

    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(3, 3, 3);



    }
    protected override void Act1()
    {
        var gameObj = Instantiate(action1, actionPos1.position, transform.localRotation) as GameObject;
        RetagHierarchy.Retag(gameObj.transform, tag + "Projectile");
    }

    protected override void Act2()
    {
        var gameObj = Instantiate(action2, new Vector3(0,1,0), transform.localRotation) as GameObject;
        RetagHierarchy.Retag(gameObj.transform, tag + "Projectile");
        gameObj.transform.localRotation=transform.rotation;
        gameObj.transform.localPosition = new Vector3(0, 3, 0);
        gameObj.transform.localScale = transform.localScale;
       var PositionScript = gameObj.GetComponent<InheritPosition>();
        PositionScript.parent = transform;
        PositionScript.initial = actionPos2;

    }
}