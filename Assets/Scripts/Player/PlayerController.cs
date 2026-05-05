using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus _status;
    private bool _isMoving;
    private Vector3 _velocity;
    private Animator _animator;
    private CharacterController _cc;
    private float _verticalVelocity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _status = GetComponent<PlayerStatus>();
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
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

    private void HandleMove()
    {
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
        _cc.Move(_velocity * Time.deltaTime);
    }
}