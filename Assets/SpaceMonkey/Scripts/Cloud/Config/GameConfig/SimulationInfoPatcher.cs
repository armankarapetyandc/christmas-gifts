using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Utilities;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class SimulationInfoPatcher : ICloudDataPatcher<SimulationInfo>
    {
        public async UniTask<SimulationInfo> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(SimulationInfo), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogWarning($"Sheet '{nameof(SimulationInfo)}' is empty or only contains headers");
                return null;
            }

            return ParseSimulationInfo(result.values);
        }

        private SimulationInfo ParseSimulationInfo(List<List<string>> sheetValues)
        {
            // Create a dictionary for easy property lookup
            var propertyDict = new Dictionary<string, List<string>>();

            // Skip header row and build dictionary
            for (int i = 1; i < sheetValues.Count; i++)
            {
                var row = sheetValues[i];
                if (row == null || row.Count < 2)
                    continue;

                string propertyName = row[0]?.Trim();
                if (string.IsNullOrWhiteSpace(propertyName))
                    continue;

                // Store all values for this property (from column 1 onwards)
                var values = new List<string>();
                for (int j = 1; j < row.Count; j++)
                {
                    if (!string.IsNullOrWhiteSpace(row[j]))
                    {
                        values.Add(row[j].Trim());
                    }
                }

                propertyDict[propertyName] = values;
            }

            // Parse individual properties
            int customersMin = GetInt(propertyDict, "CustomersMin", 1);
            int customersMax = GetInt(propertyDict, "CustomersMax", 3);
            int moodMin = GetInt(propertyDict, "MoodMin", 55);
            int moodMax = GetInt(propertyDict, "MoodMax", 75);
            int moodLeave = GetInt(propertyDict, "MoodLeave", 30);
            float moodTtpCoefficient = GetFloat(propertyDict, "MoodTtpCoefficient", 2.5f);
            float moodMaterialCoefficient = GetFloat(propertyDict, "MoodMaterialCoefficient", 10f);
            float moodPackagingCoefficient = GetFloat(propertyDict, "MoodPackagingCoefficient", 10f);
            float moodOrderFulfillmentCoefficient = GetFloat(propertyDict, "MoodOrderFulfillmentCoefficient", 10f);
            float moodOrderNotFulfillmentCoefficient =
                GetFloat(propertyDict, "MoodOrderNotFulfillmentCoefficient", -15f);
            int customerMoodThreshold = GetInt(propertyDict, "CustomerMoodThreshold", 30);
            int orderQuantityMin = GetInt(propertyDict, "OrderQuantityMin", 1);
            int orderQuantityMax = GetInt(propertyDict, "OrderQuantityMax", 3);
            int newCustomersMin = GetInt(propertyDict, "NewCustomersMin", 1);
            int newCustomersMax = GetInt(propertyDict, "NewCustomersMax", 2);
            float priceSensitivity = GetFloat(propertyDict, "PriceSensitivity", 0.3f);
            float reviewChance = GetFloat(propertyDict, "ReviewChance", 75f);
            float bigProductChange = GetFloat(propertyDict, "BigProductChange", 40f);
            float extremeSettingHigh = GetFloat(propertyDict, "ExtremeSettingHigh", 80f);
            float extremeSettingLow = GetFloat(propertyDict, "ExtremeSettingLow", 20f);

            // Parse MoodRanges (pairs of Min/Max values)
            RangeValue[] moodRanges = ParseMoodRanges(propertyDict);

            Debug.Log("Parsed SimulationInfo successfully");
            return SimulationInfo.Create(
                customersMin, customersMax, moodMin, moodMax, moodLeave,
                moodTtpCoefficient, moodMaterialCoefficient, moodPackagingCoefficient,
                moodOrderFulfillmentCoefficient, moodOrderNotFulfillmentCoefficient,
                customerMoodThreshold, moodRanges, orderQuantityMin, orderQuantityMax,
                newCustomersMin, newCustomersMax, priceSensitivity, reviewChance,
                bigProductChange, extremeSettingHigh, extremeSettingLow
            );
        }

        private int GetInt(Dictionary<string, List<string>> dict, string key, int defaultValue)
        {
            if (dict.TryGetValue(key, out var values) && values.Count > 0)
            {
                if (int.TryParse(values[0], out int result))
                    return result;
            }

            Debug.LogWarning($"Using default value for {key}: {defaultValue}");
            return defaultValue;
        }

        private float GetFloat(Dictionary<string, List<string>> dict, string key, float defaultValue)
        {
            if (dict.TryGetValue(key, out var values) && values.Count > 0)
            {
                if (float.TryParse(values[0], out float result))
                    return result;
            }

            Debug.LogWarning($"Using default value for {key}: {defaultValue}");
            return defaultValue;
        }

        private RangeValue[] ParseMoodRanges(Dictionary<string, List<string>> dict)
        {
            if (!dict.TryGetValue("MoodRanges", out var values) || values.Count < 2)
            {
                Debug.LogWarning("No MoodRanges found, using empty array");
                return Array.Empty<RangeValue>();
            }

            var ranges = new List<RangeValue>();

            // Parse pairs of values (Min, Max)
            for (int i = 0; i < values.Count - 1; i += 2)
            {
                if (float.TryParse(values[i], out float min) &&
                    float.TryParse(values[i + 1], out float max))
                {
                    ranges.Add(RangeValue.Create(min, max));
                }
            }

            Debug.Log($"Parsed {ranges.Count} mood ranges");
            return ranges.ToArray();
        }
    }
}