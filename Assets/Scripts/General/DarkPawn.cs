using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Helper;
/// <summary>
/// A sötét oldal paraszt egységének speciális viselkedését leíro osztály</summary>
public class DarkPawn : Unit
{
    /// <summary>
    /// Meghívja az ős awake függvényét, továbbá beállítja a preferált méretet, és at act1 animáció sebességét </summary>
    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(1, 1, 1);


        animationLegacy["Act1"].speed = 2.0f;
    }
    /// <summary>
    /// Az egység különleges viselkedését írja le az 1-es cselekvés végrehajtásakor</summary>
    protected override void Act1()
    {
        var gameObj = Instantiate(action1, actionPos1.position, transform.localRotation) as GameObject;
        RetagHierarchy.Retag(gameObj.transform, tag + "Projectile");
    }
    /// <summary>
    /// Az egység különleges viselkedését írja le az 2-es cselekvés végrehajtásakor</summary>
    protected override void Act2()
    {
        var gameObj = Instantiate(action2, actionPos2.position, transform.localRotation) as GameObject;
        gameObj.transform.localScale = new Vector3(4f,4f,1f);
    }
}
