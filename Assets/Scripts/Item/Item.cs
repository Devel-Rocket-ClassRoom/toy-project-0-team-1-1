using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour, IConsumable
{
    [Header("Idle Float")]
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatFrequency = 2f;  
    [SerializeField] private float rotateSpeed = 60f; 
 
    [Header("Pickup (Cubic Bezier)")]
    [SerializeField] private float pickupDuration = 0.6f;
    [SerializeField] private float backDistance = 0.8f;
    [SerializeField] private float arcHeight = 2.0f;
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private Vector3 basePosition;
    private float floatPhase;
    private bool isPickedUp;
    private Coroutine pickupCo;
 
    public abstract void GetItem(GameObject target);
    private void Awake()
    {
        pickupCo = null;
        basePosition = transform.position;
        floatPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        if (isPickedUp) return;
 
        float y = Mathf.Sin(Time.time * floatFrequency + floatPhase) * floatAmplitude;
        transform.position = basePosition + Vector3.up * y;
 
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    public void PickUp(GameObject target)
    {
        if (pickupCo != null) StopCoroutine(pickupCo);
        pickupCo = StartCoroutine(PickupRoutine(target));
    }

    private IEnumerator PickupRoutine(GameObject target)
    {
        Vector3 p0 = transform.position;

        Vector3 backDir = p0 - target.transform.position;
        backDir.y = 0f;
        backDir = backDir.sqrMagnitude > 0.0001f ? backDir.normalized : -transform.forward;

        Vector3 p1 = p0 + backDir * backDistance + Vector3.up * (arcHeight * 0.4f);

        float elapsed = 0f;
        // Vector3 startScale = transform.localScale;
        while (elapsed < pickupDuration)
        {
            elapsed += Time.deltaTime;
            float t  = Mathf.Clamp01(elapsed / pickupDuration);
            float ct = speedCurve.Evaluate(t);

            Vector3 p3 = target.transform.position;
            Vector3 p2 = Vector3.Lerp(p0, p3, 0.7f) + Vector3.up * arcHeight;

            transform.position = CubicBezier(p0, p1, p2, p3, ct);
            // transform.localScale = startScale * Mathf.Lerp(1f, 0.6f, t);
            yield return null;
        }

        transform.position = target.transform.position;
        GetItem(target);
    }

    private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        return (u * u * u) * p0 + 3f * (u * u) * t * p1 + 3f * u * (t * t) * p2 + (t * t * t) * p3;
    }
}
