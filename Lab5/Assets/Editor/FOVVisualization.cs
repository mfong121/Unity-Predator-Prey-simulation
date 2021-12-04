using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace FongMichael.Lab5
{
    [CustomEditor(typeof(FieldOfView))]
    public class FOVVisualization : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            Handles.color = Color.white;

            Vector3 leftVisionEdge = fov.DirectionFromAngle(-fov.viewAngle / 2, false);
            Vector3 rightVisionEdge = fov.DirectionFromAngle(fov.viewAngle / 2, false);

            /*Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);*/

            //Draw the edge of FOV
            Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward, -fov.viewAngle / 2, fov.viewRadius);
            Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward, fov.viewAngle / 2, fov.viewRadius);

            Handles.DrawLine(fov.transform.position, fov.transform.position + leftVisionEdge * fov.viewRadius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + rightVisionEdge * fov.viewRadius);

            //Draw wall detection lines
            Handles.DrawLine(fov.transform.position, fov.transform.position + fov.DirectionFromAngle(-50, false) * fov.wallCheckDistance);
            Handles.DrawLine(fov.transform.position, fov.transform.position + fov.DirectionFromAngle(50, false) * fov.wallCheckDistance);
            Handles.DrawLine(fov.transform.position, fov.transform.position + fov.transform.forward * fov.wallCheckDistance);


            //Draw the FOV representation on selected agent in unity editor
            /*if (fov.viewLineDensity != 0)
            {
                for (float i = fov.viewAngle / 2; i > 0; i -= fov.viewLineDensity)
                {
                    Vector3 rightArcPos = fov.DirectionFromAngle(i, false); //current arc position on right half of FOV
                    Vector3 leftArcPos = fov.DirectionFromAngle(-i, false); //current arc position on left half of FOV
                    Handles.DrawLine(fov.transform.position, fov.transform.position + rightArcPos * fov.viewRadius);
                    Handles.DrawLine(fov.transform.position, fov.transform.position + leftArcPos * fov.viewRadius);
                }
            }*/

            //draws a line from the selected agent to visible opponents
            foreach (Transform visibleOpponent in fov.visibleOpponents)
            {
                Handles.DrawLine(fov.transform.position, visibleOpponent.transform.position);
            }

        }

    }
}