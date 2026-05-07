using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus _status;
    private bool _isMoving;
    private bool _isDead;
    private Vector3 _velocity;
    private Animator _animator;
    private CharacterController _cc;
    private float _verticalVelocity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _status = GetComponent<PlayerStatus>();
        _cc = GetComponent<CharacterController>();
        _isDead = false;
        var playerStatus = GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.OnDead += HandleDeath;
        }
    }

    private void Update()
    {
        if (_isDead) return;
        if (!_cc.isGrounded)
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity = -2f; // 바닥에 붙어있게
        }
        HandleMove();
    }

    private void HandleDeath()
    {
        _isDead = true;
        _velocity = Vector3.zero;
        _verticalVelocity = 0f; // 이것도 초기화
        _animator.SetBool("Run", false);

        _cc.enabled = false;
    }
    private void HandleMove()
    {
        if (_isDead) return;
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var inputDir = new Vector3(h, 0f, v).normalized;
        _isMoving = inputDir.magnitude > 0.01f ? true : false;
        _animator.SetBool("Run", _isMoving);
        _velocity = inputDir * _status.Speed;
        if (_velocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(_velocity),
                15f * Time.deltaTime
            );
        }
        _velocity.y = _verticalVelocity;
        _cc.Move(_velocity * Time.deltaTime); // 죽을 때 여기서 에러남
    }
}