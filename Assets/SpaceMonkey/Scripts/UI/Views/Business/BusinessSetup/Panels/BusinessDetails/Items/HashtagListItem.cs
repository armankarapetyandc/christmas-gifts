using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails.Items
{
    public class HashtagListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void OnValidate()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        public void Set(string tag)
        {
            text.text = tag;
        }
    }
}