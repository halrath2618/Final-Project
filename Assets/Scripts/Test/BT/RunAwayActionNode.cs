public class RunAwayActionNode : BTNode
{
    private Monster monster;

    public RunAwayActionNode(Monster monster)
    {
        this.monster = monster;
    }

    public override NodeState Evaluate()
    {
        monster.RunAway();
        return NodeState.SUCCESS;
    }
}