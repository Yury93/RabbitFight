using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    ///TODO: ÏÎÑÌÎÒÐÅÒÜ ÂÈÄÅÎ, ÑÄÅËÀÒÜ ÒÀÉÌÅÐ ÍÀ ÐÀÍÄÎÌÍÎÅ ÊÎËÈ×ÅÑÒÂÎ Î×ÊÎÂ ÄËß ÏÎÁÅÄÛ ÎÒ 100 ÄÎ 200
    [SerializeField] private CharacterController charController;
    [SerializeField] private Joystick joystick;
    private Vector3 inputVector;
    [SerializeField] private Button buttonAttack, buttonSlope;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private Text hpTxt;
    public enum State
    {
        idle,
        move,
        attack,
        slope,
    }
    [SerializeField] private State currentState, lateState;
    public State CurrentState => currentState;
    [SerializeField] private float timerStateTransitions;
    private Player player;
    private float startTimer;
    public event Action OnChangeState;
    private Vector3 constPos;

    private void Start()
    {
        player = GetComponent<Player>();
        //if (player.ViewPlayer.IsMine)
        //{
        joystick = GameContainer.Instance.PlayerJoystick;
        buttonAttack = GameContainer.Instance.ButtonAttack;
        buttonSlope = GameContainer.Instance.ButtonSlope;

        buttonSlope.onClick.AddListener(SlopeMode);//íå çàáûòü îòïèñàòüñÿ
        buttonAttack.onClick.AddListener(Attack);

        player = GetComponent<Player>();
        startTimer = timerStateTransitions;
        constPos = transform.position;
        //}

    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, constPos.y, transform.position.z);
        
        if (player.ViewPlayer.IsMine)
        {
            transform.position = new Vector3(transform.position.x, constPos.y, transform.position.z);
            if (currentState != lateState)//ñìåíà ñîñòîÿíèÿ
            {
                OnChangeState?.Invoke();
                lateState = currentState;
            }
            if (currentState == State.idle)
            {
                inputVector = new Vector3(joystick.Horizontal,
                 0, joystick.Vertical);
                if (inputVector != Vector3.zero)
                {
                    currentState = State.move;
                }
                if (!buttonAttack.interactable)
                {
                    buttonAttack.interactable = true;
                    buttonSlope.interactable = true;
                }
            }
            if (currentState == State.move)
            {
                inputVector = new Vector3(joystick.Horizontal,
                 0, joystick.Vertical);
                if (inputVector == Vector3.zero)
                {
                    currentState = State.idle;
                }
                else
                {
                    Move();
                }
                if (!buttonAttack.interactable)
                {
                    buttonAttack.interactable = true;
                    buttonSlope.interactable = true;
                }
            }
            if (currentState == State.attack)
            {
                timerStateTransitions -= Time.deltaTime;
                if (timerStateTransitions <= 0)
                {
                    player.SetIncreadible(false);

                    currentState = State.idle;

                    timerStateTransitions = startTimer;
                }
            }
            if (currentState == State.slope)
            {
                timerStateTransitions -= Time.deltaTime;
                if (timerStateTransitions <= 0)
                {
                    player.SetIncreadible(false);

                    currentState = State.idle;

                    timerStateTransitions = startTimer;
                }
            }
        }
    }
    private void Move()
    {
        charController.Move(inputVector * speed * Time.deltaTime);
        transform.LookAt(transform.position + inputVector);
    }
    public void Attack()
    {
        currentState = State.attack;

        //player.ViewPlayer.RPC("RPC_Attack", RpcTarget.All);
        RPC_Attack();
        AudioManager.Instance.AudioPlay("kickAir");
        buttonAttack.interactable = false;
        buttonSlope.interactable = false;
    }
    //[PunRPC]
    public void RPC_Attack()
    {
        RaycastHit hit;
        

        if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, 10f))
        {
            var dest = hit.collider.transform.root.GetComponent<Destructible>();
            if (dest != null && !dest.GetComponent<PhotonView>().IsMine)
            {
                AudioManager.Instance.AudioStop("kickAir");
                AudioManager.Instance.AudioPlay("kick");
                AudioManager.Instance.AudioPlay("damage");
                var plUi = GetComponent<PlayerUi>();
                if (dest.CurrentHp < 15)
                {
                    print("ÇÀÉÊÀ ÓØÅË!");
                    var effect = PhotonNetwork.Instantiate(GameContainer.Instance.EffectDeath.name,
                        dest.transform.position,
                        Quaternion.identity);
                    StartCoroutine(CorDestroy());
                    IEnumerator CorDestroy()
                    {
                        yield return new WaitForSeconds(2f);
                        PhotonNetwork.Destroy(effect);
                    }
                    plUi.UpdateScore(50);
                }

                dest.ApplyDamage(damage,dest.gameObject);
      
            }
        }
       
    }
  
    public void SlopeMode()
    {
        currentState = State.slope;
        player.SetIncreadible(true);
        buttonAttack.interactable = false;
        buttonSlope.interactable = false;
    }
}
