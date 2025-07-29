using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class BusinessDetailsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField businessNameInputField;
        [SerializeField] private Button generateBusinessNameButton;
        [SerializeField] private Button iconBuilderButton;
        [SerializeField] private Button hashTagPanelButton;
        [SerializeField] private TextMeshProUGUI hashtagsCountText;
        [SerializeField] private HashtagListItem[] hashtagItems;
        [SerializeField] private Button saveButton;

        public Observable<Unit> OnIconButtonClicked => iconBuilderButton.OnClickAsObservable();
    }
}