using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Plug and play AI that follows and attacks a target if it is within line of sight
/// </summary>
[CreateAssetMenu(menuName = "Modules/AI/Follow And Attack If In Sight")]
public class FollowAndAttackIfInSight : EnemyAI
{
    public override void Tick(GameObject self, GameObject target, GameObject allyTarget, NavMeshAgent agent, Movement movement, EnemyGeneric enemyGeneric = null)
    {
        bool canSeeTarget = Helpers.CheckLineOfSight(self.transform, target.transform, self.GetComponent<EnemyGeneric>().visionRange);

        if(canSeeTarget)
        {
            agent.SetDestination(target.transform.position);
        } 

        else
        {
            agent.ResetPath();
        }
    }
}
