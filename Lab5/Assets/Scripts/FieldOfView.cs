using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FongMichael.Lab5
{
    [System.Flags]
    public enum wallDirection
    {
        NONE = 0,
        LEFT = 1,
        RIGHT = 2,
        FRONT = 4
    }
    public class FieldOfView : MonoBehaviour
    {

        [SerializeField]
        public float viewRadius = 4;
        [SerializeField]
        [Range(0, 360)]
        public float viewAngle = 60;
        [SerializeField]
        [Range(1, 45)]
        public float viewLineDensity = 45; //Density of FOV visualization lines (In degrees)

        [SerializeField]
        LayerMask opponent; //Predators & prey are opponents of eachother
        [SerializeField]
        LayerMask allies;
        [SerializeField]
        LayerMask walls;

        public float wallCheckDistance = 1.2f;//maybe should be longer for hunters

        public List<Transform> visibleOpponents;

        private void Start()
        {
            StartCoroutine("UpdateVisibleOpponents", .1f);
        }

        IEnumerator UpdateVisibleOpponents(float delay)
        {
            while (true)
            {
                visibleOpponents = opponentsInVision();
                yield return new WaitForSeconds(delay); //Delays repeating the function 
            }
        }

        public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobalAngle)
        {
            if (!isGlobalAngle) //Converts the input angle to correspond with the current object instead of being static
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); //sin and cos are reversed because unity starts angles at the top of a circle instead of from the right
        }

        List<Transform> opponentsInVision()
        {
            Collider[] opponentsNearby = Physics.OverlapSphere(transform.position, viewRadius, opponent); // get all opponents in view radius
            List<Transform> currentVisibleOpponents = new List<Transform>();

            for (int i = 0; i < opponentsNearby.Length; i++)
            {
                Transform currentOpponent = opponentsNearby[i].transform;
                Vector3 directionToCurrentOpponent = (currentOpponent.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToCurrentOpponent) < viewAngle / 2) // check if current opponent is within FOV
                {


                    float distanceToCurrentOpponent = Vector3.Distance(transform.position, currentOpponent.position);

                    if (!Physics.Raycast(transform.position, directionToCurrentOpponent, distanceToCurrentOpponent, walls)) //No obstacles in the way (Physics.raycast returns true if the ray intersects with a collider)
                    {
                        currentVisibleOpponents.Add(currentOpponent);
                    }
                }
            }
            return currentVisibleOpponents;
        }

        public Vector3 getOpponentPosition()
        {
            //there has to be an enemy in vision

            Collider[] opponentsNearby = Physics.OverlapSphere(transform.position, viewRadius, opponent); // get all opponents in view radius
            Vector3 closestPosition = opponentsNearby[0].transform.position;
            /*if (opponentsNearby.Length > 0)
            {
                Transform closest = opponentsNearby[0].transform;


                if (opponentsNearby.Length > 1)
                {
                    for (int i = 1; i < opponentsNearby.Length; i++)
                    {
                        float distanceToClosest = Vector3.Distance(transform.position, closest.position);
                        float distanceToCurrent = Vector3.Distance(transform.position, opponentsNearby[i].transform.position);
                        if (distanceToClosest > distanceToCurrent)
                        {
                            closest = opponentsNearby[i].transform;
                            closestPosition = closest.position;
                        }
                    }
                }
            }*/
            return closestPosition;
        }

        public wallDirection wallsNearby()
        {
            Collider[] wallsNearby = Physics.OverlapSphere(transform.position, viewRadius, walls);
            wallDirection wallDetection = new wallDirection();
            wallDetection = 0;
            //Detect if a wall is too close

            Vector3 leftWallCheck = DirectionFromAngle(-50, false);
            Vector3 rightWallCheck = DirectionFromAngle(50, false);

/*            Debug.DrawLine(transform.position, -1.5f*transform.right);*/
            if (Physics.Raycast(transform.position, leftWallCheck, wallCheckDistance, walls))
            {
                //wall on left side
                wallDetection |= wallDirection.LEFT;
            }



/*            Debug.DrawLine(transform.position, 1.5f * transform.right);*/
            if (Physics.Raycast(transform.position, rightWallCheck, wallCheckDistance, walls))
            {
                //wall on right side
                wallDetection |= wallDirection.RIGHT;
            }
            if (Physics.Raycast(transform.position, transform.forward, wallCheckDistance, walls))
            {
                //wall in front
                wallDetection |= wallDirection.FRONT;
            }
            return wallDetection;
        }

        private void OnDrawGizmos() //Shows FOV lines in Scene view
        {
            if (transform.gameObject.tag == "Prey")
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            for (float i = viewAngle / 2; i > 0; i -= viewLineDensity)
            {
                Vector3 rightArcPos = DirectionFromAngle(i, false); //current arc position on right half of FOV
                Vector3 leftArcPos = DirectionFromAngle(-i, false); //current arc position on left half of FOV
                Gizmos.DrawLine(transform.position, transform.position + rightArcPos * viewRadius);
                Gizmos.DrawLine(transform.position, transform.position + leftArcPos * viewRadius);
            }

        }

        /*public void drawFOV(Color color)
        {
            for(float i = viewAngle / 2; i > 0; i -= viewLineDensity)
            {
                Vector3 rightArcPos = DirectionFromAngle(i, false); //current arc position on right half of FOV
                GameObject currentLine = new GameObject();
                currentLine.transform.SetParent(this.transform);
                currentLine.transform.position = new Vector3(); //sets a new blank vector??
                currentLine.AddComponent<LineRenderer>();
                LineRenderer lineRenderer = currentLine.GetComponent<LineRenderer>();

                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
*//*                lineRenderer.SetWidth(0.1f, 0.15f);
*//*
                lineRenderer.SetPosition(0, rightArcPos * viewRadius);
            }

            for (float i = viewAngle / 2; i > 0; i -= viewLineDensity)
            {
                Vector3 leftArcPos = DirectionFromAngle(-i, false); //current arc position on left half of FOV
                GameObject currentLine = new GameObject();
                currentLine.transform.SetParent(this.transform);
                currentLine.transform.position = new Vector3(0,1,0); //sets a new blank vector??
                currentLine.AddComponent<LineRenderer>();
                LineRenderer lineRenderer = currentLine.GetComponent<LineRenderer>();
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
*//*                lineRenderer.SetWidth(0.1f, 0.15f);*//*
                lineRenderer.SetPosition(0, leftArcPos * viewRadius);
            }
        }*/
    }
}