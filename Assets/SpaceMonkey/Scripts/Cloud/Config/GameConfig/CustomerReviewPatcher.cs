using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class CustomerReviewPatcher : ICloudDataPatcher<ReviewInfo[]>
    {
        public async UniTask<ReviewInfo[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(ReviewInfo), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }

            // Skip header row, get first data row (row index 1)
            if (result.values.Count < 2)
            {
                Debug.LogWarning("No data row found in CreditCard sheet");
                return null;
            }

            var reviews = new List<ReviewInfo>();

            // Skip header row and parse each row
            for (int i = 1; i < result.values.Count; i++)
            {
                var row = result.values[i];

                if (row == null || row.Count < 3)
                {
                    Debug.LogWarning($"Skipping incomplete row {i}");
                    continue;
                }

                // Parse type (column 0)
                string typeStr = row[0]?.Trim();
                if (!Enum.TryParse<ReviewType>(typeStr, true, out var reviewType))
                {
                    Debug.LogWarning($"Invalid ReviewType at row {i}: {typeStr}");
                    continue;
                }

                // Parse message (column 1)
                string message = row[1]?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(message))
                {
                    Debug.LogWarning($"Empty message at row {i}");
                    continue;
                }

                // Parse needFormating (column 2)
                string needFormatStr = row[2]?.Trim().ToLower();
                bool needFormating = needFormatStr == "true" || needFormatStr == "1" || needFormatStr == "yes";

                reviews.Add(new ReviewInfo(reviewType, message, needFormating));
            }

            Debug.Log($"Parsed {reviews.Count} customer reviews");
            return reviews.ToArray();
        }
    }
}