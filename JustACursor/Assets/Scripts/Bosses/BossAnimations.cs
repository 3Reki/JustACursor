using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Bosses
{
    public class BossAnimations : MonoBehaviour
    {
        [SerializeField] private new Animation animation;
        [SerializeField] private SpriteRenderer bodySprite;

        [field: SerializeField] public float attackAnimLength { get; private set; }
        [field: SerializeField] public float phaseChangeAnimLength { get; private set; }
        [field: SerializeField] public float hitAnimLength { get; private set; }
        [field: SerializeField] public float deathAnimLength { get; private set; }

        [SerializeField] private UnityEvent onAttack;
        [SerializeField] private UnityEvent onHit;
        [SerializeField] private UnityEvent onPhaseChange;
        [SerializeField] private UnityEvent onDeath;

        private Color defaultColor;

        private void Awake()
        {
            defaultColor = bodySprite.color;
        }

        public void Attack()
        {
            onAttack.Invoke();
        }

        public void Hit()
        {
            onHit.Invoke();
        }

        public void ChangePhase()
        {
            onPhaseChange.Invoke();
        }

        public void Die()
        {
            onDeath.Invoke();
        }

        public void PlayAnimation(AnimationClip clip)
        {
            animation.clip = clip;
            animation.Play();
        }

        public void ColorLerp()
        {
            bodySprite.color = Color.red;
            bodySprite.DOColor(defaultColor, 0.1f);
        }
    }
}