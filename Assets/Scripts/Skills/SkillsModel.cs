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
                if (firstGenerationSkill.State == SkillState.Learned)
                {
                    firstGenerationSkill.ForgetSkill();
                }

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

            _baseNote = SkillTreeNode.InitSkillInfo(baseSkill, capacity: skills.Length);

            return _baseNote;
        }
    }

    public class SkillTreeNode
    {
        private readonly SkillData _data;
        private SkillTreeNode[] _parentNodes = Array.Empty<SkillTreeNode>();
        private SkillTreeNode[] _subNodes = Array.Empty<SkillTreeNode>();
        
        private ServiceReference<PointsService> _pointService;
        private ServiceReference<SkillsService> _skillService; 

        public SkillState State { get; private set; } = SkillState.CantBeLearned;
        public string SkillID => _data.SkillID;
        public int Price => _data.Price;
        public bool IsBase => _data.IsBase;

        public bool CanBeForget
        {
            get
            {
                if (State != SkillState.Learned)
                {
                    return false;
                }

                foreach (var subNode in _subNodes)
                {
                    if (subNode.State == SkillState.Learned && !subNode.IsAnyParentNodeLearned(this))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private SkillTreeNode(SkillData data) => _data = data;

        /// <returns>base note</returns>
        public static SkillTreeNode InitSkillInfo(SkillData baseSkill, int capacity = 0)
        {
            Dictionary<SkillData, SkillTreeNode> skillNodes = new Dictionary<SkillData, SkillTreeNode>(capacity);

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

        public void LearnSkill()
        {
            ValidateState(SkillState.Unlearned);
            SetState(SkillState.Learned);
        }

        public void ForgetSkill()
        {
            ValidateState(SkillState.Learned);
            SetState(SkillState.Unlearned);
        }

        public bool TryGetSubNoteByIndex(int index, out SkillTreeNode subNote)
        {
            if (index < _subNodes.Length)
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

        private bool IsAnyParentNodeLearned(SkillTreeNode exception = null)
        {
            foreach (var parentNode in _parentNodes)
            {
                if (parentNode.State == SkillState.Learned && parentNode != exception)
                {
                    return true;
                }
            }

            return false;
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
                
                AddNote(ref _subNodes, subNote);
                subNote.AddNote(ref subNote._parentNodes, this);
            }
        }

        private void AddNote(ref SkillTreeNode[] nodes, SkillTreeNode newNode)
        {
            Array.Resize(ref nodes, nodes.Length + 1);
            nodes[^1] = newNode;
        }
    }
}