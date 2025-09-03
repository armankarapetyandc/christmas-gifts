using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffProfessionTab : MonoBehaviour
    {
        [SerializeField] private StaffProfessionEnum staffProfessionEnum;
        [SerializeField] private Sprite selectedStateSprite;
        [SerializeField] private Sprite deselectedStateSprite;
        [SerializeField] private Image image;
        [SerializeField] internal Toggle toggle;
        public Observable<StaffProfessionEnum> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b).Select(_ => staffProfessionEnum);
        
        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(OnStateChanged).AddTo(this);
        }
        
        private void OnStateChanged(bool state)
        {
            image.sprite = state ? selectedStateSprite : deselectedStateSprite;
        }
    }
}