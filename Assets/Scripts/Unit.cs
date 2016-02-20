using UnityEngine;
using System.Collections;
using AStar;
using System.Collections.Generic;
public   class Unit : MonoBehaviour
{
        
    //public float moveTime = 0.1f;

    public int hp;

   public enum MovementType { Walking, Flying };
    public MovementType movementType;
    public int movementPoints;

    public bool participateInBattle;
    bool acting;

    public GameObject action1;
    public GameObject action2;
    public Transform actionPos1;
    public Transform actionPos2;

    private BoxCollider boxCollider;
    private CharacterController controller;
    private Animator animator ;

    private GameController gameController;
     

    public float speed = 6.0F;

    private Vector3 moveDirection = Vector3.zero;

    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
     
       boxCollider = GetComponent<BoxCollider>();


        controller = GetComponent<CharacterController>();
        GameObject gameControllerFinder;
        gameControllerFinder = GameObject.FindWithTag("GameController");
        gameController = gameControllerFinder.GetComponent<GameController>();
        gameController.units.Add(this);
        acting = true;
        // inverseMoveTime = 1f / moveTime;
    }


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
          
        transform.position = new Vector3(xDir, 0, zDir);
        // StartCoroutine(SmoothMovement(end));
    }
    public void InflictDamage(int dmg) {
        hp -= dmg;
    }

    void Update()
    {
        if (participateInBattle)
        {

            if (!acting)
            {
                //CharacterController controller = GetComponent<CharacterController>();
                
                if (controller.isGrounded)
                {
                     
                    if (Input.GetButtonDown(InputSettings.getRightButton(gameObject.tag))) moveDirection = new Vector3(1, 0, 0);
                    else if (Input.GetButtonDown(InputSettings.getLeftButton(gameObject.tag))) moveDirection = new Vector3(-1, 0, 0);
                    else if (Input.GetButtonDown(InputSettings.getUpButton(gameObject.tag))) moveDirection = new Vector3(0, 0, 1);
                    else if (Input.GetButtonDown(InputSettings.getDownButton(gameObject.tag))) moveDirection = new Vector3(0, 0, -1);

                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= speed;


                }

                controller.Move(moveDirection * Time.deltaTime);
            }
        }
    }
    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    /* protected IEnumerator SmoothMovement(Vector3 end)
     {
         //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
         //Square magnitude is used instead of magnitude because it's computationally cheaper.
         float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

         //While that distance is greater than a very small amount (Epsilon, almost zero):
         while (sqrRemainingDistance > float.Epsilon)
         {
             //Find a new position proportionally closer to the end, based on the moveTime
             Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

             //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
             rb2D.MovePosition(newPostion);

             //Recalculate the remaining distance after moving.
             sqrRemainingDistance = (transform.position - end).sqrMagnitude;

             //Return and loop until sqrRemainingDistance is close enough to zero to end the function
             yield return null;
         }
     }*/


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

        if (path != null && path.Count > movementPoints) return null;
        return path;


    }

    public virtual void Act1() {
        acting = true;
        animator.SetBool("Act1", true);

    }

    public virtual void Act2()
    {
        acting = true;
        animator.SetBool("Act1", true);

    }
    //The abstract modifier indicates that the thing being modified has a missing or incomplete implementation.
    //OnCantMove will be overriden by functions in the inheriting classes.
    //protected   void OnCanMove();

}