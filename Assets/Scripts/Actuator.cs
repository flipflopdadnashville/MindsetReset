using UnityEngine;
using System.Collections;

public class Actuator : MonoBehaviour
{
    public virtual AIPath GetPath (Goal goal)
    {
        return new AIPath();
    }

    public virtual Steering GetOutput (AIPath path, Goal goal)
    {
        return new Steering();
    }
}
