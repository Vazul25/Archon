using UnityEngine;
using System.Collections;

public class HPbarUpdater : MonoBehaviour,HpBarViewChanger {
    RectTransform hpbar;
    void Awake()
    {
        hpbar = GetComponent<RectTransform>();
    }
    public void HpChanged(float newhp)
    {
        if(newhp<=0) hpbar.localScale = new Vector3(0, 1, 1);
        else hpbar.localScale = new Vector3(newhp/2,1, 1); 
    }
}
