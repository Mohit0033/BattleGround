namespace BehaviorTree
{
    public abstract class ActionNode : Node
    {
        protected NPCSoldier soldier;

        public ActionNode(NPCSoldier npcSoldier)
        {
            soldier = npcSoldier;
        }
    }
}

