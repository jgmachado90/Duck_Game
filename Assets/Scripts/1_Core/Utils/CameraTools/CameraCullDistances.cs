using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraCullDistances : MonoBehaviour
    {
        //[Header("Far must be => Camera default Far Clip Plane")]
        public List<LayerCullDistance> layers = new List<LayerCullDistance>();
    
        [Serializable]
        public class LayerCullDistance
        {
            [Layer] public int layer;
            public float far;
        
            public LayerCullDistance(int numLayer, float distance)
            {
                layer = numLayer;
                far = distance;

            }
        }

        private void OnValidate() => SetDistances();

        private void Start() => SetDistances();

        public void SetDistances()
        {
            Camera camera = GetComponent<Camera>();
            float[] distances = new float[32];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = 0;
            }
        
            for (var index = 0; index < layers.Count; index++)
            {
                var layerCullDistance = layers[index];
                //Debug.Log($"{layerCullDistance.layer} {layerCullDistance.far}");
                distances[layerCullDistance.layer] = layerCullDistance.far;
            }

            camera.layerCullDistances = distances;
        }
    }
}
