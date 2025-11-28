using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.Tutorial.FunnySlide
{
    public class FunnySlideOut : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI description;

        public void Show(string desc)
        {
            description.text = desc;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}