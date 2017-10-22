using BehaviorTree;

public class IdleAction : ActionNode
{

    public IdleAction(NPCSoldier npc) : base(npc)
    {
        
    }

    public override NodeState Tick()
    {
        soldier.Idle();
        return NodeState.Running;
    }
}
