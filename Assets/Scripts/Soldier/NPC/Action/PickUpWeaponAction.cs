using BehaviorTree;
using UnityEngine;

public class PickUpWeaponAction : ActionNode
{
    private BlackBoard board;
    private string key;

    public PickUpWeaponAction(NPCSoldier npcSoldier, BlackBoard blackBoard, string key) : base(npcSoldier)
    {
        board = blackBoard;
        this.key = key;
    }

    public override NodeState Tick()
    {
        GameObject weapon;
        if (board.GetValue(key,out weapon))
        {
            soldier.PickUpWeapon(weapon);
            soldier.SetEquipment(true);
            state = NodeState.Completed;
        }

        return state;
    }
}

