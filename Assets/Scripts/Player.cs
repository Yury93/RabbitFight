using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destructible
{
    [SerializeField] private int teamId;
    [SerializeField] private PhotonView view;
    public PhotonView ViewPlayer => view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
}
