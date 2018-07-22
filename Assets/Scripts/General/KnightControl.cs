using UnityEngine;
using System.Collections;

public class KnightControl : MonoBehaviour {


  

public float speed   = 4;
public float runspeed  = 8;
private float moveAmount ;
public float smoothSpeed =  2;
private float sensitivityX = 6;


public float gravity = 25;
public float rotateSpeed = 8.0f;
public float dampTime   = 3;


private float horizontalSpeed;

public bool grounded ;
public AudioSource myaudiosource;

private float nextstep ;
public Transform target;
public Transform chest;



private bool running = false;



private Vector3 forward  = Vector3.forward;
private Vector3 moveDirection = Vector3.zero;
private Vector3 right ;


Transform shield ;
Transform weapon;
Transform lefthandpos ;
Transform righthandpos;
Transform chestposshield;
Transform chestposweapon ;
AudioClip equip1sound;
AudioClip[] wooshsounds;

AudioClip holster1sound ;

private bool fightmodus  = false;
private bool didselect ;
private bool canattack = false;

//var mycamera : Transform;
private Transform reference ;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        reference.eulerAngles = new Vector3(0, 0, 0);

        forward = reference.forward;
        right = new Vector3(forward.z, 0, -forward.x);




        var controller = GetComponent<CharacterController>();
        var animator = GetComponent < Animator>();

        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
    }

    void equip()
    {
        weapon.parent = righthandpos;
        weapon.position = righthandpos.position;
        weapon.rotation = righthandpos.rotation;
        myaudiosource.clip = equip1sound;
        myaudiosource.loop = false;
        myaudiosource.pitch = 0.9f + 0.2f * Random.value;
        myaudiosource.Play();
        shield.parent = lefthandpos;
        shield.position = lefthandpos.position;
        shield.rotation = lefthandpos.rotation;
        fightmodus = true;


    }
    void weaponselect()
    {

        var animator = GetComponent < Animator>();

        if (didselect)
        {

            animator.CrossFade("Holster", 0.15f, 0, 0);
            canattack = false;
            didselect = false;
           
        }
        else
        {

            animator.CrossFade("Equip", 0.15f, 0, 0);
            canattack = true;
            didselect = true;
            
        }



    }
}
