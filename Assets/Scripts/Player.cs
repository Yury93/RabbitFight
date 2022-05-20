using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destructible
{
    [SerializeField] private int teamId;
    public int TeamId => teamId;
    [SerializeField] private PhotonView view;
    public PhotonView ViewPlayer => view;
    public int hp;
    [SerializeField] private CharacterController charContrl;
    private Vector3 pos;
    private void Awake()
    {
        pos = transform.position;
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        { hp = CurrentHp;}
        if (view.IsMine)
        {
          var  cam = FindObjectOfType<CinemachineVirtualCamera>();
            cam.Follow = gameObject.GetComponent<Transform>();
            cam.LookAt = gameObject.GetComponent<Transform>();
            
        }
    }
    private void Update()
    {
        hp = CurrentHp;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       
        if(hit.collider.GetComponent<Banana>())
        {
            AudioManager.Instance.AudioPlay("banan");
            var plUi = GetComponent<PlayerUi>();
            if (plUi)
            {
                plUi.UpdateScore(1);
            }
            //SetHp();
                print(CurrentHp);
            hit.collider.GetComponent<Banana>().OnDest();
            transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);
        }
    }
    
}
