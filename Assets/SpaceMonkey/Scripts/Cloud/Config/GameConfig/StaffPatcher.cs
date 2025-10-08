using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Views.Staff;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class StaffPatcher : ICloudDataPatcher<Staff[]>
    {
        public async UniTask<Staff[]> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(Staff), SheetDimension.ROWS);
            if (result.values == null || result.values.Count <= 1)
            {
                Debug.LogError("Sheet is empty or only contains headers");
                return null;
            }

            var patchedStaffs = new List<Staff>();

            // Skip header row and parse each row
            for (int i = 1; i < result.values.Count; i++)
            {
                var row = result.values[i];

                if (row == null || row.Count < 8)
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

                // Parse Profession (column 1)
                string professionStr = row[1]?.Trim();
                if (!Enum.TryParse<EmployeeProfession>(professionStr, true, out var profession))
                {
                    Debug.LogWarning($"Invalid Profession at row {i}: {professionStr}");
                    continue;
                }

                // Parse Payroll (column 2)
                if (!int.TryParse(row[2]?.Trim(), out int payroll))
                {
                    Debug.LogWarning($"Invalid Payroll at row {i}: {row[2]}");
                    continue;
                }

                // Parse Capacity (column 3)
                if (!int.TryParse(row[3]?.Trim(), out int capacity))
                {
                    Debug.LogWarning($"Invalid Capacity at row {i}: {row[3]}");
                    continue;
                }

                // Parse Speed (column 4)
                if (!int.TryParse(row[4]?.Trim(), out int speed))
                {
                    Debug.LogWarning($"Invalid Speed at row {i}: {row[4]}");
                    continue;
                }

                // Parse Experience (column 5)
                if (!int.TryParse(row[5]?.Trim(), out int experience))
                {
                    Debug.LogWarning($"Invalid Experience at row {i}: {row[5]}");
                    continue;
                }

                // Parse Quirk (column 6)
                string quirk = row[6]?.Trim() ?? string.Empty;

                // Parse Work Ethic (column 7)
                string workEthic = row[7]?.Trim() ?? string.Empty;

                // Create staff with null CharacterConfig
                var staff = Staff.Create(id, profession, payroll, capacity, speed, experience, quirk, workEthic, null);
                patchedStaffs.Add(staff);
            }

            Debug.Log($"Parsed {patchedStaffs.Count} staffs");
            return patchedStaffs.ToArray();
        }
    }
}