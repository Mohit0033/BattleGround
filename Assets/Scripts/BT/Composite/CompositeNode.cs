using System.Collections.Generic;

namespace BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        protected int currIndex = -1;
        public Node currNode;
        protected List<ConditionNode> conditionList;

        public void AddCondition(ConditionNode added)
        {
            if (conditionList == null)
            {
                conditionList = new List<ConditionNode>();
            }
            conditionList.Add(added);
        }

        public void RemoveCondition(ConditionNode removed)
        {
            if (conditionList != null)
            {
                conditionList.Remove(removed);
            }
        }

        public virtual bool CheckCondition()
        {
            if (conditionList != null)
            {
                foreach (var condition in conditionList)
                {
                    if (!condition.IsSatisfied())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
    }
    
}
