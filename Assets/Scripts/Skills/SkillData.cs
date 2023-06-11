using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills.Data
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Data/Skills/SkillData", order = 1)]
    public class SkillData : ScriptableObject, IEnumerable<SkillData>
    {
        [SerializeField] private bool _isBase;
        [SerializeField] private string _skillID;
        [SerializeField] private int _price;
        
        [SerializeField] private SkillData[] _subSkills;

        public bool IsBase => _isBase;
        public string SkillID => _skillID;
        public int Price => _price;
        
        public IEnumerator<SkillData> GetEnumerator()
        {
            foreach (SkillData subSkill in _subSkills)
            {
                yield return subSkill;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}