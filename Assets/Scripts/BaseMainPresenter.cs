using UnityEngine;

namespace Skills.Core
{
    public class BaseMainPresenter : MonoBehaviour
    {
        private void Awake()
        {
            ReferenceProvider.Register(this);
            InitInternal();
        }

        protected virtual void InitInternal() { }
    }
}