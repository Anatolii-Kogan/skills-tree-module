using UnityEngine;

namespace Skills.Core
{
    public class BaseMainPresenter : MonoBehaviour
    {
        private void Awake()
        {
            ReferenceProvider.Register(this);
        }
    }
}