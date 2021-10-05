using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/ChaseTargetState")]
public class ChaseTargetState : State
{
    public override string name
    {
        get
        {
            return "ChaseTargetState";
        }
    }

    public override void OnEnter(GameObject self, GameObject target, NavMeshAgent agent, Movement movement)
    {
        Debug.Log("Enemy " + self.GetInstanceID() + " has entered " + name);
    }

    public override void OnExit(GameObject self, GameObject target, NavMeshAgent agent, Movement movement)
    {
        agent.ResetPath();
    }

    public override void Tick(GameObject self, GameObject target, NavMeshAgent agent, Movement movement)
    {
        agent.SetDestination(target.transform.position);
    }
}