using UnityEngine;

namespace UtilsEditor{
#if UNITY_EDITOR
    public static class EditorUiUtils{
        static public void DrawString(string text, Vector3 worldPos, float oX = 0, float oY = 0, Color? colour = null) {
 
            if(!IsSceneViewCameraInRange(worldPos,25)) return;
            UnityEditor.Handles.BeginGUI();
 
            var restoreColor = GUI.color;
 
            if (colour.HasValue) GUI.color = colour.Value;
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
 
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }
 
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.alignment = TextAnchor.MiddleCenter;
            guiStyle.normal.textColor = GUI.color;
            UnityEditor.Handles.Label(TransformByPixel(worldPos, oX, oY), text,guiStyle);
 
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();

        }
 
        static Vector3 TransformByPixel(Vector3 position, float x, float y) {
            return TransformByPixel(position, new Vector3(x, y));
        }
 
        static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy) {
            Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
            if (cam)
                return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);
            return position;
        }
        
        public static bool IsSceneViewCameraInRange(Vector3 position, float distance)
        {
            Vector3 cameraPos = Camera.current.WorldToScreenPoint(position);
            return ((cameraPos.x >= 0) &&
                    (cameraPos.x <= Camera.current.pixelWidth) &&
                    (cameraPos.y >= 0) &&
                    (cameraPos.y <= Camera.current.pixelHeight) &&
                    (cameraPos.z > 0) &&
                    (cameraPos.z < distance));
        }
    }
#endif
}

