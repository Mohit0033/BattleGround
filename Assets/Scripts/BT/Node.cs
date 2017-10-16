namespace BehaviorTree
{
    public enum NodeState
    {
        Running,
        Completed
    }

    public abstract class Node
    {
        protected NodeState state = NodeState.Completed;

        public abstract NodeState Tick();

        public virtual void Reset()
        {
            state = NodeState.Completed;
        }
    }
}

