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
    private bool isPickedUp = false;
    private Transform target;
    private Vector3 p0;
    private Vector3 p1;
    private float timer;
    private float duration;
    private Coroutine coPickedUp;

    private void OnEnable()
    {
        basePosition = transform.position;
        floatPhase = Random.Range(0f, Mathf.PI * 2f);
        coPickedUp = null;
    }

    private void Update()
    {
        if (!isPickedUp)
        {
            float y = Mathf.Sin(Time.time * floatFrequency + floatPhase) * floatAmplitude;
            transform.position = basePosition + Vector3.up * y;
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            timer += Time.deltaTime / duration;

            float eased = Mathf.Pow(timer, 3f);

            Vector3 p3 = target.position;
            Vector3 dirToItem = (p0 - p3).normalized;
            Vector3 p2 = p3 + dirToItem * 2f;

            transform.position = CubicBezier(p0, p1, p2, p3, eased);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"아이템 획득: {gameObject.name}");
        //Destroy(gameObject);
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
        Destroy(gameObject); // Despawn으로 가야함
    }

    public void StartLooting(Transform player)
    {
        if (isPickedUp) return;
        isPickedUp = true;
        target = player;
        timer = 0f;
        p0 = transform.position;

        Vector3 dirAway = (p0 - player.position).normalized;
        p1 = p0 + dirAway * pullbackDistance;
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
