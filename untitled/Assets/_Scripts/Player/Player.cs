using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Player : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnHPChange = new();
    [HideInInspector] public UnityEvent<int> OnStreamChange = new();
    [HideInInspector] public UnityEvent<int> OnBarrierChange = new();
    [HideInInspector] public UnityEvent<int> OnEnegryChange = new();


    [SerializeField] private GameObject _barrierPrefab;
    [SerializeField] private Material _ditheringMaterial;
    [SerializeField] private float _ditherRestoreTime = 1.2f;
    [SerializeField] private float _evasionDitherOpacity = 0.2f;
    [SerializeField] private Transform _rightHandRig;

    [SerializeField] private List<Projectile> _attackProjectiles;
    private Projectile _projectile;


    [Inject] private GameManager _gameManager;
    [Inject] private FightManager _fightManager;

    [SerializeField] private float _streamRegenCoef = 1.001f;
    private float _maxRegenRate => _playerStats.StreamRegen * 3.5f;

    private int _currentBarrierDurability;

    public bool CanSetBarrier => _stream >= _barrierDurability;
    public bool CanAttack => _stream >= _attackStreamCost;
    public bool CanUltimate => CurrentEnergy == EnergyForUltimate;

    public int HP => _HP;
    [SerializeField] private int _HP = 100;
    public int Stream => _stream;
    [SerializeField] private int _stream = 100;
    public float StreamRegen => _streamRegen;
    [SerializeField] private float _streamRegen = 100;
    public int EvasionChance => _evasionChance;
    [SerializeField] private int _evasionChance = 100;
    public int BarrierDurability => _barrierDurability;
    [SerializeField] private int _barrierDurability = 100;
    public AttackType AttackType => _attackType;
    [SerializeField] private AttackType _attackType;
    public int Damage => _damage;
    [SerializeField] private int _damage = 100;
    public float AttackRate => _attackRate;
    [SerializeField] private float _attackRate = 100;
    public int AttackStreamCost => _attackStreamCost;
    [SerializeField] private int _attackStreamCost = 100;
    public int EnergyForUltimate => _energyForUltimate;
    [SerializeField] private int _energyForUltimate;
    public int CurrentEnergy { get; private set; }

    private PlayerStats _playerStats;

    private void Awake()
    {
        _playerStats = _gameManager.PlayerStats;
        _HP = _playerStats.HP;
        _stream = _playerStats.StreamCapacity;
        _streamRegen = _playerStats.StreamRegen;
        _evasionChance = _playerStats.EvasionChance;
        _barrierDurability = _playerStats.BarrierDurability;
        _damage = _playerStats.Damage;
        _attackRate = _playerStats.AttackRate;
        _attackStreamCost = _playerStats.AttackStreamCost;
    }

    private void Start()
    {
        _projectile = _attackProjectiles[(int)AttackType];
        switch (AttackType)
        {
            case AttackType.Explosion:
                _energyForUltimate = 20;
                break;
        }
        ChangeEnergy(0);
        ChangeHP(_HP);
        ChangeStream(_stream);
        ChangeBarrier(0);
        StartCoroutine(RegenerateStream());
    }

    private IEnumerator RegenerateStream()
    {
        float accumulatedTime = 0f;
        while (true)
        {
            if (_stream < _playerStats.StreamCapacity)
            {
                accumulatedTime += Time.deltaTime;
                _streamRegen = Mathf.Min(_streamRegen * _streamRegenCoef, _maxRegenRate);
                int streamToRegen = Mathf.FloorToInt(_streamRegen * accumulatedTime);
                if (streamToRegen > 0)
                {
                    if (_stream + streamToRegen > _playerStats.StreamCapacity)
                        ChangeStream(_playerStats.StreamCapacity);
                    else
                        ChangeStream(_stream + streamToRegen);
                    accumulatedTime -= streamToRegen / _streamRegen;
                }
            }
            yield return null;
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void ApplyDamage(int damage)
    {
        if (WillEvade())
        {
            StartCoroutine(Evade());
            return;
        }
        Debug.Log("DUR " + _currentBarrierDurability);
        if (damage <= 0) return;
        if (_currentBarrierDurability > 0)
        {
            if (_currentBarrierDurability > damage)
            {
                ChangeBarrier(_currentBarrierDurability - damage);
                return;
            }
            else
            {
                damage -= _currentBarrierDurability;
                ChangeBarrier(0);
            }
        }
        if (_HP - damage > 0)
        {
            ChangeHP(_HP - damage);
            Debug.Log($"HP: {_HP}");
        }
        else
        {
            ChangeHP(0);
            Lose();
        }
    }

    private bool WillEvade()
    {
        if (Random.Range(0, 101) <= _evasionChance)
            return true;
        return false;
    }

    private IEnumerator Evade()
    {
        _ditheringMaterial.SetFloat("_Opacity", _evasionDitherOpacity);
        float timeElapsed = 0;
        while (timeElapsed / _ditherRestoreTime < 1)
        {
            _ditheringMaterial.SetFloat("_Opacity", Mathf.Lerp(_evasionDitherOpacity, 1, timeElapsed / _ditherRestoreTime));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void ChangeHP(int newHP)
    {
        _HP = newHP;
        OnHPChange.Invoke(_HP);
    }

    private void ChangeEnergy(int newEnergy)
    {
        CurrentEnergy = newEnergy;
        OnEnegryChange.Invoke(CurrentEnergy);
    }

    private void ChangeStream(int newStream)
    {
        if (newStream <= _stream)
            _streamRegen = _playerStats.StreamRegen;
        _stream = newStream;
        OnStreamChange.Invoke(_stream);
    }

    private void ChangeBarrier(int newBarrier)
    {
        if (newBarrier <= 0)
            _barrierPrefab.SetActive(false);
        _currentBarrierDurability = newBarrier;
        OnBarrierChange.Invoke(_currentBarrierDurability);
    }

    private void Lose()
    {
        Debug.Log("Proebal lox");
    }

    public void Attack(int attackTypeEnum)
    {
        ChangeStream(_stream - _attackStreamCost);
        ChangeEnergy(++CurrentEnergy);
        GameObject proj = Instantiate(_projectile.gameObject, _rightHandRig.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(EntityType.Player, _damage);
        switch ((AttackType)attackTypeEnum)
        {
            case AttackType.Explosion:
                DOTween.Sequence().Append(proj.transform.DOMoveY(transform.position.y + 1, 0.3f).SetEase(Ease.OutCirc)).Append(proj.transform.DOMove(new Vector3(0, 6, 18), 1.1f).SetEase(Ease.InOutExpo));
                break;
        }
    }

    public void Ultimate(int attackTypeEnum)
    {
        ChangeEnergy(0);
        switch ((AttackType)attackTypeEnum)
        {
            case AttackType.Explosion:
                break;
        }
    }

    public void SetBarrier()
    {
        _barrierPrefab.SetActive(true);
        ChangeStream(_stream - _barrierDurability);
        ChangeBarrier(_barrierDurability);
    }
}
