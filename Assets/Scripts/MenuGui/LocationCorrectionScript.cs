using UnityEngine;
using System.Collections;

public class LocationCorrectionScript : MonoBehaviour
{
    public Vector3 OriginalPositon;
    public float worldScreenHeight;
    public float worldScreenWidth;
    public float width = 1500f;
    public float height = 750f;
    // Use this for initialization
    void Start()
    {
        OriginalPositon = transform.localPosition;
       
    }

    // Update is called once per frame
    void Update()
    {

        worldScreenHeight = Screen.height;  
        worldScreenWidth = Screen.width; 

        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, transform.localScale.z);

        transform.localPosition = new Vector3(OriginalPositon.x * (worldScreenWidth / width), OriginalPositon.y * (worldScreenHeight / height), OriginalPositon.z);


    }
}
