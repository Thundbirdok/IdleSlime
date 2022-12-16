using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using GameResources.Enemies.Scripts;
    using GameResources.Health.Scripts;
    using GameResources.Slime.Scripts;
    using UnityEngine.Pool;

    public class HealthBarsService : MonoBehaviour
    {
        [SerializeField]
        private Slime player;
        
        [SerializeField]
        private EnemiesHandler enemiesHandler;

        [SerializeField]
        private HealthBar prefab;

        [SerializeField]
        private Transform container;

        [SerializeField]
        private Camera locationCamera;

        [SerializeField]
        private RectTransform localRect;

        [SerializeField]
        private GameOver gameOver;
        
        private List<HealthBar> CurrentViews => _viewsArray[_viewsActiveIndex];
        private List<HealthBar> NewViews => _viewsArray[_viewsActiveIndex == 0 ? 1 : 0];

        private readonly List<HealthBar>[] _viewsArray = new List<HealthBar>[2];

        private int _viewsActiveIndex;
        
        private ObjectPool<HealthBar> _pool;

        private void Awake()
        {
            InitViewsContainers();
            InitPool();
        }

        private void OnEnable()
        {
            SetViews();
            
            Subscribe();
        }

        private void OnDisable() => Unsubscribe();

        private void OnDestroy()
        {
            DestroyViewContainer();
            DestroyPool();
        }

        private void Update() => MoveViews();

        private void InitPool()
        {
            _pool = new ObjectPool<HealthBar>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        }

        private void DestroyPool() => _pool = null;

        private static void ActionOnDestroy(HealthBar healthBar) => Destroy(healthBar.gameObject);

        private static void ActionOnRelease(HealthBar healthBar)
        {
            healthBar.gameObject.SetActive(false);
        }

        private static void ActionOnGet(HealthBar healthBar)
        {
            healthBar.gameObject.SetActive(true);
        }

        private HealthBar CreateFunc()
        {
            var bar = Instantiate(prefab, container);
            
            bar.gameObject.SetActive(false);

            return bar;
        }

        private void InitViewsContainers()
        {
            for (var index = 0; index < _viewsArray.Length; index++)
            {
                _viewsArray[index] = new List<HealthBar>();
            }
        }

        private void DestroyViewContainer()
        {
            for (var index = 0; index < _viewsArray.Length; index++)
            {
                _viewsArray[index] = null;
            }
        }
        
        private void Subscribe()
        {
            enemiesHandler.OnUpdateObjects += SetViews;

            gameOver.OnOpen += Hide;
            gameOver.OnClose += Show;
        }

        private void Unsubscribe()
        {
            enemiesHandler.OnUpdateObjects -= SetViews;
            
            gameOver.OnOpen -= Hide;
            gameOver.OnClose -= Show;
        }

        private void Show()
        {
            container.gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            container.gameObject.SetActive(false);
        }

        private void MoveViews()
        {
            foreach (var healthBar in CurrentViews)
            {
                if (healthBar.Damagable.IsDead)
                {
                    return;
                }

                var worldToScreenPoint = RectTransformUtility.WorldToScreenPoint
                (
                    locationCamera,
                    healthBar.Damagable.HealthBarPosition
                );

                RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    localRect,
                    worldToScreenPoint,
                    null,
                    out var anchoredPosition
                );

                var viewRectTransform = healthBar.transform as RectTransform;

                if (viewRectTransform != null)
                {
                    viewRectTransform.anchoredPosition = anchoredPosition;
                }
            }
        }
        
        private void SetViews()
        {
            SetNewViews();

            ReleaseUnusedViews();

            SetNewViewsAsCurrent();
        }

        private void SetNewViewsAsCurrent() => _viewsActiveIndex = _viewsActiveIndex == 0 ? 1 : 0;

        private void ReleaseUnusedViews()
        {
            foreach (var oldView in CurrentViews.Where(oldView => !NewViews.Contains(oldView)))
            {
                _pool.Release(oldView);
            }
        }

        private void SetNewViews()
        {
            NewViews.Clear();

            SetNewView(player, false);
            
            foreach (var enemy in enemiesHandler.Enemies)
            {
                SetNewView(enemy, true);
            }
        }

        private void SetNewView(IDamagable damagable, bool isEnemy)
        {
            var oldView = CurrentViews
                .FirstOrDefault(x => x.Damagable.Equals(damagable));

            if (oldView != null)
            {
                NewViews.Add(oldView);

                return;
            }

            var view = _pool.Get();

            view.SetData(damagable, isEnemy);

            NewViews.Add(view);
        }
    }
}
