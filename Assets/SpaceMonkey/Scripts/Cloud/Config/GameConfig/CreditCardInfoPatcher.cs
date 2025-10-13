using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Config.GameConfig
{
    public class CreditCardInfoPatcher:ICloudDataPatcher<CreditCardInfo>
    {
        public async UniTask<CreditCardInfo> Patch(CloudDataRestClient client, string spreadSheetId)
        {
            var result = await client.ReadSheetContent(spreadSheetId, nameof(CreditCardInfo), SheetDimension.ROWS);
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

            var row = result.values[1];

            if (row == null || row.Count < 4)
            {
                Debug.LogWarning("Incomplete data in CreditCard sheet");
                return null;
            }

            // Parse Credit Limit (column 0)
            if (!float.TryParse(row[0]?.Trim(), out float creditLimit))
            {
                Debug.LogWarning($"Invalid Credit Limit: {row[0]}");
                return null;
            }

            // Parse APR (column 1)
            if (!float.TryParse(row[1]?.Trim(), out float apr))
            {
                Debug.LogWarning($"Invalid APR: {row[1]}");
                return null;
            }

            // Parse Minimum Payment (column 2)
            if (!float.TryParse(row[2]?.Trim(), out float minimumPayment))
            {
                Debug.LogWarning($"Invalid Minimum Payment: {row[2]}");
                return null;
            }

            // Parse Minimum Payment Coff (column 3)
            if (!float.TryParse(row[3]?.Trim(), out float minimumPaymentCoff))
            {
                Debug.LogWarning($"Invalid Minimum Payment Coff: {row[3]}");
                return null;
            }

            Debug.Log("Parsed CreditCardInfo successfully");
            return CreditCardInfo.Create(creditLimit, apr, minimumPayment, minimumPaymentCoff);
        }
    }
}