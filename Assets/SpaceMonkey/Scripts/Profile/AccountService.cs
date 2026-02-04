using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using SpaceMonkey.Scripts.Analytics;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto;
using SpaceMonkey.Scripts.UI.Views.UpgradeCapacity;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
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

        
        public readonly ReactiveCommand<int> OnLevelChanged = new ReactiveCommand<int>();
        private PresenterService _presenterService;
        public AccountModel Model { get; private set; }

        public bool IsFreshAccount => !File.Exists(_path);

        public AccountService(GameConfig gameConfig, PopupPresenterService popupPresenterService,PresenterService presenterService)
        {
            _presenterService = presenterService;
            _popupPresenterService = popupPresenterService;
            _gameConfig = gameConfig;
        }

        public Product GetCompetitionProduct()
        {
            return Model.Account.Products.FirstOrDefault(p =>
                100 * p.ProductPrice / p.MaxProductPrice > _gameConfig.CompetitionValue);
        }
        public void CreateNewAccount()
        {
            PlayerPrefs.DeleteAll();
            var freeProdCap = _gameConfig.ProductionLevels.Single(info => info.ProdCapCost == 0);
            Model = new AccountModel(Account.CreateEmpty(freeProdCap));
            Model.Account.OnScoreChanged.Subscribe(eventParam =>
            {
                SfxPlayer.Play(Sounds.Score_Awarded);
                LevelInfo levelInfo = _gameConfig.LevelInfos.LastOrDefault(info => info.Score <= eventParam);
                   
                int currentLevel = Model.Account.Level;
                Model.Account.Level = levelInfo?.Level ?? _gameConfig.LevelInfos.Length;
                if (currentLevel < Model.Account.Level)
                {
                    SfxPlayer.Play(Sounds.Company_Level_Up);
                    OnLevelChanged.Execute(Model.Account.Level);
                    _popupPresenterService.Show<LevelInfoAutoPopup>(isAsync:false).Forget();
                    AnalyticsProvider.SendEvent(AnalyticsEvents.LevelChanged,new Dictionary<string, string>()
                    {
                        {"company_name",Model.Account.Company.CompanyName},
                        {"level",Model.Account.Level.ToString()},
                        {"score",Model.Account.Score.ToString()},
                        {"cash",Model.Account.Money.ToString()},
                    });
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
            Model.Account.OnScoreChanged.Subscribe(eventParam =>
            {
                SfxPlayer.Play(Sounds.Score_Awarded);
                int currentLevel = Model.Account.Level;
                LevelInfo levelInfo = _gameConfig.LevelInfos.LastOrDefault(info => info.Score <= eventParam);
                Model.Account.Level = levelInfo!.Level;

                if (currentLevel < Model.Account.Level)
                {
                    OnLevelChanged.Execute(Model.Account.Level);
                    _popupPresenterService.Show<LevelInfoAutoPopup>().Forget();
                }
            }).AddTo(_compositeDisposable);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Space Monkey/Account/Delete Account")]
#endif
        public static void DeleteAccount()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            if (!File.Exists(_path))
            {
                Logger.LogError($"Account file doesn't exist. Path: {_path}", SpaceMonkeyLogChannels.Default);
            }
            else
            {
                File.Delete(_path);
                Logger.Log($"Account file deleted! Path: {_path}", SpaceMonkeyLogChannels.Default);
            }
        }

        public void Dispose()
        {
            Model?.Dispose();
            _compositeDisposable?.Dispose();
        }
    }
}