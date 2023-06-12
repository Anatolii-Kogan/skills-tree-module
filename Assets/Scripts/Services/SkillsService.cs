using System;
using Skills.Core;

namespace Skills
{
    public class SkillsService : ApplicationService
    {
        private readonly SkillsModel _model;

        public event Action<SkillTreeNode> OnDataReload;
        public event Action<SkillTreeNode> OnSkillStateChanged;

        public SkillsService()
        {
            _model = new SkillsModel();
        }

        public bool IsAnySkillLearned() => _model.IsAnySkillLearned;

        public void ForgetAllSkills() => _model.ForgetAllSkills();

        public void ReloadData()
        {
            var baseSkillNode = _model.LoadData();
            OnDataReload?.Invoke(baseSkillNode);
        }

        public void HandleSkillChanged(SkillTreeNode changedSkill) => OnSkillStateChanged?.Invoke(changedSkill);
    }
}