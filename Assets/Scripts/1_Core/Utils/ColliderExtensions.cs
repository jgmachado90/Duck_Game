using UnityEngine;

namespace Utils
{
    //ClosestPointOnSurface methods found at:
    //https://www.csharpcodi.com/vs2/2360/SuperCharacterController/Assets/SuperCharacterController/SuperCharacterController/Core/SuperCollider.cs/
    public static class ColliderExtensions
    {
        #region Collider 3D

        public static void DrawColliderBounds(this BoxCollider boxCollider, Color color = default)
        {
            var transform = boxCollider.transform;
            var center = transform.TransformPoint(boxCollider.center);
            var scaledSize = Vector3.Scale(transform.lossyScale, boxCollider.size);
            
            var halfX = transform.right * (scaledSize.x * 0.5f);
            var halfY = transform.up * (scaledSize.y * 0.5f);
            var halfZ = transform.forward * (scaledSize.z * 0.5f);

            var v0 = center - halfX + halfZ;
            var v1 = center + halfX + halfZ;
            var v2 = center + halfX - halfZ;
            var v3 = center - halfX - halfZ;

            ConnectRespective(
                DrawPlane(halfY),
                DrawPlane(-halfY)
            );

            Vector3[] DrawPlane(Vector3 yOffset)
            {
                var p = new[]
                {
                    v0 + yOffset,
                    v1 + yOffset,
                    v2 + yOffset,
                    v3 + yOffset
                };
                Debug.DrawLine(p[0], p[1], color);
                Debug.DrawLine(p[1], p[2], color);
                Debug.DrawLine(p[2], p[3], color);
                Debug.DrawLine(p[3], p[0], color);
                    
                return p;
            }

            void ConnectRespective(Vector3[] planeA, Vector3[] planeB, int arrayLength = 4)
            {
                for (var i = 0; i < arrayLength; i++)
                {
                    Debug.DrawLine(planeA[i], planeB[i], color);
                }
            }
        }
        
        public static bool ClosestPointOnSurface(this Collider collider, Vector3 to, out Vector3 closestPointOnSurface)
        {
            switch (collider)
            {
                case BoxCollider c:
                    closestPointOnSurface = ClosestPointOnSurface(c, to);
                    return true;
                case SphereCollider c:
                    closestPointOnSurface = ClosestPointOnSurface(c, to);
                    return true;
                case CapsuleCollider c:
                    closestPointOnSurface = ClosestPointOnSurface(c, to);
                    return true;
                default:
                    Debug.LogError(
                        $"{collider.GetType()} does not have an implementation for ClosestPointOnSurface; " +
                        $"GameObject.Name='{collider.gameObject.name}'");
                    closestPointOnSurface = Vector3.zero;
                    return false;
            }
        }

        public static Vector3 ClosestPointOnSurface(this SphereCollider collider, Vector3 to)
        {
            Vector3 p;
            Vector3 origin = collider.transform.position + collider.center;

            p = to - origin;
            p.Normalize();

            p *= collider.radius * collider.transform.localScale.x;
            p += origin;

            return p;
        }

        public static Vector3 ClosestPointOnSurface(this BoxCollider collider, Vector3 to)
        {
            // Cache the collider transform
            var ct = collider.transform;

            // Firstly, transform the point into the space of the collider
            var local = ct.InverseTransformPoint(to);

            // Now, shift it to be in the center of the box
            local -= collider.center;

            //Pre multiply to save operations.
            var halfSize = collider.size * 0.5f;

            // Clamp the points to the collider's extents
            var localNorm = new Vector3(
                Mathf.Clamp(local.x, -halfSize.x, halfSize.x),
                Mathf.Clamp(local.y, -halfSize.y, halfSize.y),
                Mathf.Clamp(local.z, -halfSize.z, halfSize.z)
            );

            //Calculate distances from each edge
            var dx = Mathf.Min(Mathf.Abs(halfSize.x - localNorm.x), Mathf.Abs(-halfSize.x - localNorm.x));
            var dy = Mathf.Min(Mathf.Abs(halfSize.y - localNorm.y), Mathf.Abs(-halfSize.y - localNorm.y));
            var dz = Mathf.Min(Mathf.Abs(halfSize.z - localNorm.z), Mathf.Abs(-halfSize.z - localNorm.z));

            // Select a face to project on
            if (dx < dy && dx < dz)
            {
                localNorm.x = Mathf.Sign(localNorm.x) * halfSize.x;
            }
            else if (dy < dx && dy < dz)
            {
                localNorm.y = Mathf.Sign(localNorm.y) * halfSize.y;
            }
            else if (dz < dx && dz < dy)
            {
                localNorm.z = Mathf.Sign(localNorm.z) * halfSize.z;
            }

            // Now we undo our transformations
            localNorm += collider.center;

            // Return resulting point
            return ct.TransformPoint(localNorm);
        }

        public static Vector3 ClosestPointOnSurface(this CapsuleCollider collider, Vector3 to)
        {
            Transform ct = collider.transform; // Transform of the collider

            float lineLength = collider.height - collider.radius * 2; // The length of the line connecting the center of both sphere
            Vector3 dir = Vector3.up;
            Vector3 collCenter = collider.center;

            Vector3 upperSphere = dir * (lineLength * 0.5f) + collCenter; // The position of the radius of the upper sphere in local coordinates
            Vector3 lowerSphere = -dir * (lineLength * 0.5f) + collCenter; // The position of the radius of the lower sphere in local coordinates

            Vector3 local = ct.InverseTransformPoint(to); // The position of the controller in local coordinates

            Vector3 pt = Vector3.zero; // The point we need to use to get a direction vector with the controller to calculate contact point

            if (local.y < lineLength * 0.5f && local.y > -lineLength * 0.5f) // Controller is contacting with cylinder, not spheres
                pt = dir * local.y + collider.center;
            else if (local.y > lineLength * 0.5f) // Controller is contacting with the upper sphere 
                pt = upperSphere;
            else if (local.y < -lineLength * 0.5f) // Controller is contacting with lower sphere
                pt = lowerSphere;

            //Calculate contact point in local coordinates and return it in world coordinates
            Vector3 p = local - pt;
            p.Normalize();
            p = p * collider.radius + pt;
            return ct.TransformPoint(p);
        }

        #endregion

        #region Collider 2D

        public static bool ClosestPointOnSurface(this Collider2D collider, Vector2 to,
            out Vector2 closestPointOnSurface)
        {
            switch (collider)
            {
                case BoxCollider2D c:
                    closestPointOnSurface = ClosestPointOnSurface(c, to);
                    return true;
                case CircleCollider2D c:
                    closestPointOnSurface = ClosestPointOnSurface(c, to);
                    return true;
                default:
                    Debug.LogError(string.Format(
                        "{0} does not have an implementation for ClosestPointOnSurface; GameObject.Name='{1}'",
                        collider.GetType(), collider.gameObject.name));
                    closestPointOnSurface = Vector2.zero;
                    return false;
            }
        }

        public static Vector2 ClosestPointOnSurface(this CircleCollider2D collider, Vector2 to)
        {
            Vector2 p;
            Vector2 origin = (Vector2) collider.transform.position + collider.offset;

            p = to - origin;
            p.Normalize();

            p *= collider.radius * collider.transform.localScale.x;
            p += origin;

            return p;
        }

        public static Vector2 ClosestPointOnSurface(this BoxCollider2D collider, Vector2 to)
        {
            // Cache the collider transform
            var ct = collider.transform;

            // Firstly, transform the point into the space of the collider
            var local = (Vector2) ct.InverseTransformPoint(to);

            // Now, shift it to be in the center of the box
            local -= collider.offset;

            //Pre multiply to save operations.
            var halfSize = collider.size * 0.5f;

            // Clamp the points to the collider's extents
            var localNorm = new Vector2(
                Mathf.Clamp(local.x, -halfSize.x, halfSize.x),
                Mathf.Clamp(local.y, -halfSize.y, halfSize.y)
            );

            //Calculate distances from each edge
            var dx = Mathf.Min(Mathf.Abs(halfSize.x - localNorm.x), Mathf.Abs(-halfSize.x - localNorm.x));
            var dy = Mathf.Min(Mathf.Abs(halfSize.y - localNorm.y), Mathf.Abs(-halfSize.y - localNorm.y));

            // Select a face to project on
            if (dx < dy)
            {
                localNorm.x = Mathf.Sign(localNorm.x) * halfSize.x;
            }
            else if (dy < dx)
            {
                localNorm.y = Mathf.Sign(localNorm.y) * halfSize.y;
            }

            // Now we undo our transformations
            localNorm += collider.offset;

            // Return resulting point
            return ct.TransformPoint(localNorm);
        }

        #endregion
    }
}