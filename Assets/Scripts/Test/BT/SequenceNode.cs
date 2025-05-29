using System.Collections.Generic;

public class SequenceNode : BTNode
{
    private List<BTNode> children;

    public SequenceNode(List<BTNode> nodes)
    {
        children = nodes;
    }

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;
        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    return NodeState.FAILURE;
                case NodeState.RUNNING:
                    anyChildRunning = true;
                    break;
                case NodeState.SUCCESS:
                    continue;
            }
        }
        return anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
    }
}