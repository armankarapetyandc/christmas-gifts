using System;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.Utilities.Validation
{
    public static class Validator
    {
        public static Observable<bool> Validate(params Observable<bool>[] validations)
        {
            return Observable
                .CombineLatest(validations)
                .Select(results => results.All(v => v))
                .DistinctUntilChanged();
        }

        public static IDisposable BindButton(this Observable<bool> validation, Button button)
        {
            return validation.Subscribe(state =>
            {
                Debug.LogError($"BindButton: {state}");
                button.interactable = state;
            });
        }
    }
}