using UnityEngine;

/// <summary>
/// Gameobjectek mozgatását végző script, pl: nyilak,tüskék,lövedékek </summary>
public class Mover : MonoBehaviour
{
    /// <summary>
    /// A gameobjecft rigid body componense, ezen keresztül mozgatja a keretrendszer</summary>
    private Rigidbody rb;
    /// <summary>
    /// Mozgás sebessége</summary>
    public float speed;
    /// <summary>
    /// lekéri a rigidbody komponenst, és beállítja a hozzá tartozó sebesség vektort</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
      rb.velocity = transform.forward * speed;
    }

   
}
