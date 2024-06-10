using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어 현재 HP, MP 관리한다.
/// 필요한 곳에 이벤트를 등록해서 HP, MP 바뀔 때 사용하기
/// </summary>
public class PlayerHeartStamina : MonoBehaviour
{
    public event Action OnChangeHealthMana;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    private float _hearttimeSinceLastChange = float.MaxValue;
    private float _staminatimeSinceLastChange = float.MaxValue;

    private bool _isAttacked = false;

    public int CurrentHeart { get; private set; }
    public float CurrentStamina { get; private set; }
    public int MaxHeart => _playerStatHandler.CurrentAttribute.maxHeart;
    public float MaxStamina => _playerStatHandler.CurrentAttribute.maxStamina;

    private PlayerAttributeHandler _playerStatHandler;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerStatHandler = gameObject.GetOrAddComponent<PlayerAttributeHandler>();
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        //LoadHeartStamina();
    }

    void Update()
    {
        if (_isAttacked && _hearttimeSinceLastChange < _playerStatHandler.CurrentAttribute.heartChangeDelay)
        {
            _hearttimeSinceLastChange += Time.deltaTime;

            if (_hearttimeSinceLastChange >= _playerStatHandler.CurrentAttribute.heartChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
                _isAttacked = false;
            }
        }

        // Debug.Log($"Heart : {CurrentHeart} / {MaxHeart}, Stamina : {CurrentStamina} / {MaxStamina}");  

        // TODO : 마나 회복 코루틴 써서 구현하기
        _staminatimeSinceLastChange += Time.deltaTime;

        if (CurrentStamina < MaxStamina && _staminatimeSinceLastChange >= _playerStatHandler.CurrentAttribute.staminaChangeDelay)
        {
            ChangeStamina(_playerStatHandler.CurrentAttribute.staminaFillAmount);
            _staminatimeSinceLastChange = 0f;
        }
    }

    public void SetHeart(int heart)
    {
        CurrentHeart = heart;
    }

    public void SetStamina(float stamina)
    {
        CurrentStamina = stamina;
    }

    public bool ChangeHeart(int change)
    {
        // 체력이 0이면 false 반환
        if (_hearttimeSinceLastChange < _playerStatHandler.CurrentAttribute.heartChangeDelay)
        {
            return false;
        }

        _hearttimeSinceLastChange = 0f;
        CurrentHeart += change;
        CurrentHeart = Mathf.Clamp(CurrentHeart, 0, MaxHeart);

        OnChangeHealthMana?.Invoke();

        if (CurrentHeart <= 0)
        {
            CallDeath();
            return true;
        }
        if (change > 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
            _isAttacked = true;
        }
        return true;
    }

    public bool ChangeStamina(float change)
    {
        Debug.Log($"CurrentStamina : {CurrentStamina}, change : {change}");

        // 마나가 부족하면 false 반환
        if (change < 0 && CurrentStamina < Mathf.Abs(change))
        {
            return false;
        }

        OnChangeHealthMana?.Invoke();

        _staminatimeSinceLastChange = 0f;
        CurrentStamina += change;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);

        return true;
    }

    private void CallDeath()
    {
        Debug.Log("PlayerHeartStamina::CallDeath()");
        OnDeath?.Invoke();
    }
}
