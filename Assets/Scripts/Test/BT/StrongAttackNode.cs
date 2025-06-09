public class StrongAttackActionNode : BTNode
{
    private Monster monster;

    public StrongAttackActionNode(Monster monster)
    {
        this.monster = monster;
    }

    public override NodeState Evaluate()
    {
        monster.Attack();
        return NodeState.SUCCESS;
    }
}