using UnityEngine;

namespace Utils
{
    public class VelocityInfo : MonoBehaviour
    {
        readonly Vector3[] positions = new Vector3[frameDataLength];
        readonly Quaternion[] rotations = new Quaternion[frameDataLength];
        const int frameDataLength = 5;

        void Start()
        {
            ResetFrameData();
        }

        void FixedUpdate()
        {
            UpdateFrameData();
        }

        void ResetFrameData()
        {
            for (int a = 0; a < frameDataLength; a++)
            {
                positions[a] = transform.position;
                rotations[a] = transform.rotation;
            }
        }

        void UpdateFrameData()
        {
            for (int a = 0; a < frameDataLength - 1; a++)
            {
                positions[a] = positions[a + 1];
                rotations[a] = rotations[a + 1];
            }
            positions[frameDataLength - 1] = transform.position;
            rotations[frameDataLength - 1] = transform.rotation;
        }

        public Vector3 GetAverageVelocity()
        {
            return (positions[frameDataLength - 1] - positions[0]) / (frameDataLength * Time.fixedDeltaTime);
        }

        public Vector3 GetAverageAngularVelocity()
        {
            var delta = (Quaternion.Inverse(rotations[0]) * rotations[frameDataLength - 1]).eulerAngles;
            var normalDelta = new Vector3(Mathf.DeltaAngle(0, delta.x),
                                          Mathf.DeltaAngle(0, delta.y),
                                          Mathf.DeltaAngle(0, delta.z));
            return normalDelta / (frameDataLength * Time.fixedDeltaTime);
        }
    }
}