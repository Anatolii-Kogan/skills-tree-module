using System;
using System.Collections;
using System.Collections.Generic;
using Skills.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Skills.UI
{
    public class SkillButtonController : MonoBehaviour
    {
        [SerializeField] private SkillButtonController[] _subSkillsButtons;
        [SerializeField] private Button _button;
        [SerializeField] private Image _targetGraphic;
        [SerializeField] private TextMeshProUGUI _name;

        private SkillTreeNode _skillNode;

        private void Awake()
        {
            _button.onClick.AddListener(HandleClick);
            
            UpdateState();
        }

        public void InitSkill(SkillTreeNode skillNode)
        {
            _skillNode = skillNode;
            UpdateState();

            for (int i = 0; i < _subSkillsButtons.Length; i++)
            {
                if (_skillNode.TryGetSubNoteByIndex(i, out SkillTreeNode subNote))
                {
                    _subSkillsButtons[i].InitSkill(subNote);
                }
            }
        }

        /// <summary>
        /// Try find changed skill and update buttons state, if it changed skill found
        /// </summary>
        /// <param name="changedNode">skill that had changed</param>
        /// <returns>true - changed skill was found; false - changed skill wasn't found</returns>
        public bool UpdateData(SkillTreeNode changedNode)
        {
            if (changedNode == _skillNode)
            {
                MarkAsDirty();
                return true;
            }
            
            foreach (var skillButton in _subSkillsButtons)
            {
                if (skillButton.UpdateData(changedNode))
                {
                    return true;
                }
            }

            return false;
        }

        private void MarkAsDirty()
        {
            UpdateState();
            {
                foreach (var skillButton in _subSkillsButtons)
                {
                    skillButton.UpdateState();
                }
            }
        }

        private void HandleClick()
        {
            ReferenceProvider.GetReference<SkillInfoPresenter>().SetInfo(_skillNode);
            if (_skillNode.State != SkillState.CantBeLearned)
            {
                ReferenceProvider.GetReference<SkillsWindowPresenter>().SetSkill(_skillNode);
            }
        }

        private void UpdateState()
        {
            if (_skillNode == null)
            {
                _button.interactable = false;
                _name.text = string.Empty;
                return;
            }

            _name.text = _skillNode.SkillID;
            _button.interactable = !_skillNode.IsBase;
            _targetGraphic.color = _skillNode.State switch
            {
                SkillState.Learned => Color.green,
                SkillState.Unlearned => Color.white,
                SkillState.CantBeLearned => Color.grey,
                _ => Color.white
            };
        }
    }
}