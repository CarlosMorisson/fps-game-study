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
    [Header("Efeitos")]
    [SerializeField]
    private ParticleSystem _fireEffect;
    [SerializeField]
    private GameObject _hitEffect;
    [SerializeField]
    private GameObject _bulletImpact;
    public Transform FirePoint;
    [SerializeField]
    private Transform InstantiatedsObjects;
    private bool _isReloading;
    [Header("Audio")]
    [SerializeField]
    private AudioClip _shootSound;
    private AudioSource _audioSource;
    [Header("Aim")]
    private Vector3 _originalPos;
    [SerializeField]
    private Vector3 _aimPos;
    [SerializeField]
    private float _aimSpeed;
    public enum ShootMode
    {
        Auto,
        Semi
    }
    public ShootMode shootMode;
    private bool shootInput;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _currentBullets = _totalBullets;
        anim = GetComponent<Animator>();
        _originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        switch (shootMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;
            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        }
        if (shootInput) 
        {
            if (_currentBullets > 0)
                Fire();
            else
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
        ToAim();
    }
    private void Fire()
    {
        if (_fireTimer < _fireRate || _isReloading || _currentBullets<0)
            return;
        
        RaycastHit hit;
        if(Physics.Raycast(FirePoint.position, FirePoint.transform.forward, out hit, _range))
        {
            GameObject hitParticle = Instantiate(_hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), InstantiatedsObjects);
            GameObject bullet = Instantiate(_bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), InstantiatedsObjects);

            Destroy(hitParticle, 1f);
            Destroy(bullet, 5f);
        }
        anim.CrossFadeInFixedTime("Fire", 0.02f);
        _fireEffect.Play();
        PlayShootSound();
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
    public void ToAim()
    {
        if(Input.GetButton("Fire2") && !_isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _aimPos, Time.deltaTime * _aimSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _originalPos, Time.deltaTime * _aimSpeed);
        }
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
    void PlayShootSound()
    {
        _audioSource.PlayOneShot(_shootSound);
    }
}
