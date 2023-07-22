using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float defaultDistance = 6f;
    [SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;
    [SerializeField] [Range(0f, 10f)] private float maximumDistance = 10f;

    [SerializeField] [Range(0f, 10f)] private float smoothing = 4f;
    [SerializeField] [Range(0f, 10f)] private float zoomSensitivity = 1f;

    private float currentTargetDistance;

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineInputProvider inputProvider;
    private int ZAxisIndex = 2;

    private void Awake()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>();
        inputProvider = GetComponent<CinemachineInputProvider>();
    }

    public void Start()
    {
        currentTargetDistance = defaultDistance;
    }

    private void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("IsOnUI");
            return;
        }

        Zoom();
    }

    private void Zoom()
    {
        float zoomValue = inputProvider.GetAxisValue(ZAxisIndex) * zoomSensitivity;
        currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);
        float currentDistance = framingTransposer.m_CameraDistance;
        if (currentDistance == currentTargetDistance)
        {
            return;
        }

        float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}