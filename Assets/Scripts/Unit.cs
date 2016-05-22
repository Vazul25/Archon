using UnityEngine;
using System.Collections;
using AStar;
using System.Collections.Generic;
using System;

[System.Serializable]
public abstract class Unit : MonoBehaviour
{
    static private bool Testing = false;


    //public float moveTime = 0.1f;

    public int hp;
    public float dampTime = 3;
    public enum MovementType { Walking, Flying };
    public MovementType movementType;
    public int movementPoints;

    public string prefabType;
    public bool participateInBattle;
    bool acting
    {
        set;
        get;
    }
    protected Animation animationLegacy;
    public GameObject action1;
    public GameObject action2;
    public Transform actionPos1;
    public Transform actionPos2;

    private BoxCollider boxCollider;
    private CharacterController controller;
    private Animator animator;

    private GameController gameController;
    private int gravity = 10000;
    public Vector3 cords;
    public float act1rate;
    public float act2rate;
    private float lastAct1 = 0;
    private float lastAct2 = 0;
    public float act1duration;
    public float act2duration;
    public float speed = 6.0F;
    public float invulnerabilityDuration;
    public bool invulnerable { get; private set; }
    private float invulnerableLast;
    private Vector3 moveDirection = Vector3.zero;
    private HudController hud;
    protected Vector3 PreferredArenaSize;
    public Vector3 getPreferredSize() { return PreferredArenaSize; }
    public void setInvulnerable()
    {
        invulnerable = true;
        invulnerableLast = Time.time;
    }
    //Protected, virtual functions can be overridden by inheriting classes.

    internal void RegisterHudController(HudController hudController)
    {
        hud = hudController;
        Debug.Log("Hud registered: " + tag);

    }
    protected virtual void Awake()
    {
        invulnerable = false;
        animator = GetComponent<Animator>();
        animationLegacy = GetComponent<Animation>();
        boxCollider = GetComponent<BoxCollider>();


        controller = GetComponent<CharacterController>();
        GameObject gameControllerFinder;
        if (!Testing)
        {
            gameControllerFinder = GameObject.FindWithTag("GameController");
            gameController = gameControllerFinder.GetComponent<GameController>();
            gameController.units.Add(this);
        }
        acting = false;
        cords = transform.position;

        // inverseMoveTime = 1f / moveTime;
    }
    public void EnableAnimator()
    {

        if (animator != null) animator.enabled = true; 
        else animationLegacy.enabled = true;
    }
    public void UpdatePos() { transform.position = cords; }
    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    public void Move(float xDir, float zDir)
    {
        //Store start position to move from, based on objects current transform position.
        /*   Vector3 start = transform.position;
           Vector3 end = start + new Vector3(xDir, 0,zDir);
           boxCollider.enabled = false;
           boxCollider.enabled = true;
           */
        cords = new Vector3(xDir, 0, zDir);
        transform.position = cords;
        // StartCoroutine(SmoothMovement(end));
    }
    public void InflictDamage(int dmg)
    {
        hp -= dmg;
        hud.updateHP(hp);
    }

    void Update()
    {
        if (participateInBattle)
        {
            if (invulnerableLast + invulnerabilityDuration < Time.time) invulnerable = false;
            moveDirection = Vector3.zero;
           if(animator!=null) animator.SetFloat("speed", (moveDirection.magnitude), 0, 0);
           else if(!acting) animationLegacy.CrossFade("Idle");
            if (Time.time > lastAct1 + act1duration && Time.time > lastAct2 + act2duration) acting = false;

            if (!acting)
            {
                if (Input.GetButton(InputSettings.getAction1Button(gameObject.tag)) && lastAct1 + act1rate + act1duration < Time.time)
                {
                    if (Input.GetButton(InputSettings.getRightButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, 90, 0); acting = true; Act1Base(); lastAct1 = Time.time; }
                    else if (Input.GetButton(InputSettings.getLeftButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, -90, 0); acting = true; Act1Base(); lastAct1 = Time.time; }
                    else if (Input.GetButton(InputSettings.getUpButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, 0, 0); acting = true; Act1Base(); lastAct1 = Time.time; }
                    else if (Input.GetButton(InputSettings.getDownButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, 180, 0); acting = true; Act1Base(); lastAct1 = Time.time; }

                }

                if (Input.GetButton(InputSettings.getAction2Button(gameObject.tag)) && lastAct2 + act2rate + act2duration < Time.time && !acting)
                {
                    if (Input.GetButton(InputSettings.getRightButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, 90, 0); acting = true; Act2(); lastAct2 = Time.time; }
                    else if (Input.GetButton(InputSettings.getLeftButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, -90, 0); acting = true; Act2(); lastAct2 = Time.time; }
                    else if (Input.GetButton(InputSettings.getUpButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, 0, 0); acting = true; Act2(); lastAct2 = Time.time; }
                    else if (Input.GetButton(InputSettings.getDownButton(gameObject.tag))) { transform.localEulerAngles = new Vector3(0, 180, 0); acting = true; Act2(); lastAct2 = Time.time; }

                }

                if ((!Input.GetButton(InputSettings.getAction1Button(gameObject.tag)) && !Input.GetButton(InputSettings.getAction2Button(gameObject.tag))) && !acting)
                {

                    //CharacterController controller = GetComponent<CharacterController>();

                    if (controller.isGrounded)
                    {

                        if (Input.GetButton(InputSettings.getRightButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, 90, 0); }
                        else if (Input.GetButton(InputSettings.getLeftButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, -90, 0); }
                        else if (Input.GetButton(InputSettings.getUpButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, 0, 0); }
                        else if (Input.GetButton(InputSettings.getDownButton(gameObject.tag))) { moveDirection = new Vector3(0, 0, 1); transform.localEulerAngles = new Vector3(0, 180, 0); }

                        moveDirection = transform.TransformDirection(moveDirection);
                        moveDirection *= speed;

                        // Debug.Log("Moving toward :" +moveDirection.ToString());

                    }
                    if (animator != null) animator.SetFloat("speed", (moveDirection.magnitude), 0, 0);
                    else  if (moveDirection.magnitude>0.1)animationLegacy.CrossFade("Walk"); 
                    moveDirection.y -= gravity * Time.deltaTime;
                    //  Debug.Log("Before Moving   :" + moveDirection.ToString());
                    controller.Move(moveDirection * Time.deltaTime);

                }
            }
        }
    }



    /// <summary>
    /// Finds a path from startLocation to endLocation,returns with path  if possible, else with null
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
    IEnumerator RestoreAct1Anim()
    {
        yield return new WaitForSeconds(act1duration);
        
        if (animator != null) animator.SetBool("attack", false);
        else animationLegacy.CrossFade("Idle");
    }
    IEnumerator RestoreAct2Anim()
    {
        yield return new WaitForSeconds(act2duration);
        if (animator != null) animator.SetBool("act2", false);
        else animationLegacy.CrossFade("Idle");
    }
    public void Act1Base()
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

    public void Act2Base()
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
        else {
           // animation.CrossFade("Act2");
            Debug.Log(" animation" + gameObject.name);
           // StartCoroutine(RestoreAct2Anim());
        }
        Act2();


    }
    protected abstract void Act2();
    protected abstract void Act1();
}