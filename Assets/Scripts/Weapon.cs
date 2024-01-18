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
    private int _bulletsLeft=100;
    [SerializeField]
    private int _currentBullets;
    //
    private Animator anim;
    [SerializeField]
    private float _fireRate = 0.1f;
    [SerializeField]
    private float _fireTimer;
    [SerializeField]
    private ParticleSystem _fireEffect;
    public Transform FirePoint;
    private bool _isReloading;
    void Start()
    {
        _currentBullets = _totalBullets;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (_currentBullets > 0)
                Fire();
            else if (_bulletsLeft > 0)
                DoReload();
        }
        if (_fireTimer < _fireRate)
        {
            _fireTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(_currentBullets<_totalBullets && _bulletsLeft > 0)
            {
                DoReload();
            }
        }
    }
    private void Fire()
    {
        if (_fireTimer < _fireRate || _isReloading || _currentBullets<0)
            return;
        
        RaycastHit hit;
        if(Physics.Raycast(FirePoint.position, FirePoint.transform.forward, out hit, _range))
        {

        }
        anim.CrossFadeInFixedTime("Fire", 0.02f);
        _fireEffect.Play();
        _currentBullets--;
        _fireTimer = 0f;
    }
    private void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        _isReloading = info.IsName("Reload");
    }
    private void DoReload()
    {
        if (_isReloading)
            return;
        anim.CrossFadeInFixedTime("Reload", 0.02f);
    }
    public void Reload()
    {
        if (_bulletsLeft <= 0)
        {
            return;
        }
        int bulletsToLoad = _totalBullets - _currentBullets;
        int bulletsToDeduct = (_bulletsLeft>=bulletsToLoad) ? bulletsToLoad : _bulletsLeft;

        _bulletsLeft -= bulletsToDeduct;
        _currentBullets += bulletsToDeduct;
    }
}
