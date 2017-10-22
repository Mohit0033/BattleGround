using BehaviorTree;

public class SetEquipmentAction : ActionNode
{
    private bool equip;

    public SetEquipmentAction(NPCSoldier npc, bool equip) : base(npc)
    {
        this.equip = equip;
    }

    public override NodeState Tick()
    {

        if (soldier.isEquiped == equip)
        {
            state = NodeState.Completed;
        }
        else
        {
            soldier.SetEquipment(equip);
            state = NodeState.Running;
        }

        return state;
    }
}
