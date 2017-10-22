using BehaviorTree;
using UnityEngine;

public class TurnAction : ActionNode
{
    private BlackBoard board;
    private string key;

    public TurnAction(NPCSoldier npcControl, BlackBoard blackBoard, string key) : base(npcControl)
    {
        board = blackBoard;
        this.key = key;
    }

    public override NodeState Tick()
    {
        Vector3 pos;
        if (board.GetValue(key, out pos))
        {
            var dir = pos - soldier.transform.position;
            var desiredAngle = Vector3.Angle(soldier.transform.forward, dir);
            if (Mathf.Abs(desiredAngle) > 10)
            {
                var normal = Vector3.Cross(soldier.transform.forward, dir);
                var angle = Mathf.Lerp(0, desiredAngle, 1f);
                angle *= Mathf.Sign(Vector3.Dot(Vector3.up, normal));
                soldier.Turn(angle);
                state = NodeState.Running;
            }
            else
            {
                state = NodeState.Completed;
            }
        }
        return state;

    }
}
