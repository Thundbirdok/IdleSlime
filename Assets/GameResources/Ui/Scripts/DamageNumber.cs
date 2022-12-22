using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using System;
    using System.Collections;
    using DG.Tweening;
    using TMPro;

    public class DamageNumber : MonoBehaviour
    {
        public event Action<DamageNumber> OnFade;

        [SerializeField]
        private RectTransform rectTransform;
        
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private CanvasGroup group;
        
        [SerializeField]
        private float lifeSeconds = 0.5f;

        [SerializeField]
        private Color healColor = Color.green;
        
        [SerializeField]
        private Color damageColor = Color.red;
        
        [SerializeField]
        private DamageScale[] damageScales;
        
        private Coroutine _coroutine;

        private Sequence _sequence;

        private Vector3 _startScale;
        
        private void OnDisable()
        {
            if (_coroutine == null)
            {
                return;
            }

            _sequence.Pause();
            
            StopCoroutine(_coroutine);

            _coroutine = null;
        }

        public void SetValue(in int damage)
        {
            var absDamage = Mathf.Abs(damage);
            
            SetText(absDamage, damage > 0);

            _coroutine = StartCoroutine(Wait(absDamage));
        }

        private void PrepareSequence(Vector3 startScale)
        {
            if (startScale == _startScale)
            {
                return;
            }

            _startScale = startScale;
            
            var inSequence = SetInSequence();

            var staySequence = SetStaySequence();
            
            var outSequence = SetOutSequence();

            _sequence = DOTween.Sequence();

            _sequence
                .Append(inSequence)
                .Append(staySequence)
                .Append(outSequence)
                .SetAutoKill(false)
                .Pause();
        }

        private Sequence SetInSequence()
        {
            var scaleIn = rectTransform
                .DOScale(1, lifeSeconds * 0.4f);

            var fadeIn = group
                .DOFade(1, lifeSeconds * 0.1f)
                .From(0);

            var inSequence = DOTween.Sequence();
            
            inSequence
                .Join(scaleIn)
                .Join(fadeIn);

            return inSequence;
        }

        private Sequence SetStaySequence()
        {
            var staySequence = DOTween.Sequence();
            
            staySequence
                .AppendInterval(lifeSeconds * 0.3f);

            return staySequence;
        }
        
        private Sequence SetOutSequence()
        {
            var scaleOut = rectTransform
                .DOScale(0, lifeSeconds * 0.3f);

            var fadeOut = group
                .DOFade(0, lifeSeconds * 0.3f)
                .From(0);

            var outSequence = DOTween.Sequence();

            outSequence
                .Join(scaleOut)
                .Join(fadeOut);

            return outSequence;
        }

        private void SetText(int damage, bool isHeal)
        {
            text.text = (isHeal ? '+' : "") + damage.ToString();
            text.color = isHeal ? healColor : damageColor;
        }

        private IEnumerator Wait(int damage)
        {
            gameObject.SetActive(true);

            var startScale = GetStartScale(damage) * Vector3.one;

            rectTransform.localScale = startScale;
            
            PrepareSequence(startScale);
            
            _sequence.Restart();
            
            yield return _sequence
                .Play()
                .WaitForCompletion();

            gameObject.SetActive(false);
            
            OnFade?.Invoke(this);
        }

        private float GetStartScale(in int damage)
        {
            if (damageScales == null || damageScales.Length == 0)
            {
                return 1;
            }

            foreach (var damageScale in damageScales)
            {
                if (damageScale.Damage >= damage)
                {
                    return damageScale.Scale;
                }
            }

            return damageScales[^1].Scale;
        }
        
        [Serializable]
        private class DamageScale
        {
            [field: SerializeField]
            public int Damage { get; private set; }

            [field: SerializeField]
            public float Scale { get; private set; }
        }
    }
}
