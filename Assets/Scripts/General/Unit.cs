using UnityEngine;
using System.Collections;
using AStar;
using System.Collections.Generic;
using System;
using Assets.Scripts.Models;
/// <summary>
/// Minden egység álltalános viselkedését leíró abstract ősosztály
/// </summary>
[System.Serializable]
public abstract class Unit : MonoBehaviour
{

    //static private bool Testing = false;


    //public float moveTime = 0.1f;
    /// <summary>
    /// Az egység maximális életpontja
    /// </summary>
    [SerializeField]
    private int maxHp;
    /// <summary>
    /// Az egység életpontjai, ha eléri a 0 át, megsemmisül az egység
    /// </summary>
    public int hp;
    /// <summary>
    /// Megadja hogy az egység hány életpontot gyogyul körönként
    /// </summary>
    [SerializeField]
    public int hpRegen;

    /// <summary>
    /// Az egység mozgástípusait leíro enum.
    /// Walking: A sakktáblán csak a szomszédos mezőkre tud lépni 1 movementPoint költséggel, továbbá nem tud átlépni egységeken
    /// Flying: Az egység repül, ezáltal a táblán átlóban is tud lépni 1 költségel, továbbá átléphet egységek felett 1 költséggel
    /// </summary>
    public enum MovementType { Walking, Flying };
    /// <summary>
    /// Az aktuális lépés típust tárolja
    /// </summary>
    public MovementType movementType;
    /// <summary>
    /// Az egységek lépés száma,  maximum ilyen hosszú utat fogadhat el a táblán való lépéskor
    /// </summary>
    public int movementPoints;
    /// <summary>
    /// A scripthez tartozó prefab neve, ez alapján tudjuk létrehozni mentett állás  betöltésekor az identikus egységeket
    /// </summary>
    public string prefabType;
    /// <summary>
    /// Azt jelzi, hogy éppen részt vesz e harcban az egység
    /// </summary>
    public bool participateInBattle;
    /// <summary>
    /// Azt jelzi, hogy éppen cselekvés közben van-e az egység
    /// </summary>
    bool acting
    {
        set;
        get;
    }
    /// <summary>
    /// Az új tipusu animációk lejátszásához tartozó komponens referenciája
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// A régi típusu animációk kezeléséhez tartozó komponens
    /// </summary>
    protected Animation animationLegacy;
    /// <summary>
    /// Az 1es akció által létrehozott prefab fajtája
    /// </summary>
    public GameObject action1;
    /// <summary>
    /// A 2es akció által létrehozott prefab fajtája
    /// </summary>
    public GameObject action2;
    /// <summary>
    /// Az 1es akció által létrehozott prefab kiindulási poziciója, relatív az egységhez
    /// </summary>
    public Transform actionPos1;
    /// <summary>
    /// A 2es akció által létrehozott prefab kiindulási poziciója, relatív az egységhez
    /// </summary>
    public Transform actionPos2;

    /// <summary>
    /// Az egység mozgatásáért felelős komponens referenciája
    /// </summary>
    private CharacterController controller;


    /// <summary>
    /// A gravitáció hatása az adott egységre
    /// </summary>

    private int gravity = 10000;
    /// <summary>
    /// Az egység koordinátái, tesztelésénél van szerepe, végleges kódból el kell távólítani majd
    /// </summary>
    [HideInInspector]
    public Vector3 cords;
    /// <summary>
    /// Milyen gyakorisággal használhatja az 1-es cselekvését az egység
    /// </summary>

    public float act1rate;
    /// <summary>
    /// Milyen gyakorisággal használhatja a 2-es cselekvését az egység
    /// </summary>
    public float act2rate;
    /// <summary>
    /// Mikor használta az 1-es cselekvését legutóbb az egység
    /// </summary>
    private float lastAct1 = 0;
    /// <summary>
    /// Mikor használta a 2-es cselekvését legutóbb az egység
    /// </summary>
    private float lastAct2 = 0;
    /// <summary>
    /// Meddig tart az 1-es cselekvése az egységnek
    /// </summary>
    public float act1duration;
    /// <summary>
    /// Meddig tart a 2-es cselekvése az egységnek
    /// </summary>
    public float act2duration;
    /// <summary>
    /// Milyen gyorsan mozog az egység harc közben
    /// </summary>
    public float speed;
    /// <summary>
    /// Mennyi ideig sérthetetlen az egység, ha előtte megsérült
    /// </summary>
    public float invulnerabilityDuration;
    /// <summary>
    /// Azt jelzi hogy éppen sérthetetlen-e az egység
    /// </summary>
    public bool invulnerable { get; private set; }
    /// <summary>
    /// Mikor volt legutóbb sérthetetlen az egység
    /// </summary>
    private float invulnerableLast;
    /// <summary>
    /// Melyik irányba szeretne mozogni az egység
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;
    /// <summary>
    /// Referencia az életpontok megjelenítéséhez eseményt generáló scripthez
    /// </summary>
    private HudController hud;
    /// <summary>
    /// Az egység arénában preferált mérete, erre lesz fel skálázva
    /// </summary>
    protected Vector3 PreferredArenaSize;
    /// <summary>
    /// Az egység arénában lévő Y eltolását adja meg, repülő egységeknél használatos
    /// </summary>
    public float yOffset;
    /// <summary>
    /// Megadja hogy vége van-e a játéknak ha meghal az egység
    /// </summary>
    public bool Vip;
    /// <summary>
    /// Megadja hogy felt lett-e engedve már az akció 1 gomb az akció megkezdése óta
    /// </summary>
    private bool act1Released = true;
    /// <summary>
    /// Megadja hogy felt lett-e engedve már az akció 2 gomb az akció megkezdése óta
    /// </summary>
    private bool act2Released = true;

    /// <summary>
    /// Vissza adja, az egység preferált méretét
    /// </summary>
    public Vector3 getPreferredSize() { return PreferredArenaSize; }
    /// <summary>
    /// Átbillenti az invulerable változót és feljegyzi az aktuális időpontot az invulnerableLast változóba
    /// </summary>
    public void setInvulnerable()
    {
        invulnerable = true;
        invulnerableLast = Time.time;
    }



    //Protected, virtual functions can be overridden by inheriting classes.
    /// <summary>
    /// A hud privát változóba elmenti a paraméterként kapott változó referenciáját
    /// </summary>
    internal void RegisterHudController(HudController hudController)
    {
        hud = hudController;
        Debug.Log("Hud registered: " + tag);


    }
    public void RegenerateHp(float multiplier = 1)
    {
        hp += (int)(hpRegen * multiplier);
        if (hp >= maxHp) hp = maxHp;
    }
    /// <summary>
    /// Inicializálja az adatokat
    /// </summary>
    protected virtual void Awake()
    {
        invulnerable = false;
        animator = GetComponent<Animator>();
        animationLegacy = GetComponent<Animation>();



        controller = GetComponent<CharacterController>();
        GameObject gameControllerFinder;
        //   if (!Testing)
        //  {
        GameController gameController;
       
        gameControllerFinder = GameObject.FindWithTag("GameController");
        gameController = gameControllerFinder.GetComponent<GameController>();
        gameController.units.Add(this);
        //  }
        acting = false;
        cords = transform.position;

        // inverseMoveTime = 1f / moveTime;
    }
    /// <summary>
    /// Aktiválja, az animálást
    /// </summary>
    public void EnableAnimator()
    {

        if (animator != null) animator.enabled = true;
        else animationLegacy.enabled = true;
    }
    /// <summary>
    /// Frissíti a transform poziciót a koordináták alapján, Ez se lesz bent a végleges változatban
    /// </summary>
    public void UpdatePos() { transform.position = cords; }


    /// <summary>
    /// A transform.positiont módosítja az adott x,z koordinátára
    /// </summary>
    public void Move(float xDir, float zDir)
    {

        cords = new Vector3(xDir, transform.position.y, zDir);
        transform.position = cords;

    }
    /// <summary>
    /// Az egység megsérülésekor hívódik meg, csökkenti az életpontokat, és szól a hudnak hogy változás történt az életpontokban
    /// </summary>
    public void InflictDamage(int dmg)
    {
        hp -= dmg;
        hud.updateHP(hp);
    }
    /// <summary>
    /// Az egység irányításáért felelős, a billentyűzetről bevitt gombokat itt kezeljük le
    /// </summary>
    void Update()
    {
        if (participateInBattle)
        {
            if (invulnerableLast + invulnerabilityDuration < Time.time) invulnerable = false;
            moveDirection = Vector3.zero;
            if (animator != null) animator.SetFloat("speed", (moveDirection.magnitude), 0, 0);
            else if (!acting) animationLegacy.CrossFade("Idle");

            if (Time.time > lastAct1 + act1duration && Time.time > lastAct2 + act2duration) acting = false;

            if (!Input.GetButton(InputSettings.getAction1Button(gameObject.tag))) act1Released = true;
            if (!Input.GetButton(InputSettings.getAction2Button(gameObject.tag))) act2Released = true;

            if (!acting)
            {
                if (Input.GetButton(InputSettings.getAction1Button(gameObject.tag)) && lastAct1 + act1rate + act1duration < Time.time && act1Released)
                {
                    if (Input.GetButton(InputSettings.getRightButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, 90, 0);
                        acting = true;
                        act1Released = false;
                        Act1Base();
                        lastAct1 = Time.time;
                        return;
                    }
                    if (Input.GetButton(InputSettings.getLeftButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, -90, 0);
                        acting = true;
                        act1Released = false;
                        Act1Base();
                        lastAct1 = Time.time;
                        return;
                    }
                    if (Input.GetButton(InputSettings.getUpButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, 0, 0);
                        acting = true;
                        act1Released = false;
                        Act1Base();
                        lastAct1 = Time.time; return;
                    }
                    if (Input.GetButton(InputSettings.getDownButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, 180, 0); acting = true;
                        act1Released = false; Act1Base();
                        lastAct1 = Time.time; return;
                    }


                }

                if (Input.GetButton(InputSettings.getAction2Button(gameObject.tag)) && lastAct2 + act2rate + act2duration < Time.time && act2Released)
                {
                    if (Input.GetButton(InputSettings.getRightButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, 90, 0);
                        acting = true;  
                        act2Released = false;
                        Act2Base();
                        lastAct2 = Time.time;

                        return;
                    }
                    if (Input.GetButton(InputSettings.getLeftButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, -90, 0);
                        acting = true;

                        act2Released = false;
                        Act2Base();
                        lastAct2 = Time.time;
                        return;
                    }
                    if (Input.GetButton(InputSettings.getUpButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, 0, 0);
                        acting = true;

                        act2Released = false;
                        Act2Base();
                        lastAct2 = Time.time;
                        return;
                    }
                    if (Input.GetButton(InputSettings.getDownButton(gameObject.tag)))
                    {
                        transform.localEulerAngles = new Vector3(0, 180, 0);
                        acting = true;
                        act2Released = false;
                        Act2Base();
                        lastAct2 = Time.time;
                        return;
                    }

                }

                if ((!Input.GetButton(InputSettings.getAction1Button(gameObject.tag)) && !Input.GetButton(InputSettings.getAction2Button(gameObject.tag))))
                {

                
                    if (Input.GetButton(InputSettings.getRightButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, 90, 0); }
                    else if (Input.GetButton(InputSettings.getLeftButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, -90, 0); }
                    else if (Input.GetButton(InputSettings.getUpButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, 0, 0); }
                    else if (Input.GetButton(InputSettings.getDownButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, 180, 0); }

                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= speed;

                   
                    if (animator != null) animator.SetFloat("speed", (moveDirection.magnitude), 0, 0);
                    else if (moveDirection.magnitude > 0.1) animationLegacy.CrossFade("Walk");
                    if (movementType != MovementType.Flying)
                        moveDirection.y -= gravity * Time.deltaTime;
                     
                    controller.Move(moveDirection * Time.deltaTime);

                }
            }
        }
    }
    /// <summary>
    /// Súlyózzas az egység életét attól függően hogy éppen milyen mezőn harcol.
    /// </summary>
    /// <param name="type">Mező típusa</param>
    /// <param name="mutationState">Mutálódó mezők aktuális állapota, alapértékként 0, csak akkor van használva, ha type=Mutating </param>
    internal void AdjustHpToTile(TileType.Type type, int mutationState = 0)
    {
        Debug.Log("hp befor:" + tag + " " + hp);
        switch (type)
        {
            case TileType.Type.Black:
                if (tag.Contains("Player1")) hp -= (int)(hp * 0.25);
                else hp += (int)(hp * 0.25);
                break;
            case TileType.Type.White:
                if (tag.Contains("Player2")) hp -= (int)(hp * 0.25);
                else hp += (int)(hp * 0.25);
                break;
            case TileType.Type.Mutating:
                float multiplier = 0f;
                switch (mutationState)
                {

                    case 6: multiplier = 0.25f; break;
                    case 5: multiplier = 0.15f; break;
                    case 4: multiplier = 0.05f; break;
                    case 3: return;
                    case 2: multiplier = -0.05f; break;
                    case 1: multiplier = -0.15f; break;
                    case 0: multiplier = -0.25f; break;


                }
                if (tag.Contains("Player1"))
                {


                    hp += (int)(hp * multiplier);
                }
                else
                    hp -= (int)(hp * multiplier);
                break;
            default:

                break;
        }
        Debug.Log("hp after:" + hp);
    }




    // Finds a path from startLocation to endLocation,returns with path  if possible, else with null
    /// <summary>
    /// Megpróbál a* algoritmus meghívásával eljutni az adott pontból, az adott pontba, ha sikerül akkor az utvonalat tartalmazó listát adja vissza, különben null-t
    /// <param name="startLocation">A kezdő pont, ahonnan el akarunk jutni az endLocation-be</param>
    /// <param name="endLocation">A végpont, ahová el akarunk jutni</param>
    /// </summary>
    public List<Point> AttemptMove(Point startLocation, Point endLocation)

    {
        List<Point> path;
        if (movementType == MovementType.Flying)
        {
            path = Program.FindPath(startLocation, endLocation, true);
        }
        else
        {
            path = Program.FindPath(startLocation, endLocation, false);
        }

        if (path.Count == 0 || (path != null && path.Count > movementPoints)) return null;
        return path;


    }
    /// <summary>
    /// Animáció vezérlő coroutine ami act1duration után vissza állítja az animációt az alapállapotba
    /// </summary>
    IEnumerator RestoreAct1Anim()
    {
        yield return new WaitForSeconds(act1duration);

        if (animator != null) animator.SetBool("attack", false);
        else animationLegacy.CrossFade("Idle");
    }
    /// <summary>
    /// Animáció vezérlő coroutine ami act1duration után vissza állítja az animációt az alapállapotba
    /// </summary>
    IEnumerator RestoreAct2Anim()
    {
        yield return new WaitForSeconds(act2duration);
        if (animator != null) animator.SetBool("act2", false);
        else animationLegacy.CrossFade("Idle");
    }
    /// <summary>
    /// Az 1es cselekvés lefutásakor történő álltalános dolgokat hajta végre, animáció elinditása, acting változó true-ra állítása,
    /// majd meghívja az abstract Act1() függvényt, ami a leszármazottak ban van deklarálva
    /// </summary>
    private void Act1Base()
    {
        acting = true;
        if (animator != null)
        {
            animator.SetBool("attack", true);
            StartCoroutine(RestoreAct1Anim());
            Debug.Log(" animator " + gameObject.name);
        }
        else
        {
            Debug.Log(" Setting animation to Act1 " + gameObject.name);
            animationLegacy.CrossFade("Act1");
            Debug.Log(" animation" + gameObject.name);
            StartCoroutine(RestoreAct2Anim());
        }
        Act1();


    }
    /// <summary>
    /// A 2es cselekvés lefutásakor történő álltalános dolgokat hajta végre, animáció elinditása, acting változó true-ra állítása,
    /// majd meghívja az abstract Act2() függvényt, ami a leszármazottak ban van deklarálva
    /// </summary>
    private void Act2Base()
    {
        Debug.Log("Act2");
        acting = true;
        // for testing porpuses
        if (animator != null)
        {
            animator.SetBool("act2", true);
            Debug.Log("  animator " + gameObject.name);
            StartCoroutine(RestoreAct2Anim());
        }
        else
        {
            animationLegacy.CrossFade("act2");
            Debug.Log(" animation" + gameObject.name);
            // StartCoroutine(RestoreAct2Anim());
        }
        Act2();


    }
    /// <summary>
    /// Abstarct függvény, ami az egység specifikus 2es cselekvést  határozza meg
    /// </summary>
    protected abstract void Act2();
    /// <summary>
    /// Abstarct függvény, ami az egység specifikus 1es cselekvést   határozza meg
    /// </summary>
    protected abstract void Act1();
    public virtual void onArenaEnter()
    {
        gameObject.transform.localScale = getPreferredSize();
        EnableAnimator();
        participateInBattle = true;
    }
}