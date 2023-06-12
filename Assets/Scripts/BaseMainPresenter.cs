using UnityEngine;

namespace Skills.Core
{
    public abstract class BaseMainPresenter : MonoBehaviour
    {
        private void Awake()
        {
            ReferenceProvider.Register(this);
            InitInternal();
        }

        /// <summary>
        /// Calls in Awake
        /// </summary>
        protected virtual void InitInternal() { }
    }
}