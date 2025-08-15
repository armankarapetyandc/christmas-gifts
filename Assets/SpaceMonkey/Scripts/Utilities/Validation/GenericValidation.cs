using System;
using R3;

namespace SpaceMonkey.Scripts.Utilities.Validation
{
    public static class GenericValidation
    {
        /// <summary>
        /// Creates a validation observable from any source stream.
        /// </summary>
        public static Observable<bool> RuleFrom<T>(
            this T component,
            Func<T, Observable<bool>> ruleFactory)
        {
            return ruleFactory(component);
        }

        /// <summary>
        /// Wrap any observable value and check condition.
        /// </summary>
        public static Observable<bool> ToValidation<T>(
            this Observable<T> source,
            Func<T, bool> condition)
        {
            return source.Select(condition);
        }
    }
}