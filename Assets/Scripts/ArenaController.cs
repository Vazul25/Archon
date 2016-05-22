using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArenaController : MonoBehaviour {
    Unit player1, player2;
    // Use this for initialization
    public Transform player1Spawn;
    public Transform player2Spawn;
    void Start() {




        player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Unit>();
        player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Unit>();
        player1.InflictDamage(0); player2.InflictDamage(0);
        Debug.Log(player1.gameObject.transform.position);
        player1.gameObject.transform.position = player1Spawn.position;
        Debug.Log(player1.gameObject.transform.position);
        
        
        Debug.Log(player2.gameObject.transform.position);
        player2.gameObject.transform.position = player2Spawn.position;
        Debug.Log(player2.gameObject.transform.position);
        player2.gameObject.transform.localScale = player2.getPreferredSize(); 
        player1.gameObject.transform.localScale = player1.getPreferredSize() ;
        player1.EnableAnimator();
        player2.EnableAnimator();


        //TESTING:
        player1.participateInBattle = true;
       player2.participateInBattle = true;

    }

    // Update is called once per frame
    void Update() {
        if( Input.GetKey(KeyCode.R))  End() ;
    }
    public void StartEnding()
    {

        StartCoroutine(WaitBeforeEnd());
    }
    IEnumerator WaitBeforeEnd()
    {
 
        yield return new WaitForSeconds(5);

        End();
    }
    private void End()
    {
        Debug.Log("Ending started ");
        PersistencyManager.stateNumber = 1;
        player1.participateInBattle = false;
        player2.participateInBattle = false;
        Debug.Log(player1.hp);
        Debug.Log(player2.hp);
        if (player1.hp <= 0)
        {
            PersistencyManager.prefabType = player2.prefabType;
            PersistencyManager.hp = player2.hp;
        }

        if (player2.hp <= 0)
        {
            PersistencyManager.hp = player1.hp;
            PersistencyManager.prefabType = player1.prefabType;
        }
        if (player1.hp <= 0 && player2.hp <= 0)
        {
            PersistencyManager.hp = 0;
        }
        Debug.Log("Figyelj"+player1.prefabType+" "+player2.prefabType );
        Debug.Log(PersistencyManager.prefabType+" "+PersistencyManager.hp);
        Destroy(player1.gameObject);
        Destroy(player2.gameObject);
         
        SceneManager.LoadScene(1);
    }
}
