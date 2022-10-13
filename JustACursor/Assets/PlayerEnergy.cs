using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private PlayerInput inputs;
    
    [SerializeField] private Image timeFill;

    [SerializeField] private float timeToDecrease;
    [SerializeField, Range(1,2)] private float speedUpValue;
    [SerializeField, Range(0.1f,1)] private float slowDownValue;
    
    public static float gameSpeed = 1;

    private void Start()
    {
        inputs = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (inputs.GetActionPressed(PlayerInput.InputAction.SlowDown))
        {
            if (timeFill.fillAmount == 0) return;

            gameSpeed = slowDownValue;
            timeFill.DOKill();
            timeFill.DOFillAmount(0, timeToDecrease * timeFill.fillAmount).SetEase(Ease.Linear).OnComplete((ResetSpeed));
        }

        else if (inputs.GetActionPressed(PlayerInput.InputAction.SpeedUp))
        {
            gameSpeed = speedUpValue;
            
            if (timeFill.fillAmount == 1) return;
            
            timeFill.DOKill();
            timeFill.DOFillAmount(1, timeToDecrease-timeToDecrease*timeFill.fillAmount).SetEase(Ease.Linear).OnComplete((ResetSpeed));
        }
        
        else
        {
            ResetSpeed();
        }
        
        
    }

    private void ResetSpeed()
    {
        timeFill.DOKill();
        gameSpeed = 1;
    }
}
