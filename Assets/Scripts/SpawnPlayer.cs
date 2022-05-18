using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] posSpawn; 
    [SerializeField] private Button createButton;
    [SerializeField] private List<GameObject> players;
    public List<GameObject> PlayersList => players;
    private bool findPlayer;
    private PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        createButton.gameObject.SetActive(true);
    }
    public void CreatePlayer()
    {
        view.RPC("Find", RpcTarget.All);
        PhotonNetwork.Instantiate(player.name, posSpawn[Random.Range(0,posSpawn.Length)].position, Quaternion.identity);
        createButton.gameObject.SetActive(false);
    }
    [PunRPC]
    public void Find()
    {
        findPlayer = true;
    }
    public void Update()
    {
        if (findPlayer)
        {
            players.Clear();
            var plArray = FindObjectsOfType<Player>();
            foreach (var pl in PhotonNetwork.PlayerList)
            {
                for (int i = 0; i < plArray.Length; i++)
                {
                    int id = plArray[i].GetComponent<PhotonView>().OwnerActorNr;
                    if (id == pl.ActorNumber)
                    {
                        players.Add(plArray[i].gameObject);
                    }
                }
            }
            for (int i = 0; i < players.Count; i++)
            {
                print(players[i].gameObject.GetComponent<PhotonView>().OwnerActorNr);
            }

            findPlayer = false;
        }
    }
}
