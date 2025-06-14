using Unity.Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineCamera cmCamera;
    private float startingIntensity = 1f;
    private float shakeTimer = 0.1f;
    private float shakeTimerTotal = 0.1f;


    private void Awake()
    {
        Instance = this;
        cmCamera = GetComponent<CinemachineCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmPerlin =
        cmCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        cmPerlin.AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cmPerlin =
                cmCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

            cmPerlin.AmplitudeGain =
                Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }
}
