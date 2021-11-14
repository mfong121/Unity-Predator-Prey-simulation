using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField]
    public float viewRadius = 4;
    [SerializeField]
    [Range(0,360)]
    public float viewAngle = 60;
    [SerializeField]
    [Range(1,45)]
    public float viewLineDensity = 5; //Density of FOV visualization lines (In degrees)

    [SerializeField]
    LayerMask opponent; //Predators & prey are opponents of eachother
    [SerializeField]
    LayerMask walls;

    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobalAngle)
    {
        if (!isGlobalAngle) //Converts the input angle to correspond with the current object instead of being static
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); //sin and cos are reversed because unity starts angles at the top of a circle instead of from the right
    }

    void opponentsInVision()
    {
        Collider[] opponentsNearby = Physics.OverlapSphere(transform.position,viewRadius,opponent); // get all opponents in view radius
        //TODO: exclude opponents blocked by obstacles
    }

}
