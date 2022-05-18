using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController charController;
    [SerializeField] private Joystick joystick;
    private Vector3 inputVector;
    [SerializeField] private Button buttonAttack, buttonSlope;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public enum State
    {
        idle,
        move,
        attack,
        slope,
    }
    [SerializeField] private State currentState,lateState;
    public State CurrentState => currentState;
    [SerializeField] private float timerStateTransitions;
    private Player player;
    private float startTimer;
    public event Action OnChangeState;
    private void Start()
    {
        player = GetComponent<Player>();
        //if (player.ViewPlayer.IsMine)
        //{
            joystick = GameContainer.Instance.PlayerJoystick;
        buttonAttack = GameContainer.Instance.ButtonAttack;
        buttonSlope = GameContainer.Instance.ButtonSlope;

        buttonSlope.onClick.AddListener(SlopeMode);//не забыть отписаться
        buttonAttack.onClick.AddListener(Attack);

        player = GetComponent<Player>();
        startTimer = timerStateTransitions;
        //}

    }
    private void Update()
    {
        if (player.ViewPlayer.IsMine)
        {
            if(currentState != lateState)//смена состояния
            {
                OnChangeState?.Invoke();
                lateState = currentState;
            }
            if(currentState == State.idle)
            {
                inputVector = new Vector3(joystick.Horizontal,
                 0, joystick.Vertical);
                if (inputVector!= Vector3.zero)
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
            if(currentState == State.attack)
            {
                timerStateTransitions -= Time.deltaTime;
                if(timerStateTransitions <= 0)
                {
                    player.SetIncreadible(false);
                    
                    currentState = State.idle;
                   
                    timerStateTransitions = startTimer;
                }
            }
            if(currentState == State.slope)
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
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, 10f))
        //{
        //    Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);

        //    var dest = hit.collider.transform.root.GetComponent<Destructible>();
        //    if (dest != null && dest != player.GetComponent<Destructible>())
        //    {
        //        print(dest.CurrentHp);
        //        if (dest.CurrentHp < 15)
        //        {
        //            print("ЗАЙКА УШЕЛ!");
        //        }
        //        dest.ApplyDamage(damage);
        //    }
        //}
        player.ViewPlayer.RPC("RPC_Attack", RpcTarget.All);

        buttonAttack.interactable = false;
        buttonSlope.interactable = false;
    }
    [PunRPC]
    public void RPC_Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, 10f))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);

            print(hit.collider.gameObject.name);
            var dest = hit.collider.transform.root.GetComponent<Destructible>();
            if (dest != null && dest != player.GetComponent<Destructible>())
            {
                if (dest.CurrentHp < 15)
                {
                    print("ЗАЙКА УШЕЛ!");
                }
                dest.ApplyDamage(damage);
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
