using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class GameController : MonoBehaviour {
  


    public Material[] tileMats;
    public int mutationDirection;
    public int mutationState;
    public List<Unit> units;
   

    // Use this for initialization
    void Start () {
        ChangeTileMaterial(tileMats[3]);
        
    }


    void MutateTiles()
    {
      
            if (mutationState == 6) mutationDirection = -1;
            if (mutationState == 0) mutationDirection = 1;
            mutationState+=mutationDirection;
        if (mutationState > tileMats.Length || mutationState < 0) Debug.Log("tileMats out of bounds");
        else ChangeTileMaterial(tileMats[mutationState]);
       
    }
    void ChangeTileMaterial(Material toMutateMat)
    {
        GameObject[] toMutate = GameObject.FindGameObjectsWithTag("MutableTile");
        if (toMutate != null)
        {
            Debug.Log("GameController.MutateTiles() could  find   Objects tagged as 'MutableTile'"+toMutate.Length+" "+mutationState+" "+mutationDirection);
            Renderer rend;
            for (int i = 0; i < toMutate.Length; i++)
            {
                rend = toMutate[i].GetComponent<Renderer>();
               rend.material= toMutateMat;


            }
        }
        else Debug.Log("GameController.MutateTiles() could not find any Objects tagged as 'MutableTile'");
    }
    // Update is called once per frame
    public void InitiateCombat()
    {
        MutateTiles();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MutateTiles();
        }   

        
	}
}
