using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    private int index;
    public float switchDelay = 1f;
    private bool isSwitching;
    PhotonView view;
    void Start()
    {
        InitializeWeapons();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
            {
                index++;
                if (index >= weapons.Length)
                {
                    index = 0;
                }
                StartCoroutine(switchWeaponDelay(index));
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
            {
                index--;
                if (index < 0)
                {
                    index = weapons.Length - 1;
                }
                StartCoroutine(switchWeaponDelay(index));
            }
        }
    }
    IEnumerator switchWeaponDelay(int nIndex)
    {
        isSwitching = true;
        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
        SwitchWeapons(nIndex);
    }
    void InitializeWeapons()
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
    }
    void SwitchWeapons(int nIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[nIndex].SetActive(true);
    }
}
