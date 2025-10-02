using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto;
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
        private PopupPresenterService _popupPresenterService;

        public AccountModel Model { get; private set; }

        public bool IsFreshAccount => !File.Exists(_path);

        public AccountService(GameConfig gameConfig, PopupPresenterService popupPresenterService)
        {
            _popupPresenterService = popupPresenterService;
            _gameConfig = gameConfig;
        }

        public void CreateNewAccount()
        {
            PlayerPrefs.DeleteAll();
            var freeProdCap = _gameConfig.ProductionLevels.Single(info => info.ProdCapCost == 0);
            Model = new AccountModel(Account.CreateEmpty(freeProdCap));
            Model.Account.OnScoreChanged.Subscribe(score =>
            {
                LevelInfo levelInfo = _gameConfig.LevelInfos.Where(info => info.Score <= score)
                    .DefaultIfEmpty(_gameConfig.LevelInfos[0]).Max();
                int currentLevel = Model.Account.Level;
                Model.Account.Level = levelInfo?.Level ?? _gameConfig.LevelInfos.Length;
                if (currentLevel != 1 && currentLevel < Model.Account.Level)
                {
                    _popupPresenterService.Show<LevelInfoAutoPopup>().Forget();
                }
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
            }
            else
            {
                File.Delete(_path);
                Logger.Log($"Account file deleted! Path: {_path}", SpaceMonkeyLogChannels.Default);
            }

            if (!File.Exists(CreditSimulator.Path))
            {
                Logger.LogError($"CreditSimulator file doesn't exist. Path: {CreditSimulator.Path}",
                    SpaceMonkeyLogChannels.Default);
            }
            else
            {
                File.Delete(CreditSimulator.Path);
                Logger.Log($"CreditSimulator file deleted! Path: {CreditSimulator.Path}",
                    SpaceMonkeyLogChannels.Default);
            }
        }

        public void Dispose()
        {
            Model?.Dispose();
            _compositeDisposable?.Dispose();
        }
    }
}