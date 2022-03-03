using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{
    public EnemyAI ai;
    public float threshold;

    public HealthNode(EnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        return ai.currentHealth <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
