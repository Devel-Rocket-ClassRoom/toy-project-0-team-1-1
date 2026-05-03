using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus _status;
    private bool _isMoving;
    private Vector3 _velocity;
    private Animator _animator;
    private CharacterController _cc;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _status = GetComponent<PlayerStatus>();
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
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
        _cc.Move(_velocity * Time.deltaTime);
    }
}