using UnityEngine;

public class CursorTrail : MonoBehaviour
{
    private TrailRenderer trail;
    public Color trailColor = new Color(1, 0, 0.08f, 0.15f);
    public float distanceFromCamera = 5;
    public float startWidth = 0.1f;
    public float endWidth = 0f;
    public float trailTime = 0.15f;
    float xAxisMovement;
    float yAxisMovement;

    Transform trailTransform;
    Camera thisCamera;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();

        GameObject trailObj = new GameObject("Mouse Trail");
        trailTransform = trailObj.transform;
        trail = trailObj.AddComponent<TrailRenderer>();
        trail.time = -1f;
        MoveTrailToCursor(Input.mousePosition);
        trail.time = trailTime;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        trail.numCapVertices = 2;
        trail.sharedMaterial = new Material(Shader.Find("Crystal Glass/Refraction/Blur"));
        trail.sharedMaterial.color = trailColor;
    }

    // Update is called once per frame
    void Update()
    {
        //float xAxisMovement = Input.GetAxis("Mouse X");
        //float yAxisMovement = Input.GetAxis("Mouse Y");

        //if ((xAxisMovement > -1.5 && xAxisMovement < 1.5) && (yAxisMovement > -1.5 && yAxisMovement < 1.5))
        //{
        //    trail.enabled = true;
            MoveTrailToCursor(Input.mousePosition);
        //}
        //else
        //{
        //    trail.enabled = false;
        //}
    }

    void MoveTrailToCursor(Vector3 screenPosition)
    {
        trailTransform.position = thisCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distanceFromCamera));
    }
}