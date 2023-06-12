using Skills.UI;
using UnityEngine;

namespace Skills.Core
{
    public class BaseMainPresenter : MonoBehaviour
    {
        private void Awake()
        {
            ReferenceProvider.Register(this);
            //Init();
        }

        // public void Init()
        // {
        //     ReferenceProvider.Register(GetComponent<IMainPresenter>());
        // }
    }
}