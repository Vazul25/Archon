using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour
{
    public Vector3 speed;
    private bool expanding = true;
    public bool stopOnAct1Release;
    // Use this for initialization

    // Update is called once per frame

    void Update()
    {
        if (expanding)
        {
            if (stopOnAct1Release)
            {
                if (gameObject.tag == "Untagged" || Input.GetButton(InputSettings.getAction1Button(gameObject.tag.Substring(0, 7)))) transform.localScale += speed * Time.deltaTime;
                else expanding = false;
            }
            else transform.localScale += speed * Time.deltaTime;
        }
        
    }
}
