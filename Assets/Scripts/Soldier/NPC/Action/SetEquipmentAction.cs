using BehaviorTree;

public class SetEquipmentAction : ActionNode
{
    private bool equip;
    private bool done = false;

    public SetEquipmentAction(NPCSoldier npc, bool equip) : base(npc)
    {
        this.equip = equip;
    }

    public override NodeState Tick()
    {
        if (done)
        {
            state = NodeState.Completed;
            done = false;
        }
        else
        {
            control.SetEquipment(equip);
            state = NodeState.Running;
            done = true;
        }

        return state;
    }
}
