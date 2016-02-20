using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {
    GameController gameController;
    GameObject selected;
    Transform transform;
   
    int z;
    int x;
    // Use this for initialization
    void Start () {
        selected = null;
        GameObject gameControllerTmp;
        gameControllerTmp = GameObject.FindWithTag("GameController");
        gameController=gameControllerTmp.GetComponent<GameController>();
        transform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown(InputSettings.getRightButton(gameObject.tag)))       MoveRight();
        else if (Input.GetButtonDown(InputSettings.getLeftButton(gameObject.tag)))  MoveLeft();
        else if (Input.GetButtonDown(InputSettings.getUpButton(gameObject.tag)))    MoveUp();
        else if (Input.GetButtonDown(InputSettings.getDownButton(gameObject.tag)))  MoveDown();
        if (Input.GetButtonDown(InputSettings.getAction1Button(gameObject.tag))) {     Select(); }

    }


    public void Select() {
        Debug.Log(Time.time + " Select meghivva");
        System.Collections.Generic.List<Unit> units = gameController.units;
        //when there isnt a unit selected to move
        if (selected == null) {
            //go trough units, check if there is a unit on the selectors tile
            for (int i = 0; i < units.Count; i++) {
                if (Mathf.Abs(units[i].transform.position.x - transform.position.x) < Mathf.Epsilon &&
                    Mathf.Abs(units[i].transform.position.z - transform.position.z) < Mathf.Epsilon
                    && gameObject.tag.Equals(units[i].tag))
                {
                    //select the unit
                    selected = units[i].gameObject;
                    if (selected == null) Debug.Log(Time.time + " Hiba a selectálásnál null pointer");
                    else Debug.Log(Time.time + " selectálás megtörtént a " + transform.position + " mezon");
                    return;
                }
                //log that there were an error
                else Debug.Log(Time.time + " Hiba a selectálásnál");

            }

            return;
        }
        else
        {


            Unit u = selected.GetComponent<Unit>();
            //selecting on the same tile diselects the unit
            Debug.Log(Time.frameCount + " " + transform.position + " " + u.transform.position + "  " + " hasonlit ");
            if (transform.position.x==u.transform.position.x&& transform.position.z == u.transform.position.z) { selected = null;  Debug.Log(Time.time + " elengedve/diselect"); return; }
            //TODO tweak this is not efficent, init isnt needed that often... do it after each sucessfull step and at init
            AStar.Program.InitializeMap();

            //init the map for Astar algorithm
            for (int i = 0; i < units.Count; i++)
            {
                AStar.Program.AddWall((int)units[i].transform.position.x, (int)units[i].transform.position.z);
            }
            //Remove akadály at fin, TODO tweak to skip this if there is a friendly unit
            AStar.Program.RemoveWall((int)selected.transform.position.x, (int)selected.transform.position.z);

            System.Collections.Generic.List<AStar.Point> path = u.AttemptMove(  
                new AStar.Point( (int) u.transform.position.x, (int) u.transform.position.z),
                new AStar.Point( (int) transform.position.x  , (int) transform.position.z ));


            if (path == null)
                Debug.Log(Time.time + " Nem tud oda lépni");
            else
            {
                u.Move(transform.position.x, transform.position.z);
                selected = null;

                Debug.Log(Time.time + " lepett,elengedve");
                gameController.InitiateCombat();
            }

           
           
        }

    }

    public void MoveRight()
    {
        if (x >=  8) x = 8;
        else x += 1;
        transform.position = new Vector3(x, transform.position.y, z);
    }
    public void MoveLeft()
    {
        if (x <= 0) x = 0;
        else x -= 1;
        transform.position = new Vector3(x, transform.position.y, z);
    }
    public void MoveUp()
    {
        if (z >= 8) z = 8;
        else z += 1;
        transform.position = new Vector3(x, transform.position.y, z);
    }
    public void MoveDown()
    {
        if (z <= 0) z = 0;
        else z -= 1;
        transform.position = new Vector3(x, transform.position.y, z);
    }
    

}
