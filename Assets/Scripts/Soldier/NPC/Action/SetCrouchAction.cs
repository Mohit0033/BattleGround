using BehaviorTree;

public class SetCrouchAction : ActionNode
{
    private bool target;

    public SetCrouchAction(NPCSoldier npc, bool target) : base(npc)
    {
        this.target = target;
    }

    public override NodeState Tick()
    {
        if (control.isCrouch == target)
        {
            state = NodeState.Completed;
        }
        else
        {
            control.SetCrouch();
            state = NodeState.Running;
        }

        return state;
    }
}
