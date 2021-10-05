using Addons.EditorButtons.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

#if UNITY_EDITOR

#endif

namespace TweenSystem
{
    public class Rotator: MonoBehaviour
    {
        public Space space;
        public float multiplier = 1;
        public float speed = 10;
        [FormerlySerializedAs("randomizeOnEnable")] public bool randomizeAxisOnEnable;
        public bool randomDirection;
        public Vector3 vectorUp = Vector3.up;

        private void OnEnable()
        {
            if (randomizeAxisOnEnable)
            {
                RandomizeAxis();
            }
            if (randomDirection)
            {
                speed *= -1;
            }
        }

        private void Update() 
        {
            transform.Rotate(vectorUp, speed * multiplier* Time.deltaTime,space);
        }

        public void RandomizeAxis()
        {
            vectorUp = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f));
        }

        public void RandomizeRotation()
        {
            transform.Rotate(vectorUp, Random.value * 360);
        }

        [Button("Randomize Rotation")]
        public void RandomRotation()
        {
#if UNITY_EDITOR
            RandomizeAxis();
            EditorUtility.SetDirty(this);
#endif  
        }

        public void Pause()
        {
            enabled = false;
        }

        public void Unpause()
        {
            enabled = true;
        }

        private void OnDrawGizmos()
        {
            Vector3 up = vectorUp * 0.1f;
            if (space == Space.Self) up = transform.rotation * up;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + up, transform.position - up);
        }
    }
}
