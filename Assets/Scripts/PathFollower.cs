using UnityEngine;
using System.Collections;

public class PathFollower : Seek
{
    public AIPath path;
    public float pathOffset = 0.0f;
    float currentParam;

    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
        currentParam = 0f;
    }

    public override Steering GetSteering()
    {
        currentParam = path.GetParam(transform.position, currentParam);
        Debug.Log("From PathFollower, currentParam is: " + currentParam);
        float targetParam = currentParam + pathOffset;
        Debug.Log("From PathFollower, targetParam is: " + targetParam);
        target.transform.position = path.GetPosition(targetParam);
        // TODO
        // Change the approach in order to solve
//        Vector3 targetParam = currentParam + pathOffset;
//        target.transform.position = path.GetPosition(currentParam);
        return base.GetSteering();
    }
}
