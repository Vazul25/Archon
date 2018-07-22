using UnityEngine;
using System.Collections;

public class LoadMenuCreator : MonoBehaviour {
    public Canvas canvas;
    public GameObject menuPrefab;
    public void createMenu() {
        
        var menu= Instantiate(menuPrefab, new Vector3(0,0,-1),Quaternion.identity) as GameObject;
        menu.GetComponent<RectTransform>().parent = canvas.GetComponent<RectTransform>();
        menu.transform.localPosition = new Vector3(0, 0, -1);
        menu.transform.localScale = new Vector3(1, 1, 1);
    }
}
