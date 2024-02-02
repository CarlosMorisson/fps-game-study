using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public static Enemie instance;
    public float Life;
    public float Speed;
    public Transform Target;
    
    void Start()
    {
        instance = this;
    }

    public void ReceiveDamage(float damage)
    {
        Life -= damage;

    }
    void Update()
    {
        
    }
}
