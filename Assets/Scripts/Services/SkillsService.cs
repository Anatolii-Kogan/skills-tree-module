using System;
using System.Collections;
using System.Collections.Generic;
using Skills.Core;
using UnityEngine;

namespace Skills
{
    public class SkillsService : ApplicationService
    {
        private SkillsModel _model;

        public event Action<SkillTreeNode> OnSkillStateChanged;

        public SkillsService()
        {
            _model = new SkillsModel();
        }

        public bool IsAnySkillLearned() => _model.IsAnySkillLearned;

        public void ForgetAllSkills() => _model.ForgetAllSkills();

        public void HandleSkillChanged(SkillTreeNode changedSkill) => OnSkillStateChanged?.Invoke(changedSkill);
    }
}