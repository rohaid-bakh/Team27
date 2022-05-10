using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;

    public static CameraShake Instance
    {
        get
        {
            if (_instance == null)
            {
                var instances = FindObjectsOfType<CameraShake>();
                if (instances.Length > 1) Debug.LogError("Multiple Instances of CameraShake in scene");
                else if (instances.Length == 0) Debug.LogError("No instances of CameraShake in scene");
                else _instance = instances[0];
            }

            return _instance;
        }
    }

    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.5f;

    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void Play()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            transform.position = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = initialPosition;
    }

}