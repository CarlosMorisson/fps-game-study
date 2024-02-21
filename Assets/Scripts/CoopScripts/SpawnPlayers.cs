using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerPrefab;
    [SerializeField]
    private float minX, maxX, minZ, maxZ;
    void Start()
    {
        Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 30f, Random.Range(minZ, maxZ));
        Photon.Pun.PhotonNetwork.Instantiate(PlayerPrefab.name, randomPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
