using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Com.OfTomorrowInc.DMShooter;

public class RoomPlaceholder : MonoBehaviour
{
    public GameObject enemy;

    public float cost;

    public int numToSpawn;

    public void SpawnEnemies()
    {
        if(DungeonMasterController.LocalPlayerInstance != null)
        {
            for(int i = 0; i < numToSpawn; i++)
            {
                GameManager.enemies.Add(PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity));
            }
        }

        Destroy(gameObject);
    }
}
