using UnityEngine;
using System.Collections;

namespace Fraktalia.Core.Samples
{
    public class NoClipFirstPersonController : MonoBehaviour
    {

        public float movementForwardMultiplier = 4f;
        public float movementSideMultiplier = 4f;

        private string forwardAxis = "Vertical";
        private string horizontalAxis = "Horizontal";

        void FixedUpdate()
        {
            float forwardMovement = Input.GetAxis(forwardAxis) * movementForwardMultiplier;
            float horizontalMovement = Input.GetAxis(horizontalAxis) * movementSideMultiplier;
            Vector3 movementDelta = new Vector3(horizontalMovement, 0, forwardMovement);
            GetComponent<Rigidbody>().AddRelativeForce(movementDelta);
        }
    }
}
