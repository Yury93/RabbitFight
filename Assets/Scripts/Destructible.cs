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
    public Action OnDamage;

   

    public void ApplyDamage(int damage, GameObject go)
    {
        if (!increadible)
        {
          var ani =  gameObject.GetComponent<PlayerAnimationController>();
            ani.Damage();
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
        Destroy(gameObject,0.5F);
    }
    public void SetIncreadible(bool incread)
    {
        increadible = incread;
    }
    public void OnDamageEvent(Action damage)
    {
        OnDamage += damage;
    }
    public void SetHp()
    {
        if(currentHp != 30)
        {
            currentHp += 10;
        }
    }
}
