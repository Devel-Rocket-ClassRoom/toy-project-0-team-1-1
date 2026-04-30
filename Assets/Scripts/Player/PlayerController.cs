using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 9f;

    private bool IsMoving;
    private Vector3 _velocity;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        IsMoving = inputDir.magnitude > 0.01f ? true : false;
        animator.SetBool("Run", IsMoving);
        _velocity = inputDir * speed;
        if (_velocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(_velocity),
                15f * Time.deltaTime
            );
        }
        transform.position += _velocity * Time.deltaTime;
    }

}