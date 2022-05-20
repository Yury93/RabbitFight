using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBanana : MonoBehaviour
{
    [SerializeField] private float radius, startTimer, time;
    [SerializeField] private bool loop;
    [SerializeField] private GameObject prefab;
    private void Start()
    {
        if (!loop)
        {
            Spawn();
        }
    }
    private void Update()
    {
            if (loop)
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    Spawn();
                    time = startTimer;
                }
            }
    }
    private void Spawn()
    {
        var rnd = RandomCircle(transform.position, Random.Range(0, radius));
        var obj = PhotonNetwork.Instantiate(prefab.name, rnd, Quaternion.identity);
    }
    public Vector3 RandomCircle(Vector3 center, float radius)
    {
        var ang = Random.value * 360;
        Vector3 pos = new Vector3();
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
    
}
