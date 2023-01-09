using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    public class BasicMovement : MonoBehaviour
    {
        [SerializeField] private Movement movementAxis;
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField, Range(1,30)] private float amplitude = 15;
        [SerializeField, Range(0.1f,20)] private float period = 10;
        [SerializeField] private Ease movementEase = Ease.Linear;
        
        
        
        private Vector2 startPosition;
        private float curveTime;
        
        private void Awake()
        {
            startPosition = transform.position;
        }

        private void Start()
        {
            if (movementAxis == Movement.None) return;
            
            StartCoroutine(MovementLoop());
        }
        
        private IEnumerator MovementLoop()
        {
            float newPos = movementCurve.Evaluate(curveTime)*amplitude;
            float duration = Time.deltaTime * Energy.GameSpeed / period;
            
            transform.DOComplete();
            if (movementAxis == Movement.Horizontal)
            {
                transform.DOMoveX(startPosition.x+newPos, duration).SetEase(movementEase);
            }
            else if (movementAxis == Movement.Vertical)
            {
                transform.DOMoveY(startPosition.y+newPos, duration).SetEase(movementEase);
            }
            
            curveTime += duration;
            curveTime %= 1;

            yield return new WaitForSeconds(duration);
            StartCoroutine(MovementLoop());
        }

        public void StopMovement()
        {
            transform.DOKill();
        }
        
        private enum Movement { None, Horizontal, Vertical }
    }
}