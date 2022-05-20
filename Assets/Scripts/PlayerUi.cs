using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject uIElements;
    [SerializeField] private Text  scoreTxt;
    [SerializeField] private int score;
    public int Score => score;
    [SerializeField] private int scoreLvl;
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

        scoreTxt.text = $"{score}/{scoreLvl}";
    }
    
    
    public void UpdateScore(int i)
    {
        score+=i;
        scoreTxt.text = $"{score}/{scoreLvl}";
    }
    public void SetScoreLvl(int i)
    {
        scoreLvl = i;
    }
    public void ScoreNull()
    {
        score = 0;
        scoreTxt.text = $"{score}/{scoreLvl}";
    }
}
