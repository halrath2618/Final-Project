using System.Collections.Generic;

public class SelectorNode : BTNode
{
    private List<BTNode> children;

    public SelectorNode(List<BTNode> nodes)
    {
        children = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in children)
        {
            var result = node.Evaluate();
            if (result != NodeState.FAILURE)
            {
                return result;
            }
        }
        return NodeState.FAILURE;
    }
}