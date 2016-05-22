using UnityEngine;
//using System.Collections.Generic;


public static class InputSettings {
    //temporary sollution util i can find a better, would be less code with dictionary, but harder calls
    // Use this for initialization
   
    public static string P1_action1Button = "P1_Act1";
    public static string P1_action2Button = "P1_Act2";
    public static string P1_upButton = "P1_Up";
    public static string P1_downButton = "P1_Down";
    public static string P1_leftButton = "P1_Left";
    public static string P1_rightButton = "P1_Right";

    public static string P2_action1Button = "P2_Act1";
    public static string P2_action2Button = "P2_Act2";
    public static string P2_upButton = "P2_Up";
    public static string P2_downButton = "P2_Down";
    public static string P2_leftButton = "P2_Left";
    public static string P2_rightButton = "P2_Right";

    public static string getAction1Button(string tag)
    {
        if (tag == "Player1") return P1_action1Button;
        if (tag == "Player2") return P2_action1Button;
        Debug.Log("Tag mismatch in inputSettings " + tag);
        return "";
    }
    public static string getAction2Button(string tag)
    {
        if (tag == "Player1") return P1_action2Button;
        if (tag == "Player2") return P2_action2Button;
        Debug.Log("Tag mismatch in inputSettings " + tag);
        return "";
    }
    public static string getUpButton(string tag)
    {
        if (tag == "Player1") return P1_upButton;
        if (tag == "Player2") return P2_upButton;
        Debug.Log("Tag mismatch in inputSettings " + tag);
        return "";
    }
    public static string getDownButton(string tag)
    {
        if (tag == "Player1") return P1_downButton;
        if (tag == "Player2") return P2_downButton;
        Debug.Log("Tag mismatch in inputSettings " + tag);
        return "";
    }
    public static string getLeftButton(string tag)
    {
        if (tag == "Player1") return P1_leftButton;
        if (tag == "Player2") return P2_leftButton;
        Debug.Log("Tag mismatch in inputSettings " + tag);
        return "";
    }
    public static string getRightButton(string tag)
    {
        if (tag == "Player1") return P1_rightButton;
        if (tag == "Player2") return P2_rightButton;
        Debug.Log("Tag mismatch in inputSettings " + tag);
        return "";
    }
 

}
