using System;
using Skills.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Skills.UI
{
    public class SkillsWindowPresenter : BaseMainPresenter
    {
        [SerializeField] private SkillButtonController _baseSkillButton;
        [SerializeField] private Button _learnSkillButton;
        [SerializeField] private Button _forgetAllSkillsButton;
        [SerializeField] private Button _forgetSkillButton;

        private SkillTreeNode _currentNode;
        
        private ServiceReference<SkillsService> _skillService;

        protected override void InitInternal()
        {
            base.InitInternal();
            _skillService.Reference.OnDataReload += SetNewData;
            _skillService.Reference.OnSkillStateChanged += UpdateData;

            _learnSkillButton.onClick.AddListener(LearnSkill);
            _forgetAllSkillsButton.onClick.AddListener(ForgetAllSkills);
            _forgetSkillButton.onClick.AddListener(ForgetSkill);
            
            UpdateUI();
        }

        public void SetSkill(SkillTreeNode skill)
        {
            _currentNode = skill;
            UpdateUI();
        }
        
        private void SetNewData(SkillTreeNode baseSkillNode)
        {
            _baseSkillButton.InitSkill(baseSkillNode);
        }

        private void UpdateData(SkillTreeNode changedSkillNode)
        {
            bool isChangedSkillFounded = _baseSkillButton.UpdateData(changedSkillNode);
            if (!isChangedSkillFounded)
            {
                throw new InvalidOperationException("Skill data is out of date!");
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            _forgetAllSkillsButton.interactable = _skillService.Reference.IsAnySkillLearned();
            if (_currentNode == null)
            {
                _learnSkillButton.interactable = false;
                _forgetSkillButton.interactable = false;
                return;
            }

            _learnSkillButton.interactable = _currentNode.State == SkillState.Unlearned;
            _forgetSkillButton.interactable = _currentNode.State == SkillState.Learned;
        }

        private void LearnSkill() => _currentNode.TryLearnSkill();
        private void ForgetSkill() => _currentNode.ForgetSkill();
        private void ForgetAllSkills() => _skillService.Reference.ForgetAllSkills();
    }
}