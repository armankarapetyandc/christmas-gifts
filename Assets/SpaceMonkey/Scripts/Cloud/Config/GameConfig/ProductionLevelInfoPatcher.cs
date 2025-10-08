using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class ProductionLevelInfoPatcher : ICloudDataPatcher<ProductionLevelInfo[]>
    {
        public async UniTask<ProductionLevelInfo[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(ProductionLevelInfo), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }

            var levels = new List<ProductionLevelInfo>();

            // Skip header row and parse each row
            for (int i = 1; i < result.values.Count; i++)
            {
                var row = result.values[i];

                if (row == null || row.Count < 3)
                {
                    Debug.LogWarning($"Skipping incomplete row {i}");
                    continue;
                }

                // Parse Id (column 0)
                string id = row[0]?.Trim();
                if (string.IsNullOrWhiteSpace(id))
                {
                    Debug.LogWarning($"Empty Id at row {i}");
                    continue;
                }

                // Parse Prod Cap Add (column 1)
                if (!int.TryParse(row[1]?.Trim(), out int prodCapAdd))
                {
                    Debug.LogWarning($"Invalid Prod Cap Add at row {i}: {row[1]}");
                    continue;
                }

                // Parse Prod Cap Cost (column 2)
                if (!int.TryParse(row[2]?.Trim(), out int prodCapCost))
                {
                    Debug.LogWarning($"Invalid Prod Cap Cost at row {i}: {row[2]}");
                    continue;
                }

                levels.Add(ProductionLevelInfo.Create(id, prodCapAdd, prodCapCost));
            }

            Debug.Log($"Parsed {levels.Count} production levels");
            return levels.ToArray();
        }
    }
}