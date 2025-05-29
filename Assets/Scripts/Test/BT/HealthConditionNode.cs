public class HealthConditionNode : BTNode
{
    private Monster monster;
    private float threshold;   // Expressed as a fraction (0.5 for 50%, etc.)
    private bool checkLessThan; // If true: checks if currentHP <= threshold * maxHP; else, currentHP > threshold * maxHP

    public HealthConditionNode(Monster monster, float threshold, bool checkLessThan = true)
    {
        this.monster = monster;
        this.threshold = threshold;
        this.checkLessThan = checkLessThan;
    }

    public override NodeState Evaluate()
    {
        if (checkLessThan)
        {
            return (monster.currentHP <= threshold * monster.maxHP) ? NodeState.SUCCESS : NodeState.FAILURE;
        }
        else
        {
            return (monster.currentHP > threshold * monster.maxHP) ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}