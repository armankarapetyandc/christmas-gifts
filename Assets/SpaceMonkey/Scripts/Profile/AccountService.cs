using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using Logger = DCLogger.Runtime.Logger;

namespace SpaceMonkey.Scripts.Profile
{
    public class AccountService
    {
        private const string FILENAME = "Account";
        private static readonly string _path = Path.Combine(Application.persistentDataPath, FILENAME);

        public Account Account { get; private set; }

        public bool IsFreshAccount => !File.Exists(_path);

        public void CreateNewAccount()
        {
            Account = Account.CreateEmpty();
        }

        public async UniTask SaveAsync()
        {
            if (Account == null)
            {
                throw new Exception("Account is null");
            }

            var content = JsonConvert.SerializeObject(Account);
            await File.WriteAllTextAsync(_path, content);
        }

        public async UniTask LoadAsync()
        {
            if (!File.Exists(_path))
            {
                throw new Exception("Account file doesn't exist");
            }

            var content = await File.ReadAllTextAsync(_path);
            Account = JsonConvert.DeserializeObject<Account>(content);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Space Monkey/Account/Delete Account")]
#endif
        public static void DeleteAccount()
        {
            if (!File.Exists(_path))
            {
                Logger.LogError($"Account file doesn't exist. Path: {_path}", SpaceMonkeyLogChannels.Default);
                return;
            }

            File.Delete(_path);
            Logger.Log($"Account file deleted! Path: {_path}", SpaceMonkeyLogChannels.Default);
        }
    }
}