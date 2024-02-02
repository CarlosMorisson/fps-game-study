using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField]
    private float _health = 100;
    [SerializeField]
    private ParticleSystem _destroyParticle;
    public void ApplyDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _destroyParticle.Play();
            Destroy(gameObject,2f);
        } 
    }
}
