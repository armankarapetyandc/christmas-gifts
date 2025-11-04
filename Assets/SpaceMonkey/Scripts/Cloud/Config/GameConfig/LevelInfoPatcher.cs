using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class LevelInfoPatcher : ICloudDataPatcher<LevelInfo[]>
    {
        public async UniTask<LevelInfo[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(LevelInfo), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }

            var patchedLevelInfos = new List<LevelInfo>();
            Dictionary<int, List<LevelUnlockInfo>> levelUnlockMap = new Dictionary<int, List<LevelUnlockInfo>>();

            // Skip header row and parse each row
            for (int i = 1; i < result.values.Count; i++)
            {
                var row = result.values[i];

                if (row == null || row.Count < 5)
                {
                    Debug.LogWarning($"Skipping incomplete row {i}");
                    continue;
                }

                // Parse Level (column 0)
                if (!int.TryParse(row[0]?.Trim(), out int level))
                {
                    Debug.LogWarning($"Invalid Level at row {i}: {row[0]}");
                    continue;
                }

                // Parse Score (column 1)
                if (!int.TryParse(row[1]?.Trim(), out int score))
                {
                    Debug.LogWarning($"Invalid Score at row {i}: {row[1]}");
                    continue;
                }

                // Parse UnlockInfo Key (column 2)
                string unlockKey = row[2]?.Trim();
                
                // Parse IconVisualAssetId (column 3)
                string iconAssetId = row[3]?.Trim() ?? string.Empty;
                
                // Parse Description (column 4)
                string description = row[4]?.Trim() ?? string.Empty;

                // Initialize list for this level if not exists
                if (!levelUnlockMap.ContainsKey(level))
                {
                    levelUnlockMap[level] = new List<LevelUnlockInfo>();
                    
                    // Create LevelInfo with Score (will add unlocks later)
                    var levelInfo = CreateLevelInfo(level, score, new List<LevelUnlockInfo>());
                    patchedLevelInfos.Add(levelInfo);
                }

                // If there's unlock info, add it to the level
                if (!string.IsNullOrWhiteSpace(unlockKey))
                {
                    var unlockInfo = CreateLevelUnlockInfo(unlockKey, iconAssetId, description);
                    levelUnlockMap[level].Add(unlockInfo);
                }
            }

            // Assign unlock infos to their respective levels
            foreach (var levelInfo in patchedLevelInfos)
            {
                if (levelUnlockMap.ContainsKey(levelInfo.Level))
                {
                    SetUnlockInfo(levelInfo, levelUnlockMap[levelInfo.Level]);
                }
            }

            Debug.Log($"Parsed {patchedLevelInfos.Count} level infos");
            return patchedLevelInfos.ToArray();
        }

        private LevelInfo CreateLevelInfo(int level, int score, List<LevelUnlockInfo> unlockInfo)
        {
            var levelInfo = new LevelInfo();
            var levelType = levelInfo.GetType();
            
            // Use reflection to set private properties
            levelType.GetProperty(nameof(LevelInfo.Level))?.SetValue(levelInfo, level);
            levelType.GetProperty(nameof(LevelInfo.Score))?.SetValue(levelInfo, score);
            levelType.GetProperty(nameof(LevelInfo.UnlockInfo))?.SetValue(levelInfo, unlockInfo);
            
            return levelInfo;
        }

        private LevelUnlockInfo CreateLevelUnlockInfo(string key, string iconVisualAssetId, string description)
        {
            var unlockInfo = new LevelUnlockInfo();
            var unlockType = unlockInfo.GetType();
            
            // Use reflection to set private properties
            unlockType.GetProperty(nameof(LevelUnlockInfo.Key))?.SetValue(unlockInfo, key);
            unlockType.GetProperty(nameof(LevelUnlockInfo.IconVisualAssetId))?.SetValue(unlockInfo, iconVisualAssetId);
            unlockType.GetProperty(nameof(LevelUnlockInfo.Description))?.SetValue(unlockInfo, description);
            
            return unlockInfo;
        }

        private void SetUnlockInfo(LevelInfo levelInfo, List<LevelUnlockInfo> unlockInfos)
        {
            var levelType = levelInfo.GetType();
            levelType.GetProperty(nameof(LevelInfo.UnlockInfo))?.SetValue(levelInfo, unlockInfos);
        }
    }
}