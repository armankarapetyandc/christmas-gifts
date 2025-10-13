using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class BusinessExamplePatcher : ICloudDataPatcher<BusinessExample[]>
    {
        public async UniTask<BusinessExample[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(BusinessExample), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }
            
            var examples = new List<BusinessExample>();

            // Skip header row and parse each row
            for (int i = 1; i < result.values.Count; i++)
            {
                var row = result.values[i];

                if (row == null || row.Count < 5)
                {
                    Debug.LogWarning($"Skipping incomplete row {i}");
                    continue;
                }

                // Skip Id (column 0) - not used in BusinessExample

                // Parse Category (column 1)
                string category = row[1]?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(category))
                {
                    Debug.LogWarning($"Empty Category at row {i}");
                    continue;
                }

                // Parse Name (column 2)
                string name = row[2]?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                {
                    Debug.LogWarning($"Empty Name at row {i}");
                    continue;
                }

                // Parse Title (column 3)
                string title = row[3]?.Trim() ?? string.Empty;

                // Parse Description (column 4)
                string description = row[4]?.Trim() ?? string.Empty;

                examples.Add(BusinessExample.Create(category, name, title, description));
            }

            Debug.Log($"Parsed {examples.Count} business examples");
            return examples.ToArray();
        }
    }
}