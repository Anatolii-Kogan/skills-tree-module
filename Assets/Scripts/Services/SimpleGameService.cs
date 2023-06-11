using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills.Core
{
    public class SimpleGameService : ApplicationService
    {
        public event Action<SkillTreeNode> OnDataLoaded;

        public void HandleDataLoaded(SkillTreeNode baseSkillNode) => OnDataLoaded?.Invoke(baseSkillNode);
    }
}