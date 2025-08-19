using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class EnhancedButton : Button
    {
        [System.Serializable]
        public class TextColorBlock
        {
            public Color defaultColor = Color.black;
            public Color highlightedColor = Color.dimGray;
            public Color pressedColor = Color.darkSlateGray;
            public Color selectedColor = Color.darkGray;
            public Color disabledColor = Color.gray2;
        }

        [Header("Text Color Transition")] public bool enableTextColorTransition = true;
        public TextColorBlock textColors = new TextColorBlock();
        [Header("Text References")] public TextMeshProUGUI targetTMP;

        protected override void Start()
        {
            base.Start();
            if (targetTMP == null)
            {
                targetTMP = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (targetTMP == null)
            {
                targetTMP = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
#endif

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (!enableTextColorTransition)
                return;

            Color targetColor = state switch
            {
                SelectionState.Normal => textColors.defaultColor,
                SelectionState.Highlighted => textColors.highlightedColor,
                SelectionState.Pressed => textColors.pressedColor,
                SelectionState.Selected => textColors.selectedColor,
                SelectionState.Disabled => textColors.disabledColor,
                _ => textColors.defaultColor
            };
            SetTextColor(targetColor);
        }

        private void SetTextColor(Color color)
        {
            if (targetTMP != null)
                targetTMP.color = color;
        }

        public void ResetTextColor()
        {
            SetTextColor(textColors.defaultColor);
        }
    }
}