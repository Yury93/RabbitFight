using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviourPunCallbacks
{
    [SerializeField] private int currentHp;
    public int CurrentHp => currentHp;
    [SerializeField] private bool increadible;

    public event Action OnDamage;
   
    public void ApplyDamage(int damage)
    {
        if (!increadible)
        {
            currentHp -= damage;
            OnDamage?.Invoke();
            if (currentHp <= 0)
            {
                var viewPh = GetComponent<PhotonView>();
                if (viewPh)
                {
                    viewPh.RPC("RPC_DestroyGo", RpcTarget.All);
                }
            }
        }
    }
    [PunRPC]
    public void RPC_DestroyGo()
    {
        Destroy(gameObject);
    }
    public void SetIncreadible(bool incread)
    {
        increadible = incread;
    }
    public void OnDamageEvent(Action damage)
    {
        OnDamage += damage;
    }
}
