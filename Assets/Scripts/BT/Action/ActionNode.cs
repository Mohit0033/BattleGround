namespace BehaviorTree
{
    public abstract class ActionNode : Node
    {
        protected NPCSoldier control;

        public ActionNode(NPCSoldier npcControl)
        {
            control = npcControl;
        }
    }
}

