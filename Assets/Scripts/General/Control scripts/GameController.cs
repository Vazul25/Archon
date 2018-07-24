using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Assets.Scripts.Models;
using System.Linq;

/// <summary>
/// A játék sakktáblán történő vezérlését megvalósító osztály
/// főbb felelősségei: a pálya karbantartása(változó mezők változtatása), körök végének lebonyolítása, adatok-ról való gondoskodás,
/// scene-ek közti váltás lebonyolítása</summary> 
[Serializable]
public class GameController : MonoBehaviour
{
    private static GameController instance;
    
     
    /// <summary>
    /// Kulcs érték párokat tartalmazó struktúra</summary>
    /// <remarks>Azért volt szükség rá, mert csak így lehetett a unity editorból állítani az értékeket, ebből alkotott tömböt használva mapként</remarks>
    [Serializable]
    public struct DictionaryEntry
    {
        public string typename;
        public GameObject Prefab;
    }

    /// <summary>
    ///A mezők anyagát tároló tömb, ebből veszik fel az új értékeket minden kör végén a változó mezők  </summary>
    public Material[] tileMats;

    /// <summary>
    /// A változó mezők változásának íránya</summary> 
    public int mutationDirection;

    /// <summary>
    /// A változó mezők aktuális állapota</summary> 
    public int mutationState;
    /// <summary>
    /// A táblán lévő egységeket tároló lista</summary> 
    public List<Unit> units;
    /// <summary>
    ///  A név-prefab típust összerendelő segéd tömb</summary> 
    /// <remarks>Ezt fel lehet tölteni az editorból, később inicializálásnál pedig rendes mapet építeni belőle</remarks>
    public DictionaryEntry[] unitTypeToPrefab;
    /// <summary>
    ///  A név-prefab típust összerendelő map, inicializálásnál kerül feltöltésre a unitTypeToPrefab tömb értékeiből</summary> 
    private Dictionary<string, GameObject> typemap;

    /// <summary>
    /// Az egységek kiválasztásáért felelős prefab típus, ezt instantiate-eli a script a körök elején</summary> 
    public GameObject selectorPrefab;
    /// <summary>
    /// Az aktuális kiválasztó prefab példány, élettartalma 1 kör </summary> 
    private GameObject selector;
    /// <summary>
    /// Adatok mentését és betöltését kezelő objektum </summary> 
    private PersistenencyHelper persistManager;
    /// <summary>
    /// Eltelt körök száma</summary> 
    public int turncount = 1;
    /// <summary>
    /// A kamera animálásáért felel 
    /// Minden kör végén, átfordul a kamera a másik játékos nézőpontjába</summary> 
    Animator mainCameraAnimator;

    private List<GameObject> TileList;

    /// <summary>
    /// Monobehaviorból örökült függvény, az első update előtt fut le
    /// Inicializálási funkciókat végez, beállítja a componensekre mutató tagváltozók értékét
    /// ,a persistManager-t, továbbá betölti az előzőleg mentett adatokat ha szükséges</summary> 

    void Awake() {
        TileList = new List<GameObject>();
        typemap = new Dictionary<string, GameObject>();
        foreach (var item in unitTypeToPrefab)
        {
            typemap.Add(item.typename, item.Prefab);
        }
    }

    void Start()
    {
       
        persistManager = PersistencyManager.GetCurrentPersistor();
        Debug.Log("gamecontroller start");
        if (PersistencyManager.stateNumber == 1)
        {
            Debug.Log("Loading");
            Load();
            Debug.Log(turncount + " " + mutationState);


        }

        ChangeTileMaterial(tileMats[mutationState]);
        CreateSelector();
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");


        mainCameraAnimator = mainCamera.GetComponent<Animator>();

        if (turncount % 2 == 1) { mainCameraAnimator.SetTrigger("next"); Debug.Log("fut"); }

    }
    /// <summary>
    ///  Létrehoz egy egységet az adott kordinátán </summary>
    /// <param name="prefab">A létrehozni kívánt egység típusa, prefabként megadva</param>
    /// <param name="pos">A létrehozás pozíciója</param>
    /// <param name="hp">A létrehozni kívánt egység életpontjai</param> 
    private void InstantiateUnitAt(GameObject prefab, Vector3 pos, int hp)
    {
        var unitGO = Instantiate(prefab) as GameObject;
        Unit script = unitGO.GetComponent<Unit>();
        script.cords = pos;
        script.hp = hp;

        script.UpdatePos();
    }
    /// <summary>
    /// Betölti a legutóbb kimentett adatokat a persistManager segítségével</summary> 
    private void Load()
    {
        //PersistencyManager.UseLongTermPersist("Saves"+Path.DirectorySeparatorChar+"MySave2.bin");
        //var Data = PersistencyManager.GetCurrentPersistor().LoadAll();
        var Data = persistManager.LoadAll();
        turncount = Data.generalData.turncount;
        mutationState = Data.generalData.mutationState;
        mutationDirection = Data.generalData.mutationDirection;



        foreach (var item in Data.units)
        {
            
            InstantiateUnitAt(typemap[item.prefabType], new Vector3(item.coordx, item.coordy, item.coordz), item.hp);
        }
        //ez az érték akkor 0, ha mindkét egység elpusztult a csatában legutóbb, vagy ha még nem volt csata
        //erre azért van szükség mert a harcot folytató egységek lekerülnek a unit list ből a harc kezdete előtt, 
        //így nem mentődnek ki, hanem külön kell velük foglalkozni
        if (PersistencyManager.hp != 0)
        {
            Debug.Log(" item:");

            var unitGO = Instantiate(typemap[PersistencyManager.prefabType]);
            Unit script = unitGO.GetComponent<Unit>();
            script.cords = PersistencyManager.battleLocation;
            script.hp = PersistencyManager.hp;
            script.UpdatePos();
        }



    }
    /// <summary>
    ///Az aktuális adatok kimentése a persistManager segítségével</summary> 
    private void Save()
    {


        persistManager.SaveAll(
        new PersistencyModel
        {
            units = units.Select(m => new UnitData
            {
                coordx = m.cords.x,
                coordz = m.cords.z,
                coordy = m.cords.y,
                hp = m.hp,
                prefabType = m.prefabType
            }).ToList(),
            generalData = new GeneralData
            {
                mutationDirection = mutationDirection,
                mutationState = mutationState,
                turncount = turncount
            }
        });
        


    }
    /// <summary>
    /// A változó mezők átálítása a soron következő állapotba</summary> 
    void MutateTiles()
    {

        if (mutationState == 6) mutationDirection = -1;
        else if (mutationState == 0) mutationDirection = 1;
        mutationState += mutationDirection;
        if (mutationState > tileMats.Length || mutationState < 0) Debug.Log("tileMats out of bounds");
        else ChangeTileMaterial(tileMats[mutationState]);

    }
    /// <summary>
    /// Megváltoztatja a változó mezőkhöz tartozó material-t(textúrát)</summary> 
    /// <param name="toMutateMat">Az új textúra, amire szeretnék áttérni</param>
    void ChangeTileMaterial(Material toMutateMat)
    {
        GameObject[] toMutate = GameObject.FindGameObjectsWithTag("MutableTile");
        if (toMutate != null)
        {
            Debug.Log("mutate");
            Renderer rend;
            for (int i = 0; i < toMutate.Length; i++)
            {
                rend = toMutate[i].GetComponent<Renderer>();
                rend.material = toMutateMat;


            }
        }
        else Debug.Log("GameController.MutateTiles() could  find   Objects tagged as 'MutableTile'" + toMutate.Length + " " + mutationState + " " + mutationDirection);
    }

    /// <summary>
    /// A csatát indító függvény, ez felel a mentés meghívásáért, a szükséges adatokra, továbbá a Scene átváltásért</summary> 
    /// <param name="a">A harc egyik résztvevője</param>
    /// <param name="b">A harc másik résztvevője</param>
    public void InitiateCombat(Unit a, Unit b)
    {
        DontDestroyOnLoad(a.gameObject);
        DontDestroyOnLoad(b.gameObject);
        a.participateInBattle = true;
        b.participateInBattle = true;
        units.Remove(a);
        units.Remove(b);
        RegenUnits();
        Debug.Log(a.tag + " \t" + b.tag);

        var Tile = TileList.Find(p => Mathf.Approximately(p.transform.localPosition.x, a.transform.localPosition.x) && Mathf.Approximately(p.transform.localPosition.z, a.transform.localPosition.z));
        Debug.Log(Tile.tag);
        var aktMutationState = this.mutationState;




        MutateTiles();
        turncount++;
        PersistencyManager.battleLocation = a.cords;
        Save();
        switch (Tile.tag)
        {
            case "Black Tile":
                SceneManager.LoadScene(3);
                a.AdjustHpToTile(TileType.Type.Black);
                b.AdjustHpToTile(TileType.Type.Black);
                SceneManager.LoadScene("Dark_Arena");
                break;
            case "White Tile":
                a.AdjustHpToTile(TileType.Type.White);
                b.AdjustHpToTile(TileType.Type.White);
                SceneManager.LoadScene("ForestArena");
                
                break;
            case "MutableTile":
                a.AdjustHpToTile(TileType.Type.Mutating, aktMutationState);
                b.AdjustHpToTile(TileType.Type.Mutating, aktMutationState);
                if(aktMutationState > 4) SceneManager.LoadScene("ForestArena");
                else if(aktMutationState < 3) SceneManager.LoadScene("Dark_Arena");
                SceneManager.LoadScene("Neutral_Arena");
                break;
            default:
                Debug.Log("unindentified tile tag:" + Tile.tag);
                break;
        }
       


    }

    private void RegenUnits() {
        foreach (var unit in units)
        {
            unit.RegenerateHp();
        }
    }

    /// <summary>
    /// Minen kör végén lefut és a szükséges változtatásokat végrehajta,
    /// létrehozza az új selector-t, elindítja a kamera fordító animációt,
    /// továbbá meghívja a MutateTiles()-t</summary> 
    public void EndTurn()
    {
        Destroy(selector.gameObject);
        RegenUnits();
        turncount++;
        CreateSelector();
         

        if (mainCameraAnimator == null) Debug.Log("nincs MainCameraAnimator");
        mainCameraAnimator.SetTrigger("next");
        MutateTiles();
    }
    /// <summary>
    /// Létrehoz egy új selector objektumot, az hogy melyik játékoshoz tartozik, a körök számából határozza meg</summary> 
    void CreateSelector()
    {
        Debug.Log("Selector created" + turncount);
        string newtag;
        if ((turncount % 2) == 1) newtag = "Player2";
        else newtag = "Player1";
        if (selector != null)
            selector = Instantiate(selectorPrefab, selector.transform.position, selector.transform.rotation) as GameObject;
        else
            selector = Instantiate(selectorPrefab);
        selector.tag = newtag;
    }


    public void SavePermanent()
    {
        PersistencyManager.UseLongTermPersist(Path.Combine(Path.Combine(Application.dataPath, "Saves"), "SaveFile" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss.bin")).Replace('/', Path.DirectorySeparatorChar));
        persistManager = PersistencyManager.GetCurrentPersistor();
        Save();
        PersistencyManager.UseShortTermPersist();
        persistManager = PersistencyManager.GetCurrentPersistor();

    }

    internal void RegisterTile(GameObject tile)
    {
        
        TileList.Add(tile);
    }
}

