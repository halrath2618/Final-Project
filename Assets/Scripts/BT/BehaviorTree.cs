using System.Collections.Generic;

public class BehaviorTree
{
    private BTNode rootNode;

    public BehaviorTree(List<BTNode> nodes)
    {
        // The root is a selector: try attack branch first, then patrol.
        rootNode = new SelectorNode(nodes);
    }

    public BTNode.NodeState Evaluate()
    {
        return rootNode.Evaluate();
    }
}