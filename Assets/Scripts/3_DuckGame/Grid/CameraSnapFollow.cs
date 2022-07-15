using System;
using UnityEngine;

public class CameraSnapFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 mask = Vector3.one;
    [SerializeField] private int gridSize;
    [SerializeField] private Transform cameraFollow;

    private bool _initialized;
    private Vector3 _iniPos;

    public void OnValidate(){
        InitializeValues();
    }

    private void Start(){
        if (!_initialized){
            InitializeValues();
            _initialized = true;
        }
    }

    public void SetGridSize(int size){
        gridSize = size;
        InitializeValues();
    }

    private void InitializeValues(){
        _iniPos = transform.position;
        if (TryGetComponent(out Camera cam) && cam.orthographic){
            cam.aspect = (float)gridSize / gridSize;
            cam.orthographicSize = (float)gridSize / 2;
        }
    }

    private void LateUpdate(){
        Vector3 offset = target.position - _iniPos;
        if (gridSize > 0)
            offset.x = Mathf.Round(offset.x / gridSize) * gridSize * mask.x;
        if (gridSize > 0)
            offset.y = Mathf.Round(offset.y / gridSize) * gridSize * mask.y;
        if (gridSize > 0)
            offset.z = Mathf.Round(offset.z / gridSize) * gridSize * mask.z;
        transform.position = _iniPos + offset;
    }
}

