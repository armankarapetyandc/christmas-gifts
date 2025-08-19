using R3;
using TMPro;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.Utilities.Validation
{
    public static class UIValidationExtensions
    {
        public static Observable<bool> NotEmpty(this TMP_InputField field) =>
            field.onValueChanged.AsObservable()
                .Select(text => !string.IsNullOrWhiteSpace(text))
                .StartWithValue(!string.IsNullOrWhiteSpace(field.text));

        public static Observable<bool> MinLength(this TMP_InputField field, int length) =>
            field.onValueChanged.AsObservable()
                .Select(text => text != null && text.Length >= length)
                .StartWithValue(field.text.Length >= length);

        public static Observable<bool> RegexMatch(this TMP_InputField field, string pattern) =>
            field.onValueChanged.AsObservable()
                .Select(text => System.Text.RegularExpressions.Regex.IsMatch(text ?? "", pattern))
                .StartWithValue(System.Text.RegularExpressions.Regex.IsMatch(field.text ?? "", pattern));

        public static Observable<bool> IsOn(this Toggle toggle) =>
            toggle.OnValueChangedAsObservable()
                .StartWithValue(toggle.isOn);

        public static Observable<bool> IsOff(this Toggle toggle) =>
            toggle.OnValueChangedAsObservable().Select(on => !on).StartWithValue(!toggle.isOn);

        public static Observable<bool> GreaterThan(this Slider slider, float value) =>
            slider.OnValueChangedAsObservable().Select(v => v > value).StartWithValue(slider.value > value);

        public static Observable<bool> LessThan(this Slider slider, float value) =>
            slider.OnValueChangedAsObservable().Select(v => v < value).StartWithValue(slider.value < value);

        public static Observable<bool> InRange(this Slider slider, float min, float max) =>
            slider.OnValueChangedAsObservable().Select(v => v >= min && v <= max)
                .StartWithValue(slider.value >= min && slider.value <= max);
    }
}