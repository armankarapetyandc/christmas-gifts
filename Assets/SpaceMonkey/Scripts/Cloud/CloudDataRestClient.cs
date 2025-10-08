using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using UnityEngine;
using UnityEngine.Networking;

namespace SpaceMonkey.Scripts.Cloud
{
    public enum SheetDimension
    {
        DIMENSION_UNSPECIFIED,
        ROWS,
        COLUMNS
    }
    [Serializable]
    public class GoogleServiceAccountKey
    {
        public string type;
        public string project_id;
        public string private_key_id;
        public string private_key;
        public string client_email;
        public string client_id;
        public string auth_uri;
        public string token_uri;
        public string auth_provider_x509_cert_url;
        public string client_x509_cert_url;
    }

    [Serializable]
    public class DriveFilesResponse
    {
        public struct DriveFileInfo
        {
            public string id;
            public string name;
        }

        public List<DriveFileInfo> files;
    }

    [Serializable]
    public class GoogleSheetResponse
    {
        public string range;
        public string majorDimension;
        public List<List<string>> values;
    }

    public class CloudDataRestClient
    {
        private const string TOKEN_URL = "https://oauth2.googleapis.com/token";

        private static readonly string[] SCOPES =
        {
            "https://www.googleapis.com/auth/drive",
            "https://www.googleapis.com/auth/drive.file",
            "https://www.googleapis.com/auth/spreadsheets"
        };

        public bool Initialized { get; private set; }
        
        private GoogleServiceAccountKey _credentials;
        private string _accessToken;

      
        
        public async UniTask Initialize()
        {
            Initialized = false;
            await ReadCredentials();
            await RetrieveAccessToken();
            if (string.IsNullOrEmpty(_accessToken))
            {
                Debug.LogError("Failed to get access token");
                return;
            }
            Initialized = true;
            Debug.Log("Access token acquired");
            /*var files = await ListDriveFiles();
            var file = files.files.FirstOrDefault();
            await ReadSheetContent(file.id);*/
        }
        private async UniTask ReadCredentials()
        {
            // Load credentials
            var content =
                (TextAsset)await Resources.LoadAsync<TextAsset>("Credentials/space-monkey-unity-f62716954218");
            _credentials = JsonConvert.DeserializeObject<GoogleServiceAccountKey>(content.text);
        }
        private async UniTask RetrieveAccessToken()
        {
            try
            {
                // Create JWT header and claim set manually
                var header = new
                {
                    alg = "RS256",
                    typ = "JWT"
                };

                long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var claimSet = new
                {
                    iss = _credentials.client_email,
                    scope = string.Join(" ", SCOPES),
                    aud = TOKEN_URL,
                    exp = now + 3600,
                    iat = now
                };

                string headerBase64 = Base64UrlEncode(JsonConvert.SerializeObject(header));
                string claimBase64 = Base64UrlEncode(JsonConvert.SerializeObject(claimSet));

                string unsignedJwt = $"{headerBase64}.{claimBase64}";
                string signature = CreateJwtSignature(unsignedJwt, _credentials.private_key);
                string signedJwt = $"{unsignedJwt}.{signature}";

                var postData = new Dictionary<string, string>
                {
                    { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                    { "assertion", signedJwt }
                };

                using (var www = UnityWebRequest.Post(TOKEN_URL, postData))
                {
                    www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                    await www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Token request failed: " + www.error);
                        _accessToken = null;
                    }

                    var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(www.downloadHandler.text);
                    _accessToken = response["access_token"].ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error getting access token: " + ex);
                _accessToken = null;
            }
        }

        private async UniTask<DriveFilesResponse> ListDriveFiles()
        {
            string url = "https://www.googleapis.com/drive/v3/files?pageSize=10&fields=files(id,name)";

            using (var request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Drive list failed: " + request.error);
                    return null;
                }

                Debug.Log("Drive Files:\n" + request.downloadHandler.text);
                DriveFilesResponse response =
                    JsonConvert.DeserializeObject<DriveFilesResponse>(request.downloadHandler.text);
                return response;
            }
        }

        public async UniTask<GoogleSheetResponse> ReadSheetContent(string spreadsheetId, string range = "Sheet1", SheetDimension dimension = SheetDimension.ROWS)
        {
            try
            {
                string url =
                    $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?majorDimension={dimension.ToString()}";

                using (var request = UnityWebRequest.Get(url))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {_accessToken}");
                    await request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Failed to read sheet: {request.error}");
                        Debug.LogError(request.downloadHandler.text);
                        return null;
                    }

                    // Debug.Log("Sheet data raw JSON:\n" + request.downloadHandler.text);

                    // Optional: parse for easier access
                    var sheetData = JsonConvert.DeserializeObject<GoogleSheetResponse>(request.downloadHandler.text);

                    // Debug.Log($"📘 Sheet Range: {sheetData.range}");
                    // Debug.Log("📋 Sheet Values:");
                    // foreach (var row in sheetData.values)
                    // {
                    //     Debug.Log(string.Join(" | ", row));
                    // }

                    return sheetData;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error reading sheet: " + ex);
            }

            return null;
        }

        // -----------------------------
        // Helper Methods
        // -----------------------------
        private static string Base64UrlEncode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string CreateJwtSignature(string unsignedJwt, string privateKey)
        {
            try
            {
                // Clean up PEM
                privateKey = privateKey
                    .Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "");

                // Decode Base64 PKCS8 private key
                byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

                // Parse using BouncyCastle
                AsymmetricKeyParameter keyParameter = PrivateKeyFactory.CreateKey(privateKeyBytes);
                ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");

                signer.Init(true, keyParameter);
                byte[] data = Encoding.UTF8.GetBytes(unsignedJwt);
                signer.BlockUpdate(data, 0, data.Length);
                byte[] signature = signer.GenerateSignature();

                return Convert.ToBase64String(signature)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
            catch (Exception ex)
            {
                Debug.LogError("Error signing JWT with BouncyCastle: " + ex);
                return null;
            }
        }
    }
}