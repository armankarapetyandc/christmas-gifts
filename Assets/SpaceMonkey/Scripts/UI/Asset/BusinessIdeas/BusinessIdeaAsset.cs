using Services.AssetDatabaseService;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.BusinessIdeas
{
    [CreateAssetMenu(fileName = "BusinessIdeas", menuName = "SpaceMonkey/Resources/BusinessIdeasAsset", order = 0)]
    public class BusinessIdeaAsset : ScriptableAsset
    {
        [SerializeField] private Sprite businessIdeaItemAsset;
        [SerializeField] private string businessIdeaName;

        public Sprite BusinessIdeaItemAsset => businessIdeaItemAsset;
        public string BusinessIdeaName => businessIdeaName;
    }
}