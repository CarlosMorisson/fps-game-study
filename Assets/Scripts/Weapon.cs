using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Bullets and Fire")]
    [SerializeField]
    private float _range = 100f;
    [SerializeField]
    private int _totalBullets = 30;
    [SerializeField]
    private int _bulletsLeft;
    //
    [SerializeField]
    private float _fireRate = 0.1f;
    [SerializeField]
    private float _fireTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
        if (_fireTimer < _fireRate)
        {
            _fireTimer += Time.deltaTime;
        }
    }
    private void Fire()
    {
        if (_fireTimer < _fireRate)
        {
            return;
        }
        _fireTimer = 0f;
    }
}
