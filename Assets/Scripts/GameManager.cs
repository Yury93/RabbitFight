using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>,IPunObservable
{
    [SerializeField] private PhotonView view;
    [SerializeField] private SpawnPlayer spawnPlayer;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private Button buttonExit;
    [SerializeField] private int rndScore;
    [SerializeField] private float timer ;
    [SerializeField] private Text timerTxt;
    [SerializeField] private bool endLvl = false;
    [SerializeField] private Text resultTxt;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        spawnPlayer.OnUpdateListPlayer += ListPlayer;
        rndScore = UnityEngine.Random.Range(100, 200);
        resultTxt.gameObject.SetActive(false);
        timer = rndScore;
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    [PunRPC]
    private void RandScore(int i)
    {
        rndScore = i;
    }

    public void ListPlayer()
    {
        players = spawnPlayer.PlayersList;
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].gameObject)
            {
                view.RPC("RandScore", RpcTarget.AllBuffered, rndScore);
                players[i].GetComponent<PlayerUi>().SetScoreLvl(rndScore);
            }
        }
    }
    public void ExitMenu()
    {
        Photon.Pun.PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }
    private void Update()
    {
        if(players.Count > 1)
        {
            if (endLvl == false)
            {
                if (timer <= 0)
                {
                    timerTxt.text = "Timer: 0";
                    foreach (var Pl in players)
                    {
                        if (Pl)
                        {
                            var plScore = Pl.GetComponent<PlayerUi>().Score;

                            if (plScore < rndScore)
                            {
                                print("lose");
                                resultTxt.gameObject.SetActive(true);
                                resultTxt.text = "You lost!";
                                StartCoroutine(CorExit());
                                IEnumerator CorExit()
                                {
                                    yield return new WaitForSeconds(4f);
                                    ExitMenu();
                                }
                                endLvl = true;
                            }
                            else if (plScore >= rndScore)
                            {
                                print("win");
                                resultTxt.gameObject.SetActive(true);
                                resultTxt.text = "You win!";

                                StartCoroutine(CorExit());
                                IEnumerator CorExit()
                                {
                                    yield return new WaitForSeconds(4f);
                                    ExitMenu();
                                }
                                endLvl = true;
                            }
                        }
                    }
                }
                else
                {
                    timer -= Time.deltaTime;
                    timerTxt.text = "Timer: "+((int)timer).ToString();
                }
            }
            else//через 4 секунды будет загружена новы€ сцена
            {

            }
        }
        else if(players.Count>0)
        {
            foreach(var pl in players)
            {
                pl.GetComponent<PlayerUi>().ScoreNull();
            }
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timer);
        }
        else if (stream.IsReading)
        {
            timer = (float)stream.ReceiveNext();
        }
    }
}
