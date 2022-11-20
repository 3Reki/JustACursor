using BulletPro;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

public class Energy : MonoBehaviour
{
    [SerializeField] private EnergyData data;
    
    public static float GameSpeed
    {
        get => _gameSpeed;
        private set
        {
            _gameSpeed = value;
            BulletModuleMovement.SpeedMultiplier = value;
            onGameSpeedUpdate?.Invoke();
        }
    }
    
    private static float _gameSpeed = 1;
    
    private float tweenTime;
    
    private TimeState timeState = TimeState.Resetting;
    
    public bool SpeedUpTime()
    {
        if (timeState == TimeState.Speeding) return false;
            
        timeState = TimeState.Speeding;
            
        DOTween.Kill(GameSpeed);
        tweenTime = data.lerpDuration - Mathf.Lerp(0, data.lerpDuration, (GameSpeed - 1) / (data.speedUpModifier - 1));
        DOTween.To(() => GameSpeed, x => GameSpeed = x, data.speedUpModifier, tweenTime).SetEase(data.lerpEase);
            
        onSpeedUp?.Invoke();
        return true;
    }

    public bool SlowDownTime()
    {
        if (timeState == TimeState.Slowing) return false;
            
        timeState = TimeState.Slowing;

        DOTween.Kill(GameSpeed);
        tweenTime = data.lerpDuration - Mathf.Lerp(0, data.lerpDuration, (1-GameSpeed)/(1-data.slowDownModifier));
        DOTween.To(() => GameSpeed, x => GameSpeed = x, data.slowDownModifier, tweenTime).SetEase(data.lerpEase);
            
        onSlowDown?.Invoke();
        return true;
    }

    public bool ResetSpeed()
    {
        if (timeState == TimeState.Resetting) return false;
            
        timeState = TimeState.Resetting;

        DOTween.Kill(GameSpeed);
        if (GameSpeed > 1) tweenTime = Mathf.Lerp(0,data.lerpDuration, (GameSpeed - 1) / (data.speedUpModifier - 1));
        else tweenTime = Mathf.Lerp(0,data.lerpDuration,(1-GameSpeed)/(1-data.slowDownModifier));
        DOTween.To(() => GameSpeed, x => GameSpeed = x, 1, data.lerpDuration).SetEase(data.lerpEase);
            
        onReset?.Invoke();
        return true;
    }

    public static UnityEvent onSpeedUp;
    public static UnityEvent onSlowDown;
    public static UnityEvent onReset;
        
    public delegate void OnGameSpeedUpdate();
    public static OnGameSpeedUpdate onGameSpeedUpdate;
    
    private enum TimeState
    {
        Slowing,
        Resetting,
        Speeding
    }
}
