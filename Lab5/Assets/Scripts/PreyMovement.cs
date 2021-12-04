
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FongMichael.Lab5 
{
    [RequireComponent(typeof(FieldOfView))]
    public class PreyMovement : MonoBehaviour
    {
        [SerializeField]
        float moveSpeed = 4;
        [SerializeField]
        [Range(0.001f, 0.05f)]
        float turnSpeed = 0.015f; //rotation in radians

        FieldOfView fov;

        Vector3 rotationIncrement;
        turnDirection direction;
        Vector3 turnVector;

        private enum turnDirection
        {
            LEFT,RIGHT,NONE,FLEE
        }

        // Start is called before the first frame update
        void Start()
        {
            fov = GetComponent<FieldOfView>();
            direction= turnDirection.NONE;
            StartCoroutine("movementLogic", .2f);
            /*fov.drawFOV(Color.green);*/
        }

        // Update is called once per frame
        void Update()
        {
            switch (direction){
                case turnDirection.FLEE:
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
                //randomly move to start off

                reactToNothing();
                reactToOpponents();
                reactToWall();

                yield return new WaitForSeconds(delay);
            }
        }

        void reactToNothing()
        {
            //Standard movement
            var rand = Random.Range(0, 10);
            if (rand > 0)
            {
                //go straight
                direction = turnDirection.NONE;
            }
            else
            {
                turnRandom();
                /*                    Debug.Log("RANDOM TURN");*/
            }
        }
        void reactToOpponents()
        {
/*            Debug.Log("Visible Opponents: " + fov.visibleOpponents.Count);*/
            if (fov.visibleOpponents.Count > 0)
            {
/*                Debug.Log("RUN!!");*/
                try
                {
                    turnVector = transform.position - fov.getOpponentPosition();
                    direction = turnDirection.FLEE;
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
            if (collision.gameObject.tag == "Predator")
            {

                if (Random.Range(0, 2) > 0)
                {
                    //bottom left corner
                    transform.position = new Vector3(-7.5f, 1f, -7.5f);
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    //maybe spawn a new one at spawn point
                }
                else
                {
                    //top right corner
                    transform.position = new Vector3(7.5f, 1f, 7.5f);
                    transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                }
            }
        }
    }
}