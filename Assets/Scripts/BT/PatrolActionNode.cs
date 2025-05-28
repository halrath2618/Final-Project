public class PatrolActionNode : BTNode
{
    private Monster monster;

    public PatrolActionNode(Monster monster)
    {
        this.monster = monster;
    }

    public override NodeState Evaluate()
    {
        monster.Patrol();
        return NodeState.RUNNING;  // Patrol is a continuous process.
    }
}