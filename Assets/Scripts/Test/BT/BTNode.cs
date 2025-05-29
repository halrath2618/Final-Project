using UnityEngine;

public abstract class BTNode
{
    public enum NodeState { RUNNING, SUCCESS, FAILURE }
    protected NodeState nodeState;
    public NodeState State => nodeState;

    public abstract NodeState Evaluate();
}