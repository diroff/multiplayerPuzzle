using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeToRespawn;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _acceleration = 52f;
    [SerializeField] private float _decceleration = 52f;
    [SerializeField] private float _turnSpeed = 80f;

    [Header("Camera Following")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smothing = 1f;

    [Header("Shooting")]
    [SerializeField] private Weapon _weapon;

    private CoinsDisplayer _coinsDisplayer;

    private HealthComponent _health;
    
    private float _timeFromDie;
    private int _coins;

    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    private Vector2 _input;

    private float _maxSpeedChange;
    private float _speedLimiter = 0.7f;

    private bool _isPressingKey;
    private bool _normalSprite = true;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

    public int Coins => _coins;
    public HealthComponent Health => _health;

    public UnityAction<int> CoinsCountChanged;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<HealthComponent>();
        _camera = Camera.main;
    }

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        _coinsDisplayer = FindObjectOfType<CoinsDisplayer>(); //fix it please
        _coinsDisplayer.SetPlayer(this);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!_health.IsAlive)
        {
            bool canRespawn = _timeFromDie >= _timeToRespawn;

            if (!canRespawn)
            {
                _timeFromDie += Time.deltaTime;
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
                Respawn();
            
            return;
        }

        GetInput();
        PressingKeyCheck();

        if (Input.GetKey(KeyCode.Space))
            _weapon.Shoot(_normalSprite);

        _desiredVelocity = new Vector2(_input.x, _input.y) * Mathf.Max(_maxSpeed, 0f);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        _velocity = _rigidbody.velocity;

        Move();
        CameraMovement();
    }

    public void Dead()
    {
        _rigidbody.simulated = false;
        _animator.SetBool("dead", true);
    }

    [Command]
    public void Respawn()
    {
        _health.Respawn();
        _rigidbody.simulated = true;
        _animator.SetBool("dead", false);
        SetPosition();
        _timeFromDie = 0f;
    }

    [ClientRpc]
    private void SetPosition()
    {
        transform.position = NetworkManager.singleton.GetStartPosition().position;
    }

    public void AddCoins(int count)
    {
        _coins += count;
        CoinsCountChanged?.Invoke(_coins);
    }

    private void CameraMovement()
    {
        var nextPosition = Vector3.Lerp(_camera.transform.position, transform.position + _offset, Time.fixedDeltaTime * _smothing);

        _camera.transform.position = nextPosition;
    }

    private void ChangeSpriteDirection()
    {
        if (_input.x != 0)
        {
            if (_input.x > 0)
            {
                transform.localScale = Vector3.one;
                _normalSprite = true;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                _normalSprite = false;
            }
        }
    }

    private void PressingKeyCheck()
    {
        if (_input.x != 0 || _input.y != 0)
            _isPressingKey = true;
        else
            _isPressingKey = false;
    }

    private void Move()
    {
        if (_isPressingKey)
        {
            if (Mathf.Sign(_input.x) != Mathf.Sign(_velocity.x) || Mathf.Sign(_input.y) != Mathf.Sign(_velocity.y))
                _maxSpeedChange = _turnSpeed * Time.deltaTime;
            else
                _maxSpeedChange = _acceleration * Time.deltaTime;
        }
        else
            _maxSpeedChange = _decceleration * Time.deltaTime;

        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
        _velocity.y = Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, _maxSpeedChange);

        _rigidbody.velocity = _velocity;
        ChangeSpriteDirection();
    }

    private void GetInput()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (IsDiagonalMovement())
        {
            _input.x *= _speedLimiter;
            _input.y *= _speedLimiter;
        }
    }

    private bool IsDiagonalMovement()
    {
        return _input.x != 0 && _input.y != 0;
    }
}