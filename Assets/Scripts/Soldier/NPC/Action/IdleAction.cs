using BehaviorTree;

public class IdleAction : ActionNode
{

    public IdleAction(NPCSoldier npc) : base(npc)
    {
        
    }

    public override NodeState Tick()
    {
        control.Idle();
        return NodeState.Running;
    }
}
