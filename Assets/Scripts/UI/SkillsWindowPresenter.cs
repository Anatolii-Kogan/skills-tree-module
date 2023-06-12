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
        
        private ServiceReference<SkillsService> _skillsService;
        private ServiceReference<PointsService> _pointsService;

        protected override void InitInternal()
        {
            base.InitInternal();
            _skillsService.Reference.OnDataReload += _baseSkillButton.InitSkill;
            _skillsService.Reference.OnSkillStateChanged += UpdateData;

            _pointsService.Reference.OnAmountChanged += HandlePointsAmountChanged;

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
            _forgetAllSkillsButton.interactable = _skillsService.Reference.IsAnySkillLearned();
            
            _learnSkillButton.interactable = _currentNode != null &&  _currentNode.State == SkillState.Unlearned && _pointsService.Reference.IsEnoughPoint(_currentNode.Price);
            _forgetSkillButton.interactable = _currentNode != null && _currentNode.State == SkillState.Learned;
        }

        private void HandlePointsAmountChanged(int points, int totalPoints) => UpdateUI();

        private void LearnSkill() => _currentNode.LearnSkill();
        private void ForgetSkill() => _currentNode.ForgetSkill();
        private void ForgetAllSkills() => _skillsService.Reference.ForgetAllSkills();
    }
}