using System.Collections.Generic;

namespace BehaviorTree
{
    public class SequenceNode : CompositeNode
    {
        private List<ConditionNode> conditionList;
        private List<ActionNode> actionList;

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

        public void AddNode(ActionNode added)
        {
            if (actionList == null)
            {
                actionList = new List<ActionNode>();
            }
            actionList.Add(added);
        }

        public void RemoveNode(ActionNode removed)
        {
            if (actionList != null)
            {
                actionList.Remove(removed);
            }
        }

        public override bool CheckCondition()
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

        public override NodeState Tick()
        {
            if (currIndex==-1)
            {
                currIndex = 0;
                currNode = actionList[0];
            }

            state = currNode.Tick();
            if (state == NodeState.Completed)
            {
                currNode.Reset();
                currIndex++;
                if (currIndex >= actionList.Count)
                {
                    currIndex = 0;
                    currNode = actionList[0];
                }
                else
                {
                    currNode = actionList[currIndex];
                    state = NodeState.Running;
                }
            }

            return state;
        }

        public override void Reset()
        {
            currIndex = -1;
            currNode = null;

            foreach (var node in actionList)
            {
                node.Reset();
            }
        }

    }
}
