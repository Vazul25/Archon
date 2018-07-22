using UnityEngine;
using System.Collections;

public class InheritPosition : MonoBehaviour {
    public Transform parent;
    public Transform initial;
    void Awake() {   }
    void LateUpdate() {
        if (parent != null) this.transform.localPosition = parent.position+initial.localPosition;
    }
}
