using System.Collections.Generic;

namespace BehaviorTree
{
    public class SelectionNode : CompositeNode
    {
        private List<CompositeNode> CompNodeList;

        public void AddNode(CompositeNode added)
        {
            if (CompNodeList == null)
            {
                CompNodeList = new List<CompositeNode>();
            }
            CompNodeList.Add(added);
        }

        public void RemoveNode(CompositeNode removed)
        {
            if (CompNodeList != null)
            {
                CompNodeList.Remove(removed);
            }
        }

        public override NodeState Tick()
        {
            for (int i = 0; i < CompNodeList.Count; i++)
            {
                var compNode = CompNodeList[i];
                if (compNode.CheckCondition())
                {
                    if (i != currIndex)
                    {
                        if (currNode != null)
                        {
                            currNode.Reset();
                        }
                        currIndex = i;
                        currNode = CompNodeList[i];
                    }
                    break;
                }
            }
            
            return currNode.Tick();
        }

        public override void Reset()
        {
            currIndex = -1;
            currNode = null;
            foreach (var node in CompNodeList)
            {
                node.Reset();
            }
        }
    }
}
