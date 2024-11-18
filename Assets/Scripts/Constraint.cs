using UnityEngine;
using System.Collections;

public class Constraint : MonoBehaviour
{
	// Use this for initialization
	public virtual bool WillViolate (AIPath path)
    {
        return true;
    }

    public virtual Goal Suggest (AIPath path) {
        return new Goal();
    }
}
