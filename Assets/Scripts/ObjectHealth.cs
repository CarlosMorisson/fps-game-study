using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private ParticleSystem _destroyParticle;
    public void ApplyDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _destroyParticle.Play();
            Destroy(gameObject,2f);
        } 
    }
}
