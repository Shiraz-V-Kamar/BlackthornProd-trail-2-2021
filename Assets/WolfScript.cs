using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class WolfScript : MonoBehaviour
{
    public enum State { Idle, triggered, stay, chase, kill, Retreat}

    public NavMeshAgent enemyagent ;
    public State state = default;

    public bool isOutOfRange;
    public bool isInRange;
    public bool Retreat;

}
