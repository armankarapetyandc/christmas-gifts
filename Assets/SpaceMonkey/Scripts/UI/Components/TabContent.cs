using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class TabContent : MonoBehaviour
    {
        [SerializeField] private RectTransform container;

        public void SetState(bool state)
        {
            container.gameObject.SetActive(state);
        }
    }
}