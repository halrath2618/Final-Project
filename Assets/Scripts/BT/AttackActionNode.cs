public class AttackActionNode : BTNode
{
    private Monster monster;

    public AttackActionNode(Monster monster)
    {
        this.monster = monster;
    }

    public override NodeState Evaluate()
    {
        monster.NormalAttack();
        return NodeState.SUCCESS;
    }
}