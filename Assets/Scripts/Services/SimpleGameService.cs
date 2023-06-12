using System;

namespace Skills.Core
{
    public class SimpleGameService : ApplicationService
    {
        public event Action<SkillTreeNode> OnDataLoaded;

        public void HandleDataLoaded(SkillTreeNode baseSkillNode) => OnDataLoaded?.Invoke(baseSkillNode);
    }
}