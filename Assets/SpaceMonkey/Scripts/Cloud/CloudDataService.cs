using ContextLoaderService.Runtime;
using ContextLoaderService.Runtime.BaseUnits;
using SpaceMonkey.Scripts.Configs;

namespace SpaceMonkey.Scripts.Cloud
{
    public class CloudDataService
    {
        private const string SpreadSheetId = "13LHz9pq6QP2DXH4aUtMnVAHHRMsD3laqY7VQsZ_hrAs";
        private readonly CloudDataRestClient _client;
        private readonly GameConfig _gameConfig;

        public CloudDataService(CloudDataRestClient client, GameConfig gameConfig)
        {
            _client = client;
            _gameConfig = gameConfig;
        }

        public UniLoadingUnit Initialize()
        {
            return _client.Initialize().ToLoadingUnit();
        }

        public UniLoadingUnit<T> Patch<TPatcher, T>() where TPatcher : ICloudDataPatcher<T>, new()
        {
            var patcher = new TPatcher();
            return patcher.Patch(_client, SpreadSheetId).ToLoadingUnit();
        }
    }
}