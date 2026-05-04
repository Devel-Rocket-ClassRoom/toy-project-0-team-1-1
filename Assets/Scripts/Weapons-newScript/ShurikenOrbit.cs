using UnityEngine;

public class ShurikenOrbit : MonoBehaviour
{
    private Transform owner;
    private float radius;
    private float rotateSpeed;
    private float damage;
    private LayerMask targetLayer;
    private float angle;

    public void Init(
        Transform owner,
        float radius,
        float rotateSpeed,
        float damage,
        LayerMask targetLayer,
        float startAngle)
    {
        this.owner = owner;
        this.radius = radius;
        this.rotateSpeed = rotateSpeed;
        this.damage = damage;
        this.targetLayer = targetLayer;
        this.angle = startAngle;
    }

    private void Update()
    {
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }

        angle += rotateSpeed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(rad),
            0f,
            Mathf.Sin(rad)
        ) * radius;

        transform.position = owner.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0)
            return;

        Debug.Log($"수리검 타격: {other.name} / 데미지: {damage}");

        // 나중에 몬스터 구현 후
        // other.GetComponent<IDamageable>()?.OnDamage(damage);
    }
}