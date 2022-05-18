using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject uIElements;
    [SerializeField] private Text hpTxt, scoreTxt;
    [SerializeField] private int score;
    private Player player;
    private Destructible dest;
    void Start()
    {
        player = GetComponent<Player>();
        if (player.ViewPlayer.IsMine)
        {
            uIElements.SetActive(true);
        }
        else
        {
            uIElements.SetActive(false);
        }
        dest = player.GetComponent<Destructible>();
        hpTxt.text = dest.CurrentHp.ToString();
        dest.OnDamageEvent(UpdateTxtHp);
        scoreTxt.text = score.ToString();
    }
    public void UpdateTxtHp()
    {
        //if (player.ViewPlayer.IsMine)
        //{
            hpTxt.text = dest.CurrentHp.ToString();
        //}
    }
}
