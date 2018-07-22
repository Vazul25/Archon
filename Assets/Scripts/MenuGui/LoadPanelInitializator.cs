using UnityEngine;
using System.Collections;
using System.IO;

public class LoadPanelInitializator : MonoBehaviour {
    public GameObject loadButtonPrefab;
     
	// Use this for initialization
	void Start () {
        Debug.Log(Application.dataPath);
      
        DirectoryInfo d = new DirectoryInfo(Path.Combine(Application.dataPath,"Saves"));
        Debug.Log(d.Name);
        FileInfo[] Files = d.GetFiles("*.bin"); //Getting Text files
        
        foreach (FileInfo file in Files)
        {
            
            var aktButton=Instantiate(loadButtonPrefab,Vector3.one,Quaternion.identity) as GameObject;
            
            var buttonText= aktButton.GetComponentInChildren<UnityEngine.UI.Text>();
           
            buttonText.text = file.Name;
            
            var rectTransform = aktButton.GetComponent<RectTransform>();
            rectTransform.parent = this.gameObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition   = new Vector3(0f,0f,1f);
        }
    }
	
	 
}
