using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class CategoryInfoPatcher : ICloudDataPatcher<CategoryInfo[]>
    {
        public async UniTask<CategoryInfo[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(CategoryInfo), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }

            // Skip header row and group rows by Name (first column)
            var groupedRows = result.values
                .Skip(1) // Skip header
                .Where(row => row != null && row.Count >= 5 && !string.IsNullOrWhiteSpace(row[0]))
                .GroupBy(row => row[0].Trim()); // Group by Name        

            var categories = new List<CategoryInfo>();
            foreach (var group in groupedRows)
            {
                string categoryName = group.Key;
                var rows = group.ToList();

                // Parse tags from all rows in this group
                var tags = new List<HashtagInfo>();
                bool isEnabled = true; // Default value

                foreach (var row in rows)
                {
                    // Parse tag information
                    string tagName = row.Count > 1 ? row[1].Trim() : string.Empty;

                    if (!string.IsNullOrWhiteSpace(tagName))
                    {
                        float materialAdd = 0f;
                        float packagingAdd = 0f;

                        // Parse Material Add (column 2)
                        if (row.Count > 2 && !string.IsNullOrWhiteSpace(row[2]))
                        {
                            float.TryParse(row[2].Trim(), out materialAdd);
                        }

                        // Parse Packaging Add (column 3)
                        if (row.Count > 3 && !string.IsNullOrWhiteSpace(row[3]))
                        {
                            float.TryParse(row[3].Trim(), out packagingAdd);
                        }

                        tags.Add(HashtagInfo.Create(tagName, materialAdd, packagingAdd));
                    }

                    // Parse Enabled (column 4) - use from last row or first valid value
                    if (row.Count > 4 && !string.IsNullOrWhiteSpace(row[4]))
                    {
                        string enabledStr = row[4].Trim().ToLower();
                        isEnabled = enabledStr == "true" || enabledStr == "1" || enabledStr == "yes";
                    }
                }

                // Create category with all tags
                var category = CategoryInfo.Create(categoryName, tags.ToArray(), isEnabled);
                categories.Add(category);
            }

            return categories.ToArray();
        }
    }
}