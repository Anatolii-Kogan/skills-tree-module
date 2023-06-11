using System;
using Skills.Core;
using UnityEngine;
using TMPro;

namespace Skills.UI
{
    public class SkillInfoPresenter : BaseMainPresenter
    {
        [SerializeField] private TextMeshProUGUI _info;

        private void Start()
        {
            _info.text = string.Empty;
        }

        public void SetInfo(SkillTreeNode skill)
        {
            _info.text = $"Skill:\nid: {skill.SkillID}\nprice: {skill.Price}";
        }
    }
}