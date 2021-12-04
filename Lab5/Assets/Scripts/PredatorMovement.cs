
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FongMichael.Lab5
{
    [RequireComponent(typeof(FieldOfView))]
    public class PredatorMovement : MonoBehaviour
    {
        [SerializeField]
        float moveSpeed = 4;
        [SerializeField]
        [Range(0.001f, 0.05f)]
        float turnSpeed = 0.012f; //rotation in radians

        FieldOfView fov;

        Vector3 rotationIncrement;
        turnDirection direction;
        Vector3 turnVector;

        private enum turnDirection
        {
            LEFT, RIGHT, NONE, HUNT
        }

        // Start is called before the first frame update
        void Start()
        {
            fov = GetComponent<FieldOfView>();
            direction = turnDirection.NONE;
            StartCoroutine("movementLogic", .2f);
        }

        // Update is called once per frame
        void Update()
        {
            switch (direction)
            {
                case turnDirection.HUNT:
                    break;
                case turnDirection.LEFT:
                    turnVector = -transform.right;
                    break;
                case turnDirection.RIGHT:
                    turnVector = transform.right;
                    break;
                case turnDirection.NONE:
                    turnVector = transform.forward;
                    break;
                default:
                    break;
            }

            transform.position += transform.forward * moveSpeed * Time.deltaTime; //moves forward
            rotationIncrement = Vector3.RotateTowards(transform.forward, turnVector, turnSpeed, 0);
            transform.rotation = Quaternion.LookRotation(rotationIncrement);
        }

        IEnumerator movementLogic(float delay)
        {
            while (true)
            {
                reactToNothing();
                reactToWall();
                reactToOpponents();

                yield return new WaitForSeconds(delay);
            }
        }

        void reactToNothing()
        {
            //Standard movement
            var rand = Random.Range(0, 10);
            if (rand > 1)
            {
                //go straight
                direction = turnDirection.NONE;
            }
            else
            {
                turnRandom();
            }
        }
        void reactToOpponents()
        {
            /*            Debug.Log("Visible Opponents: " + fov.visibleOpponents.Count);*/
            if (fov.visibleOpponents.Count > 0)
            {
/*                Debug.Log("HUNT!");*/
                try
                {
                    turnVector = fov.getOpponentPosition() - transform.position;
                    direction = turnDirection.HUNT;
                }
                catch
                {

                }

            }
        }
        void reactToWall()
        {
            wallDirection currentWalls = fov.wallsNearby();
            /*            Debug.Log("CurrentWalls: " + currentWalls.ToString());*/
            switch (currentWalls)
            {

                /*NONE = 0,
                  LEFT = 1,
                  RIGHT = 2,
                  FRONT = 4*/

                case wallDirection.NONE:
                    break;
                case wallDirection.RIGHT:
                case (wallDirection)6: //RIGHT and FRONT
                    turnLeft();
                    break;
                case wallDirection.LEFT:
                case (wallDirection)5: //LEFT and FRONT
                    turnRight();
                    break;
                default: //FRONT, LEFT and RIGHT, FRONT and LEFT and RIGHT
                    turnRandom();
                    /*turnRight();*/
                    break;
            }
        }
        void turnRight()
        {
            direction = turnDirection.RIGHT;
        }

        void turnLeft()
        {
            direction = turnDirection.LEFT;
        }

        void turnRandom()
        {
            var rand = Random.Range(0, 2);
            if (rand == 0)
            {
                turnLeft();
            }
            else
            {
                turnRight();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //could play a sound file here lol
        }
    }
}