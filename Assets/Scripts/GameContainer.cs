using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameContainer : SingletonBase<GameContainer>
{
    [SerializeField] private Joystick joystick;
    public Joystick PlayerJoystick => joystick;
    [SerializeField] private Button buttonAttack;
    public Button ButtonAttack => buttonAttack;
    [SerializeField] private Button buttonSlope;
    public Button ButtonSlope => buttonSlope;
}
