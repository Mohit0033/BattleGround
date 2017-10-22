using BehaviorTree;
using UnityEngine;

public class MoveAction : ActionNode
{
    private BlackBoard board;
    private string key;

    public MoveAction(NPCSoldier npc, BlackBoard blackBoard, string key) : base(npc)
    {
        board = blackBoard;
        this.key = key;
    }

    public override NodeState Tick()
    {
        Vector3 pos;
        if (board.GetValue(key, out pos))
        {
            if (Vector3.Distance(soldier.transform.position, pos) > 0.1f)
            {
                soldier.Move(pos);
                state = NodeState.Running;
            }
            else
            {
                soldier.Stop();
                state = NodeState.Completed;
            }
        }

        return state;
    }

}
