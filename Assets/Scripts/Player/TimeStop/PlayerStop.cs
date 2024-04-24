using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

public class PlayerStop : MonoBehaviour
{
    [SerializeField] private TimeManager _timemanager;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lerpVolumeWeightSpeed;
    [SerializeField] private KeyCode _timeInvertKey;
    [SerializeField] private Volume _volume;

    [SerializeField] private GrayscaleLayers Grayscale;

    private float _targetStopVolumeWeight;

    private void Update()
    {
        _volume.weight = Mathf.Lerp(_volume.weight, _targetStopVolumeWeight, _lerpVolumeWeightSpeed * Time.deltaTime);

        if (_timemanager.TimeIsStopped)
            _rigidbody.velocity *= 0.3f;

        if (Input.GetKeyDown(_timeInvertKey))
        {
            Debug.Log("Time Stopped");
            _timemanager.StopTime();
            _targetStopVolumeWeight = 1;
            Grayscale.enabled = true;
        }

        if (Input.GetKeyUp(_timeInvertKey) && _timemanager.TimeIsStopped)
        {
            _targetStopVolumeWeight = 0;
            Debug.Log("Time Runned");
            _timemanager.ContinueTime();
            Grayscale.enabled = false;
        }
    }
}