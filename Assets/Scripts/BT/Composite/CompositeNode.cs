namespace BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        protected int currIndex = -1;
        public Node currNode;
        
        public abstract bool CheckCondition();
        
    }
    
}
