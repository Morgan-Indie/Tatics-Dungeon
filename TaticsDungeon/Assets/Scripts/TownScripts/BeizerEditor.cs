using UnityEngine;
using UnityEditor;

namespace PrototypeGame
{
    [CustomEditor(typeof(BezierHandler))]
    public class BeizerEditor : Editor
    {
        private static BezierHandler bezier;

        private static void OnSceneViewGUI(SceneView sceneView)
        {
            Handles.DrawBezier(
                    bezier.points[0].position, 
                    bezier.points[3].position, 
                    bezier.points[1].position, 
                    bezier.points[2].position,
                    Color.red,
                    null,
                    5f
                );
            //Line from start to end
            Handles.color = Color.green;
            Handles.DrawLine(
                    bezier.points[0].position,
                    bezier.points[3].position
                );
            //Line for startControl
            Handles.color = Color.magenta;
            Handles.DrawLine(
                    bezier.points[0].position,
                    bezier.points[1].position
                );
            //Line for endControl
            Handles.DrawLine(
                    bezier.points[2].position,
                    bezier.points[3].position
                );
        }

        private void OnEnable()
        {
            bezier = target as BezierHandler;
            if (!bezier.persist)
            {
                SceneView.duringSceneGui += OnSceneViewGUI;
            }
        }

        void OnDisable()
        {
            if (!bezier.persist)
                SceneView.duringSceneGui -= OnSceneViewGUI;
        }
    }
}