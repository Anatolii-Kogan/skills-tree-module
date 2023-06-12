using System;
using System.Collections.Generic;
using Skills.Core;
using UnityEngine;
using Skills.Data;

namespace Skills
{
    public class SkillsModel
    {
        private const string FilePath = "Skills";

        private SkillTreeNode _baseNote;

        public bool IsAnySkillLearned
        {
            get
            {
                if (_baseNote != null)
                {
                    int index = 0;
                    while (_baseNote.TryGetSubNoteByIndex(index, out SkillTreeNode firstGenerationSkill))
                    {
                        if (firstGenerationSkill.State == SkillState.Learned)
                        {
                            return true;
                        }

                        index++;
                    }
                }

                return false;
            }
        }

        public void ForgetAllSkills()
        {
            int index = 0;
            while (_baseNote.TryGetSubNoteByIndex(index, out SkillTreeNode firstGenerationSkill))
            {
                firstGenerationSkill.TryForgetSkill();
                index++;
            }
        }
        
        /// <returns>base skill node</returns>
        /// <exception cref="NullReferenceException">base skill node not founded</exception>
        public SkillTreeNode LoadData()
        {
            SkillData[] skills = Resources.LoadAll<SkillData>(FilePath);

            SkillData baseSkill = null;
            foreach (var skill in skills)
            {
                if (skill.IsBase)
                {
                    baseSkill = skill;
                    break;
                }
            }

            if (baseSkill == null)
            {
                throw new NullReferenceException("There is no base skill data!");
            }

            _baseNote = SkillTreeNode.InitSkillInfo(baseSkill);

            return _baseNote;
        }
    }

    public class SkillTreeNode
    {
        private readonly SkillData _data;
        private SkillTreeNode[] _parentNodes = Array.Empty<SkillTreeNode>();
        private SkillTreeNode[] _subNodes = Array.Empty<SkillTreeNode>();

        public SkillState State { get; private set; } = SkillState.CantBeLearned;
        public string SkillID => _data.SkillID;
        public int Price => _data.Price;
        public bool IsBase => _data.IsBase;

        private ServiceReference<PointsService> _pointService;
        private ServiceReference<SkillsService> _skillService;

        private SkillTreeNode(SkillData data) => _data = data;

        /// <returns>base note</returns>
        public static SkillTreeNode InitSkillInfo(SkillData baseSkill)
        {
            Dictionary<SkillData, SkillTreeNode> skillNodes = new Dictionary<SkillData, SkillTreeNode>();
            
            SkillTreeNode baseNode = new SkillTreeNode(baseSkill);
            skillNodes.Add(baseSkill, baseNode);

            baseNode.InitSkillInfo(skillNodes);
            baseNode.State = SkillState.Learned;
            foreach (var subNode in baseNode._subNodes)
            {
                subNode.State = SkillState.Unlearned;
            }

            skillNodes.Clear();
            return baseNode;
        }

        public bool TryLearnSkill()
        {
            if (_pointService.Reference.IsEnoughPoint(Price))
            {
                ValidateState(SkillState.Unlearned);
                SetState(SkillState.Learned);
                
                return true;
            }

            return false;
        }

        public bool TryForgetSkill()
        {
            if (State == SkillState.Learned)
            {
                SetState(SkillState.Unlearned);
                return true;
            }

            return false;
        }

        public void ForgetSkill()
        {
            ValidateState(SkillState.Learned);
            SetState(SkillState.Unlearned);
        }

        public bool TryGetSubNoteByIndex(int index, out SkillTreeNode subNote)
        {
            if (index < _subNodes.Length )
            {
                subNote = _subNodes[index];
                return true;
            }

            subNote = null;
            return false;
        }

        private void OnParentStateChanged()
        {
            bool hasLearnedParent = false;
            foreach (var node in _parentNodes)
            {
                if (node.State == SkillState.Learned)
                {
                    hasLearnedParent = true;
                    break;
                }
            }

            SkillState newState = State;
            if (State != SkillState.CantBeLearned && !hasLearnedParent)
            {
                newState = SkillState.CantBeLearned;
            }

            if (State == SkillState.CantBeLearned && hasLearnedParent)
            {
                newState = SkillState.Unlearned;
            }

            SetState(newState);
        }
        
        private void SetState(SkillState newState)
        {
            if (newState != State)
            {
                if (newState == SkillState.Learned)
                {
                    _pointService.Reference.SpendPoints(_data.Price);
                }

                if (State == SkillState.Learned)
                {
                    _pointService.Reference.ReturnPoints(_data.Price);
                }
                
                State = newState;
                foreach (var subNode in _subNodes)
                {
                    subNode.OnParentStateChanged();
                }

               _skillService.Reference.HandleSkillChanged(this);
            }
        }

        private void ValidateState(SkillState state)
        {
            if (state != State)
            {
                throw new InvalidOperationException($"Skill state was {State}, but real state have to be {state}");
            }
        }

        private void InitSkillInfo(Dictionary<SkillData, SkillTreeNode> skillNodes)
        {
            foreach (SkillData subSkill in _data)
            {
                if (!skillNodes.TryGetValue(subSkill, out SkillTreeNode subNote))
                {
                    subNote = new SkillTreeNode(subSkill);
                    skillNodes.Add(subSkill, subNote);
                    subNote.InitSkillInfo(skillNodes);
                }
                this.AddSub(subNote);
                subNote.AddParent(this);
            }
        }

        private void AddSub(SkillTreeNode subNote)
        {
            Array.Resize(ref _subNodes, _subNodes.Length + 1);
            _subNodes[^1] = subNote;
        }

        private void AddParent(SkillTreeNode parentNode)
        {
            Array.Resize(ref _parentNodes, _parentNodes.Length + 1);
            _parentNodes[^1] = parentNode;
        }
    }
}