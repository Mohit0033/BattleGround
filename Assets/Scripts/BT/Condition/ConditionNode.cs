using System;

namespace BehaviorTree
{
    public class ConditionNode
    {
        protected Func<bool> func;
        private bool goal;

        public ConditionNode(Func<bool> func, bool goal)
        {
            this.func = func;
            this.goal = goal;
        }

        public bool IsSatisfied()
        {
            if (func!=null)
            {
                return goal == func();
            }
            return false;
        }
    }
}
