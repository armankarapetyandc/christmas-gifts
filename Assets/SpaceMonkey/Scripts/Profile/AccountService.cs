using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using Logger = DCLogger.Runtime.Logger;

namespace SpaceMonkey.Scripts.Profile
{
    public class AccountService : IDisposable
    {
        private const string FILENAME = "Account";
        private static readonly string _path = Path.Combine(Application.persistentDataPath, FILENAME);

        public AccountModel Model { get; private set; }

        public bool IsFreshAccount => !File.Exists(_path);

        public void CreateNewAccount()
        {
            Model = new AccountModel(Account.CreateEmpty());
        }

        public async UniTask SaveAsync()
        {
            if (Model?.Account == null)
            {
                throw new Exception("Account is null");
            }

            var content = JsonConvert.SerializeObject(Model.Account);
            await File.WriteAllTextAsync(_path, content);
        }

        public async UniTask LoadAsync()
        {
            if (!File.Exists(_path))
            {
                throw new Exception("Account file doesn't exist");
            }

            var content = await File.ReadAllTextAsync(_path);
            var account = JsonConvert.DeserializeObject<Account>(content);
            Model = new AccountModel(account);
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

        public void Dispose()
        {
            Model?.Dispose();
        }
    }
}