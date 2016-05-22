using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


[Serializable]
public class GameController : MonoBehaviour
{
    [Serializable]
    public struct DictionaryEntry
    {
        public string typename;
        public GameObject Prefab;
    }

    
    public Material[] tileMats;
    public int mutationDirection;
    public int mutationState;
    public List<Unit> units;
    public   DictionaryEntry[] unitTypeToPrefab;
    private Dictionary<string,GameObject> typemap;
    public GameObject selectorPrefab;
    private GameObject selector;
    private PersistenencyHelper persistManager;
    public int turncount=1;
    Animator mainCameraAnimator;


    // Use this for initialization
    void Start()
    {
        typemap = new Dictionary<string, GameObject>();
        foreach (var item in unitTypeToPrefab)
        {
            typemap.Add(item.typename, item.Prefab);
        }
        persistManager = PersistencyManager.GetCurrentPersistor();
        Debug.Log("gamecontroller start");
        if (PersistencyManager.stateNumber == 1) {
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
    private void InstantiateUnitAt(GameObject prefab, Vector3 pos,int hp)
    {
        var unitGO = Instantiate(prefab) as GameObject;
        Unit script = unitGO.GetComponent<Unit>();
        script.cords = pos;
        script.hp = hp;

        script.UpdatePos();
    }
    private void Load()
    {
        List<int> tmp = persistManager.LoadGeneralData();
        turncount = tmp[0];
        mutationState = tmp[1];
        mutationDirection = tmp[2];
        var tempunits = persistManager.LoadUnits();


        foreach (var item in tempunits)
        {
            InstantiateUnitAt(typemap[item.prefabType], item.cords, item.hp);
        /* var unitGO=   Instantiate(typemap[item.prefabType]) as GameObject;
         Unit script=unitGO.GetComponent<Unit>();
         script.cords =  item.cords ;
         script.hp = item.hp;
          
         script.UpdatePos();*/
        }
        if (PersistencyManager.hp != 0)
        {
            Debug.Log(   " item:" );

            var unitGO = Instantiate(typemap[PersistencyManager.prefabType]);
            Unit script = unitGO.GetComponent<Unit>();
            script.cords = PersistencyManager.battleLocation;
            script.hp = PersistencyManager.hp;
            script.UpdatePos();
        }
            

       
    }
    private void Save() {
        persistManager.SaveGeneralData(turncount, mutationState, mutationDirection);
        persistManager.SaveUnits(units);
       
        
    }

    void MutateTiles()
    {

        if (mutationState == 6) mutationDirection = -1;
        else if (mutationState == 0) mutationDirection = 1;
        mutationState += mutationDirection;
        if (mutationState > tileMats.Length || mutationState < 0) Debug.Log("tileMats out of bounds");
        else ChangeTileMaterial(tileMats[mutationState]);

    }
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
    // Update is called once per frame

    public void InitiateCombat(Unit a,Unit b)
    {
        DontDestroyOnLoad(a.gameObject);
        DontDestroyOnLoad(b.gameObject);
        a.participateInBattle = true;
        b.participateInBattle = true;
        units.Remove(a);
        units.Remove(b);
        Debug.Log(a.tag+" \t"+ b.tag);
        MutateTiles();
        turncount++;
        PersistencyManager.battleLocation = a.cords;
        Save(); 
        SceneManager.LoadScene(2);
       
       
    }
    public void EndTurn()
    {
        Destroy(selector.gameObject);
        
        turncount++;
        CreateSelector();

        //Renderer rtmp= selector.GetComponentInChildren<Renderer>();


        //   rtmp.material.color = Color.red;
        //ParticleSystem portalcore=selector.GetComponentInChildren<ParticleSystem>();
        // Material shader = selector.GetComponentInChildren<Material>();
        //shader.SetColor("_TintColor",Color.red);
        // portalcore.startColor =   Color.red;

        if (mainCameraAnimator == null) Debug.Log("nincs MainCameraAnimator");
        mainCameraAnimator.SetTrigger("next");
        MutateTiles();
    }
    void CreateSelector() {
        Debug.Log("Selector created"     + turncount);
        string newtag;
        if ((turncount % 2) == 1 ) newtag = "Player2";
        else newtag = "Player1";
        if (selector != null)
            selector = Instantiate(selectorPrefab, selector.transform.position, selector.transform.rotation) as GameObject;
        else
            selector = Instantiate(selectorPrefab);
      selector.tag = newtag;
    }
    void Update()
    {
        
    }
    
 

}
