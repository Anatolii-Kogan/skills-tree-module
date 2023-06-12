using System;
using Skills.Core;

namespace Skills
{
    public class SkillsService : ApplicationService
    {
        private readonly SkillsModel _model;

        public event Action<SkillTreeNode> OnDataLoaded;
        public event Action<SkillTreeNode> OnSkillStateChanged;

        public SkillsService()
        {
            _model = new SkillsModel();
        }

        public bool IsAnySkillLearned() => _model.IsAnySkillLearned;

        public void ReloadData() => _model.LoadData();

        public void ForgetAllSkills() => _model.ForgetAllSkills();

        public void HandleSkillChanged(SkillTreeNode changedSkill) => OnSkillStateChanged?.Invoke(changedSkill);
        public void HandleDataLoaded(SkillTreeNode baseSkillNode) => OnDataLoaded?.Invoke(baseSkillNode);
    }
}