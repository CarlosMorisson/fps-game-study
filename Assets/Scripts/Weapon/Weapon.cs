using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class Weapon : MonoBehaviour
{
    [Header("Bullets and Fire")]
    [SerializeField]
    [Range(0, 10000)]
    private float _hitForce;
    [SerializeField]
    private float _range = 100f;
    [SerializeField]
    private int _totalBullets = 30;
    [SerializeField]
    private int _bulletsLeft=100;
    [SerializeField]
    private int _currentBullets;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _sprayShoot;
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
    [Header("UI Components")]
    [SerializeField]
    private TextMeshProUGUI _bulletText;
    public PhotonView view;
    public enum ShootMode
    {
        Auto,
        Semi
    }
    public ShootMode shootMode;
    private bool shootInput;
    void Start()
    {
        if (view.IsMine)
        {
            _audioSource = GetComponent<AudioSource>();
            _currentBullets = _totalBullets;
            anim = GetComponent<Animator>();
            _originalPos = transform.localPosition;
            UpdateTextAmmo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(view);
        if (view.IsMine)
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
                if (_currentBullets < _totalBullets && _bulletsLeft > 0)
                {
                    DoReload();
                }
            }
            ToAim();
        }
        
    }
    private void Fire()
    {
        if (_fireTimer < _fireRate || _isReloading || _currentBullets<0)
            return;
        
        RaycastHit hit;
        Vector3 shootDirection = FirePoint.transform.forward;
        shootDirection = shootDirection + FirePoint.TransformDirection(new Vector3(Random.Range(-_sprayShoot, _sprayShoot), Random.Range(-_sprayShoot, _sprayShoot)));
        if(Physics.Raycast(FirePoint.position, shootDirection, out hit, _range))
        {
            GameObject hitParticle = Instantiate(_hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bullet = Instantiate(_bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            //
            bullet.transform.SetParent(hit.transform);
            Destroy(hitParticle, 1f);
            Destroy(bullet, 5f);
            if (hit.transform.GetComponent<ObjectHealth>())
                hit.transform.GetComponent<ObjectHealth>().ApplyDamage(_damage);
            if (hit.transform.CompareTag("Foot"))
            {
                hit.transform.GetComponent<EnemieMember>().MemberDamage(_damage, -hit.normal);
            }
            if (hit.transform.GetComponent<Rigidbody>() != null)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(-hit.normal * _hitForce);
            }
        }
        anim.CrossFadeInFixedTime("Fire", 0.02f);
        _fireEffect.Play();
        PlayShootSound();   
        UpdateTextAmmo();
        _currentBullets--;
        _fireTimer = 0f;
    }
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            _isReloading = info.IsName("Reload");
        }
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
        UpdateTextAmmo();   
    }
    void PlayShootSound()
    {
        _audioSource.PlayOneShot(_shootSound);
    }
    void UpdateTextAmmo()
    {
        _bulletText.text = _currentBullets + " / " + _bulletsLeft;
    }
}
