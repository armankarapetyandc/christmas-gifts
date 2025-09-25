using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using SpaceMonkey.Scripts.Configs;
using UnityEngine;
using Logger = DCLogger.Runtime.Logger;

namespace SpaceMonkey.Scripts.Profile
{
    public class AccountService : IDisposable
    {
        private readonly GameConfig _gameConfig;
        private const string FILENAME = "Account.spacemonkey";
        private static readonly string _path = Path.Combine(Application.persistentDataPath, FILENAME);

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public AccountModel Model { get; private set; }

        public bool IsFreshAccount => !File.Exists(_path);

        public AccountService(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public void CreateNewAccount()
        {
            PlayerPrefs.DeleteAll();
            var freeProdCap = _gameConfig.ProductionLevels.Single(info => info.ProdCapCost == 0);
            Model = new AccountModel(Account.CreateEmpty(freeProdCap));
            Model.Account.OnScoreChanged.Subscribe(score =>
            {
                Model.Account.Level = 1 + _gameConfig.LevelScoreRanges.TakeWhile(t => score >= t).Count();
            }).AddTo(_compositeDisposable);
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
            _compositeDisposable?.Dispose();
        }
    }
}