using UnityEngine;
using System.Collections;

public class ChimeraProjectileMovementController : MonoBehaviour
{

    string plusButton;
    string minusButton;
    string playerTag;
    public float xSpeed;
    private enum Orientation { Up,Down,Left,Right};
    private Orientation orientation;
    
    // Use this for initialization
    void Start()
    {
      
        Debug.Log(transform.eulerAngles.y);
        playerTag = tag.Substring(0, 7);
        if (Mathf.Approximately(transform.eulerAngles.y, 90) || Mathf.Approximately(transform.eulerAngles.y, -270))
        {
            minusButton = InputSettings.getDownButton(playerTag);
            plusButton = InputSettings.getUpButton(playerTag);
            orientation = Orientation.Right;
        }
        if (Mathf.Approximately(transform.eulerAngles.y, 0))
        {
            plusButton = InputSettings.getRightButton(playerTag);
            minusButton = InputSettings.getLeftButton(playerTag);
            orientation = Orientation.Up;
        }
        if (Mathf.Approximately(transform.eulerAngles.y, 180) || Mathf.Approximately(transform.eulerAngles.y, -180))
        {
            minusButton = InputSettings.getLeftButton(playerTag);
            plusButton  = InputSettings.getRightButton(playerTag);
            orientation = Orientation.Down;
        }
        if (Mathf.Approximately(transform.eulerAngles.y, 270) || Mathf.Approximately(transform.eulerAngles.y, -90))
        {
            plusButton = InputSettings.getUpButton(playerTag);
            minusButton = InputSettings.getDownButton(playerTag);
            orientation = Orientation.Left;
        }
        Debug.Log(playerTag + " x+ " + plusButton);

        Debug.Log(playerTag + " x- " + minusButton);
        Debug.Log(playerTag + " act2 " +  InputSettings.getAction2Button(playerTag)) ;

        Debug.Log(playerTag + " act2 " + Input.GetButtonDown(InputSettings.getAction2Button(playerTag)));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerTag);
        Debug.Log(InputSettings.getAction2Button(playerTag));
        Debug.Log(Input.GetButtonDown(InputSettings.getAction2Button(playerTag)));
        if (Input.GetButton(InputSettings.getAction2Button(playerTag)))
        {
            if (Input.GetButton(plusButton))
            {
                Debug.Log("XplusButton pressed");
                if (orientation==Orientation.Up || orientation==Orientation.Down)

                transform.localPosition+= new Vector3(xSpeed, 0,0)*Time.deltaTime;
                else
                    transform.localPosition += new Vector3(0, 0, xSpeed) * Time.deltaTime;
            }
            else if (Input.GetButton(minusButton))
            {
                Debug.Log("XminusButton pressed");
                if (orientation == Orientation.Up || orientation == Orientation.Down)

                    transform.localPosition += new Vector3(-xSpeed, 0, 0) * Time.deltaTime;
                else
                    transform.localPosition += new Vector3(0, 0, -xSpeed) * Time.deltaTime;
            }
            Debug.Log("nothing pressed");

        }
        else Destroy(this);


    }
}
