using UnityEngine;
using System.Collections;
/// <summary>
/// Az egységek sakktáblán való kiválasztásáért,mozgatásáért felelős script ami a selector prefab-hoz van hozzárendelve  </summary>
public class Selector : MonoBehaviour
{
    /// <summary>
    /// Az aktív gameController script, ami a gameController gameObject-hez tartozik
    /// </summary>
    GameController gameController;
    /// <summary>
    /// Az aktuálissan kiválasztott egység, null ha nincs kiválasztva egy egység se
    /// </summary>
    GameObject selected;

    /// <summary>
    /// A selector z koordinátája
    /// </summary>
    int z;
    /// <summary>
    /// A selector x koordinátája
    /// </summary>
    int x;

    /// <summary>
    /// Az előre irány, ez a körök végén a kamera mozgatás miatt előjelet vált
    /// </summary>
    int forward;

    /// <summary>
    /// A jobbra irány, ez a körök végén a kamera mozgatás miatt előjelet vált
    /// </summary>
    int right;

    /// <summary>
    /// A gamecontroller-ben tárolt units ra referencia
    /// </summary>
    System.Collections.Generic.List<Unit> units;
    /// <summary>
    /// A gamecontroller-ben tárolt units ra referencia
    /// </summary>

    /// <summary>
    /// Az adatok inicializálása történik itt
    /// </summary>
    void Start()
    {
        selected = null;
        GameObject gameControllerTmp;
        gameControllerTmp = GameObject.FindWithTag("GameController");
        gameController = gameControllerTmp.GetComponent<GameController>();

        z = (int)transform.position.z;
        x = (int)transform.position.x;
        units = gameController.units;
        if (gameObject.tag == "Player1") { forward = 1; right = 1; }
        else
        {
            forward = -1; right = -1;
        }
        //   gameController.RegisterSelector(this);
    }

    /// <summary>
    /// Minden frame-ben meghívásra kerül, itt történik a billentyűzetről bevitt adatok feldolgozása
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown(InputSettings.getRightButton(gameObject.tag))) MoveRight();
        else if (Input.GetButtonDown(InputSettings.getLeftButton(gameObject.tag))) MoveLeft();
        else if (Input.GetButtonDown(InputSettings.getUpButton(gameObject.tag))) MoveUp();
        else if (Input.GetButtonDown(InputSettings.getDownButton(gameObject.tag))) MoveDown();
        if (Input.GetButtonDown(InputSettings.getAction1Button(gameObject.tag))) { Select(); }

    }
    /// <summary>
    /// Visszatér az adott pozición álló egységel, kizárólag az x,z kordináta figyelembe vévtelével 
    /// </summary>
    public Unit GetUnitAtPosXZ(Vector3 pos)
    {
        for (int i = 0; i < units.Count; i++)
        { 
            if (Mathf.Abs(units[i].transform.position.x - pos.x) < Mathf.Epsilon &&
                Mathf.Abs(units[i].transform.position.z - pos.z) < Mathf.Epsilon
                 )
                return units[i];
        }
        return null;

    }
    /// <summary>
    /// Az egység vagy az egység lépésének céljának kijelölése, és a lépés elfogadhatóságának ellenörzése a*-al, majd a lépés végrehajtása.
    /// Az act1 gomb lenyomásakor hivódik meg.
    /// </summary>
    public void Select()
    {
        Debug.Log(Time.time + " Select meghivva");

        //when there isnt a unit selected to move
        if (selected == null)
        {
            //go trough units, check if there is a unit on the selectors tile replaced by using GetUnitAtPosXY

            Unit unitSelect = GetUnitAtPosXZ(transform.position);
            // for (int i = 0; i < units.Count; i++) {
            if (unitSelect != null && gameObject.tag.Equals(unitSelect.gameObject.tag))
            {
                //select the unit
                selected = unitSelect.gameObject;
                if (selected == null) Debug.Log(Time.time + " Hiba a selectálásnál null pointer");
                else Debug.Log(Time.time + " selectálás megtörtént a " + transform.position + " mezon");
                return;
            }
            //log that there were an error
            else Debug.Log(Time.time + " Hiba a selectálásnál " + transform.position);

            //    }

            return;
        }
        else //alrdy have selected unit
        {


            Unit u = selected.GetComponent<Unit>();

            //selecting on the same tile diselects the unit
            Debug.Log(Time.frameCount + " " + transform.position + " " + u.transform.position + "  " + " compare ");
            if (transform.position.x == u.transform.position.x && transform.position.z == u.transform.position.z) { selected = null; Debug.Log(Time.time + " elengedve/diselect"); return; }
            //TODO tweak this, this is not efficent, init isnt needed that often... 
            AStar.Program.InitializeMap();

            //init the map for Astar algorithm
            if (u.movementType != Unit.MovementType.Flying)
                for (int i = 0; i < units.Count; i++)
                {

                    AStar.Program.AddWall((int)units[i].transform.position.x, (int)units[i].transform.position.z);
                }
             
            Unit temp = GetUnitAtPosXZ(transform.position);
            if (temp!=null && temp.gameObject.tag == gameObject.tag) { Debug.Log("Cant move there friendly unit occupies the tile"); return; }
            if (temp != null && temp.gameObject.tag != gameObject.tag)
            {
                AStar.Program.RemoveWall((int)transform.position.x, (int)transform.position.z);
                AStar.Program.RemoveWall((int)transform.position.x, (int)transform.position.z);

            }
            //  if(GetUnitAtPosXZ())
            // AStar.Program.RemoveWall((int)selected.transform.position.x, (int)selected.transform.position.z);

            System.Collections.Generic.List<AStar.Point> path = u.AttemptMove(
                new AStar.Point((int)u.transform.position.x, (int)u.transform.position.z),
                new AStar.Point((int)transform.position.x, (int)transform.position.z));


            if (path == null)
                Debug.Log(Time.time + " Nem tud oda lépni");

            else
            {
                Unit selectedUnit = selected.GetComponent<Unit>();
               
                u.Move(transform.position.x, transform.position.z);

                //AStar.Program.ShowRoute("asd", path);
                //foreach (var a in path) { System.Console.WriteLine("\n 1 ssssss\n" +" x:"+a.X+" y:"+a.Y); }
                Debug.Log(Time.time + " lepett,elengedve");

                
                Debug.Log(selectedUnit.tag);
                // Debug.Log(temp.tag);
                if (temp != null) gameController.InitiateCombat(selectedUnit, temp);
                else gameController.EndTurn();
                selected = null;
            }



        }

    }
    //TODO get camera forward direction
    /// <summary>
    /// Jobbra lépteti a selector prefab-et kamera állást figyelembe véve, feltéve hogy valid a lépés
    /// </summary>
    public void MoveRight()
    {
        x += right;
        x = correctCordinate(x);
        transform.position = new Vector3(x, transform.position.y, z);
    }
    /// <summary>
    /// Balra lépteti a selector prefab-et kamera állást figyelembe véve, feltéve hogy valid a lépés
    /// </summary>
    public void MoveLeft()
    {
        x -= right;
        x = correctCordinate(x);
        transform.position = new Vector3(x, transform.position.y, z);
    }
    /// <summary>
    /// Felfele lépteti a selector prefab-et kamera állást figyelembe véve, feltéve hogy valid a lépés
    /// </summary>
    public void MoveUp()
    {
        z += forward;
        z = correctCordinate(z);
        transform.position = new Vector3(x, transform.position.y, z);
    }
    /// <summary>
    /// Lefele lépteti a selector prefab-et kamera állást figyelembe véve, feltéve hogy valid a lépés
    /// </summary>
    public void MoveDown()
    {
        z -= forward;
        z = correctCordinate(z);
        transform.position = new Vector3(x, transform.position.y, z);
    }
    /// <summary>
    /// Ellenőrzi hogy korrekt e a koordináta
    /// </summary>
    private int correctCordinate(int x)
    {
        if (x > 8) return 8;
        if (x < 0) return 0;
        return x;
    }
}
