using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// A sötét oldal Chimera egységének speciális viselkedését leíro osztály</summary>
public class Chimera : Unit
{
    /// <summary>
    /// Meghívja az ős awake függvényét, továbbá beállítja a preferált méretet </summary>
    protected override void Awake()
    {
        base.Awake();
        PreferredArenaSize = new Vector3(5, 5, 5);


    }
    /// <summary>
    /// Az egység különleges viselkedését írja le az 1-es cselekvés végrehajtásakor</summary>
    protected override void Act1()
    {
        var instantiated = Instantiate(action1, actionPos1.position, transform.localRotation) as GameObject;
        instantiated.tag = tag + "Projectile";
    }

    /// <summary>
    /// Az egység különleges viselkedését írja le a 2-es cselekvés végrehajtásakor</summary>
    protected override void Act2()
    {
        var instantiated = Instantiate(action2, actionPos2.position, transform.localRotation) as GameObject;
        instantiated.tag = tag + "Projectile";
    }

}
