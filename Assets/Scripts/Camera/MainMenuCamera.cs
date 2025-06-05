using UnityEngine;
using System.Collections.Generic;
public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private float moveForce = 1f;
    [SerializeField] private List<GameObject> waypoints;
    private int pointsIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[pointsIndex].transform.position, moveForce * Time.deltaTime);

        if (transform.position == waypoints[pointsIndex].transform.position)
        {
            //Next point of the array of Locations
            pointsIndex++;
        }

        if (pointsIndex == (waypoints.Count))
        {
            //Going Back to the start point
            pointsIndex = 0;
        }
    }
}
