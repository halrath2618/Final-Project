using UnityEngine;

public class PlayerInRangeConditionNode : BTNode
{
    private Monster monster;
    private float range;

    public PlayerInRangeConditionNode(Monster monster, float range)
    {
        this.monster = monster;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        if (monster.player != null)
        {
            float distance = Vector3.Distance(monster.transform.position, monster.player.position);
            return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        }
        return NodeState.FAILURE;
    }
}