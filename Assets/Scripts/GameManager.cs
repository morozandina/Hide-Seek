using System;
using CameraControl;
using Trails;
using UnityEngine;

[Serializable]
public class CameraSettings
{
    public Camera camera;
    public ShakeTransform shakeTransform;
    public ShakeTransformEventData data;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action shakeCamera;
    [Space(5)][Header("CAMERA CONTROL :")]
    public CameraSettings cameraSettings;

    private void Awake()
    {
        instance = this;
        shakeCamera += () => cameraSettings.shakeTransform.AddShakeEvent(cameraSettings.data);
    }

    private void OnDestroy()
    {
        shakeCamera = null;
    }

    public void ChangeCamera(LayerMask mask)
    {
        cameraSettings.camera.cullingMask = mask;
    }
}
