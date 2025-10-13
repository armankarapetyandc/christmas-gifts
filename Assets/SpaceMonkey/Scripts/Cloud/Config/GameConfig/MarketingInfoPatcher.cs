using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class MarketingInfoPatcher: ICloudDataPatcher<MarketingInfo[]>
    {
        public async UniTask<MarketingInfo[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(MarketingInfo), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }
            
            var marketingInfos = new List<MarketingInfo>();

            // Skip header row and parse each row
            for (int i = 1; i < result.values.Count; i++)
            {
                var row = result.values[i];

                if (row == null || row.Count < 5)
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

                // Parse Min Mult (column 1)
                if (!int.TryParse(row[1]?.Trim(), out int minMult))
                {
                    Debug.LogWarning($"Invalid Min Mult at row {i}: {row[1]}");
                    continue;
                }

                // Parse Max Mult (column 2)
                if (!int.TryParse(row[2]?.Trim(), out int maxMult))
                {
                    Debug.LogWarning($"Invalid Max Mult at row {i}: {row[2]}");
                    continue;
                }

                // Parse Div (column 3)
                if (!int.TryParse(row[3]?.Trim(), out int div))
                {
                    Debug.LogWarning($"Invalid Div at row {i}: {row[3]}");
                    continue;
                }

                // Parse Unlock (column 4)
                if (!int.TryParse(row[4]?.Trim(), out int unlock))
                {
                    Debug.LogWarning($"Invalid Unlock at row {i}: {row[4]}");
                    continue;
                }

                marketingInfos.Add(MarketingInfo.Create(id, minMult, maxMult, div, unlock));
            }

            Debug.Log($"Parsed {marketingInfos.Count} marketing infos");
            return marketingInfos.ToArray();
        }
    }
}