using UnityEngine;

public class CameraShake : MonoBehaviour 
{
    public float shakeSize;
    private Vector3 _lastPosition;
    private Vector3 _lastRotation;
    public Vector3 MaximumAngularShake;
    public Vector3 MaximumTranslationShake;

    private void Update()
    {
        float shake = Mathf.Pow(shakeSize, 1);
        if(shake > 0)
        {
            var previousRotation = _lastRotation;
            var previousPosition = _lastPosition;
            _lastPosition = new Vector3(
                MaximumTranslationShake.x * (Mathf.PerlinNoise(0, Time.time * 25) * 2 - 1),
                MaximumTranslationShake.y * (Mathf.PerlinNoise(1, Time.time * 25) * 2 - 1),
                MaximumTranslationShake.z * (Mathf.PerlinNoise(2, Time.time * 25) * 2 - 1)
            ) * shake;

            _lastRotation = new Vector3(
                MaximumAngularShake.x * (Mathf.PerlinNoise(3, Time.time * 25) * 2 - 1),
                MaximumAngularShake.y * (Mathf.PerlinNoise(4, Time.time * 25) * 2 - 1),
                MaximumAngularShake.z * (Mathf.PerlinNoise(5, Time.time * 25) * 2 - 1)
            ) * shake;

            transform.localPosition += _lastPosition - previousPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + _lastRotation - previousRotation);
            shakeSize = Mathf.Clamp01(shakeSize - Time.deltaTime);
        }
        else
        {
            if (_lastPosition == Vector3.zero && _lastRotation == Vector3.zero) return;
            /* Clear the transform of any left over translation and rotations */
            transform.localPosition -= _lastPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - _lastRotation);
            _lastPosition = Vector3.zero;
            _lastRotation = Vector3.zero;
        }
    }

  
}