using R3;
using UIService.Runtime.Core;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Navigation.Core
{
    [RequireComponent(typeof(Canvas))]
    public class NavigationPresenterView : MonoBehaviour, IInitializable
    {
        [SerializeField] private Canvas canvas;


        private NavigationPresenterService _presenterService;

        public Canvas Canvas => canvas;

        [Inject]
        private void Inject(NavigationPresenterService presenterService)
        {
            _presenterService = presenterService;
        }

        protected virtual void Awake() => DontDestroyOnLoad(gameObject);


        public void Initialize()
        {
            _presenterService.PanelShowObservable.Subscribe(OnNewPresenter).AddTo(this);
        }

        private void OnNewPresenter(InitializablePresenter presenter)
        {
            Debug.Log($"Show panel presenter: {presenter.name}");
            var presenterRect = presenter.GetComponent<RectTransform>();
            DontDestroyOnLoad(presenter.gameObject);
            presenterRect.SetParent(transform, false);
            presenterRect.anchorMin = new Vector2(0f, 0f);
            presenterRect.anchorMax = new Vector2(1f, 1f);
            presenterRect.offsetMin = new Vector2(0f, 0f);
            presenterRect.offsetMax = new Vector2(0f, 0f);
        }
    }
}