using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 0.3f;
    public float ShakeAmplitude = 1.2f;
    public float ShakeFrequency = 2.0f;

    private float ShakeElapsedTime = 0f;

    public CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camNoise;

    void Start()
    {
        if(cam != null)
        {
            camNoise = cam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if(cam != null || camNoise != null)
        {
            if(ShakeElapsedTime > 0)
            {
                camNoise.m_AmplitudeGain = ShakeAmplitude;
                camNoise.m_FrequencyGain = ShakeFrequency;


                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                camNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }


    }

    public void Shake()
    {
        ShakeElapsedTime = ShakeDuration;
    }
}
