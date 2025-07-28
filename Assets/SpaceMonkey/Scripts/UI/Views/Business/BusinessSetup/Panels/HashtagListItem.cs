using System;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class HashtagListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void OnValidate()
        {
            text = GetComponent<TextMeshProUGUI>();
        }
    }
}