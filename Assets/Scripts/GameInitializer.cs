using Skills.Core;
using UnityEngine;

namespace Skills
{
    public class GameInitializer : MonoBehaviour
    {
        private ServiceReference<SkillsService> _skillService;
        
        /// <summary>
        /// Let different presenter add listeners in Awake
        /// </summary>
        private void Start()
        {
            _skillService.Reference.ReloadData();
        }
    }
}