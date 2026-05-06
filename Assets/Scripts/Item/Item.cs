using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public abstract class Item : MonoBehaviour, ILootable
{
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatFrequency = 2f;  
    [SerializeField] private float rotateSpeed = 60f; 
    [SerializeField] private float pullbackDistance = 1.5f;
    [SerializeField] private float pullbackDuration = 0.25f;
    [SerializeField] private float rushSpeed = 15f;
    [SerializeField] private float rushAcceleration = 30f;
    [SerializeField] protected ItemData itemData;

    private float floatPhase;
    private Vector3 basePosition;
    private Transform target;
    private Vector3 p0;
    private Vector3 p1;
    private float timer;
    private Coroutine coPickedUp;

    private void OnEnable()
    {
        floatPhase = Random.Range(0f, Mathf.PI * 2f);
        coPickedUp = null;
    }
    public void Init(Vector3 spawnPosition)
    {
        basePosition = spawnPosition;
    }
    private enum LootPhase { Idle, Pullback, Rush }
    private LootPhase phase = LootPhase.Idle;
    private float currentSpeed;

    private void Update()
    {
        if (phase == LootPhase.Idle)
        {
            float y = Mathf.Sin(Time.time * floatFrequency + floatPhase) * floatAmplitude;
            transform.position = basePosition + Vector3.up * y;
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
        else if (phase == LootPhase.Pullback)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / pullbackDuration);
            float eased = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(p0, p1, eased);

            if (t >= 1f)
            {
                phase = LootPhase.Rush;
                currentSpeed = rushSpeed;
            }
        }
        else if (phase == LootPhase.Rush)
        {
            currentSpeed += rushAcceleration * Time.deltaTime;
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * currentSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        coPickedUp = StartCoroutine(CoPickedUp());
    }
    private IEnumerator CoPickedUp()
    {
        float pickedUpDuration = 0.5f;
        float pickedUpTimer = 0f;

        while (pickedUpTimer < pickedUpDuration)
        {
            pickedUpTimer += Time.deltaTime;
            float t = pickedUpTimer / pickedUpDuration;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
        }
        coPickedUp = null;
        GetEffect(target);
        // Destroy(gameObject); // Despawn으로 가야함
        PoolManager.Instance.Despawn(itemData.prefab, gameObject);
    }

    public void StartLooting(Transform player)
    {
        if (phase != LootPhase.Idle) return;
        target = player;
        timer = 0f;
        p0 = transform.position;

        Vector3 dirAway = (p0 - player.position).normalized;
        p1 = p0 + dirAway * pullbackDistance;

        phase = LootPhase.Pullback;
    }

    private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        return u * u * u * p0
             + 3f * u * u * t * p1
             + 3f * u * t * t * p2
             + t * t * t * p3;
    }

    public abstract void GetEffect(Transform player);
}
