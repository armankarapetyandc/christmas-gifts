using Services.AssetDatabaseService;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.BusinessIdeas
{
    [CreateAssetMenu(fileName = "BusinessIdeas", menuName = "Space Monkey/Database/BusinessIdeasAsset", order = 0)]
    public class BusinessIdeaAsset : ScriptableAsset
    {
        [SerializeField] private Sprite businessIdeaItemSprite;
        [SerializeField] private string businessIdeaName;

        public Sprite BusinessIdeaItemSprite => businessIdeaItemSprite;
        public string BusinessIdeaName => businessIdeaName;
    }
}