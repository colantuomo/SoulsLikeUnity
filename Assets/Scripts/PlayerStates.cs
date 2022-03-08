using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerManager
{
    public enum States
    {
        Idle,
        Walking,
        Attacking,
        Blocking
    }

    public class PlayerStates : MonoBehaviour
    {
        public static States currentState = States.Idle;
    }
}
