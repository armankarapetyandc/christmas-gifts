using Cysharp.Threading.Tasks;

namespace SpaceMonkey.Scripts.Cloud
{
    public interface ICloudDataPatcher<T>
    {
        UniTask<T> Patch(CloudDataRestClient client, string spreadSheetId);
    }

    public interface ICloudDataWriter
    {
        
    }
}