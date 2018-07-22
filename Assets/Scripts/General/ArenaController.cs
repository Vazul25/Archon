using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// A játék harctéren történő vezérlését megvalósító osztály
/// főbb felelőssége: adatok inicializálás,scene-ek közti váltás lebonyolítása</summary> 
public class ArenaController : MonoBehaviour
{

    Unit player1;
    Unit player2;
    public GameObject gameOverPrefab;
    /// <summary>
    /// 1-es játékos kezdő pozíciója, editorból állítható</summary> 
    public Transform player1Spawn;

    /// <summary>
    /// 2-es játékos kezdő pozíciója, editorból állítható</summary> 
    public Transform player2Spawn;
    /// <summary>
    /// Inicializálást végez, player1,2 lekérése, kezdő pozíciojának, méretének beállítása
    /// , hud beállítása, egységek animálásának engedélyezése</summary> 
    void Start()
    {


        //Time.timeScale = 0.2f;
        //Time.fixedDeltaTime = 0.2f;
        player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Unit>();
        player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Unit>();

        player1.InflictDamage(0);
        player2.InflictDamage(0);
        Debug.Log(player1.gameObject.transform.position);
        player1.gameObject.transform.position = player1Spawn.position+new Vector3(0f,player1.yOffset,0f);
        Debug.Log(player1.gameObject.transform.position);


        Debug.Log(player2.gameObject.transform.position);
        player2.gameObject.transform.position = player2Spawn.position+ new Vector3(0f, player2.yOffset, 0f); ;
        Debug.Log(player2.gameObject.transform.position);

        //player2.gameObject.transform.localScale = player2.getPreferredSize();
        //player1.gameObject.transform.localScale = player1.getPreferredSize();
         
        //player1.EnableAnimator();
        //player2.EnableAnimator();


        ////TESTING:
        //player1.participateInBattle = true;
        //player2.participateInBattle = true;
        player1.onArenaEnter();
        player2.onArenaEnter();

    }
    /*
    // Update is called once per frame
    void Update() {
       // if( Input.GetKey(KeyCode.R))  End() ;
    }*/
    /// <summary>
    /// Elinditja a csata végének megkezdésért felelős WaitBeforeEnd() Coroutine-t</summary> 
    public void StartEnding()
    {

        StartCoroutine(WaitBeforeEnd());
    }
    /// <summary>
    /// Várakozik 5mp-et majd meghívja az End() függvényt</summary>
    IEnumerator WaitBeforeEnd()
    {

        yield return new WaitForSeconds(5);

        StartCoroutine(End());
    }
    /// <summary>
    /// Befejezi a csatát, menti a gyöztes egységet és betölti a sakktábla scene-t,
    /// továbbá beállítja a státusz változót a PersistencyManagerben</summary>
    private IEnumerator End()
    {
        Debug.Log("Ending started ");
        PersistencyManager.stateNumber = 1;
        player1.participateInBattle = false;
        player2.participateInBattle = false;
        Debug.Log(player1.hp);
        Debug.Log(player2.hp);
        if (player1.hp <= 0 && player2.hp <= 0)
        {
            PersistencyManager.hp = 0;
            if (player1.Vip && player2.Vip)
            {
                gameOver("No one");
                yield return new WaitForSeconds(5);
                SceneManager.LoadScene(0);
            }
            Destroy(player1.gameObject);
            Destroy(player2.gameObject);

            SceneManager.LoadScene(2);
        }
        if (player1.hp <= 0)
        {
            PersistencyManager.prefabType = player2.prefabType;
            PersistencyManager.hp = player2.hp;
            if (player1.Vip)
            {
               
                gameOver(player2.tag);
                yield return new WaitForSeconds(5);
                SceneManager.LoadScene(0);
            }
        }

        if (player2.hp <= 0)
        {
            PersistencyManager.hp = player1.hp;
            PersistencyManager.prefabType = player1.prefabType;
            if (player2.Vip)
            {
                gameOver(player1.tag);
                yield return new WaitForSeconds(5);
                SceneManager.LoadScene(0);
            }
        }
       
       
        Debug.Log(PersistencyManager.prefabType + " " + PersistencyManager.hp);
        Destroy(player1.gameObject);
        Destroy(player2.gameObject);
        if(!(player1.Vip &&player1.hp<=0) && !(player2.Vip&&player2.hp<=0))
            SceneManager.LoadScene(2);
    }
    private void gameOver(string winnersName) {
        var gameOver = Instantiate(gameOverPrefab,new Vector3(0,0,0),Quaternion.identity) as GameObject;
        gameOver.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        var text = gameOver.GetComponentInChildren<UnityEngine.UI.Text>();
        text.text = winnersName + " wins this game";
    }
}
