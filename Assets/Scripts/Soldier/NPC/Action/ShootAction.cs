using UnityEngine;
using BehaviorTree;

public class ShootAction : ActionNode
{
    private BlackBoard board;
    private string key;
    private Vector3 shootPos;

    public ShootAction(NPCSoldier npc, BlackBoard blackBoard, string key) : base(npc)
    {
        board = blackBoard;
        this.key = key;
        shootPos = new Vector3(0, 0.8f, 0);
    }

    public override NodeState Tick()
    {
        Vector3 pos;
        if (board.GetValue(key, out pos))
        {
            control.Fire(pos + shootPos);
        }

        return NodeState.Running;
    }
}
