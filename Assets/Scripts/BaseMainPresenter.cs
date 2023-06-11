using Skills.UI;
using UnityEngine;

namespace Skills.Core
{
    public class BaseMainPresenter : MonoBehaviour, IMainPresenter
    {
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            ReferenceProvider.Register(GetComponent<IMainPresenter>());
        }
    }
}