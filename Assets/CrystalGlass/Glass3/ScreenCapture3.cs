using UnityEngine;

public class ScreenCapture3 : MonoBehaviour
{
	public LayerMask m_GlassLayers = -1;
	public bool m_ShowRt = false;
	RenderTexture m_Rt;
	Camera m_RTCam;

	void Start ()
	{
		m_Rt = new RenderTexture (Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
		m_Rt.name = "Scene";
	}
	void Update ()
	{
		Camera c = Camera.main;
		if (m_RTCam == null)
		{
			GameObject go = new GameObject ("RtCamera", typeof (Camera), typeof (Skybox));
			m_RTCam = go.GetComponent<Camera> ();
			go.transform.parent = c.transform;
		}
		m_RTCam.CopyFrom (c);
		m_RTCam.allowMSAA = m_RTCam.allowHDR = false;
		m_RTCam.targetTexture = m_Rt;
		m_RTCam.cullingMask &= ~m_GlassLayers;
		m_RTCam.enabled = false;
		if (m_RTCam.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox skyCurr = c.GetComponent (typeof (Skybox)) as Skybox;
			Skybox skyRt = m_RTCam.GetComponent (typeof (Skybox)) as Skybox;
			skyRt.enabled = true;
			skyRt.material = skyCurr.material;
		}
		m_RTCam.Render ();
		Shader.SetGlobalTexture ("_Global_ScreenTex", m_Rt);
	}
	void OnGUI ()
	{
		if (m_ShowRt)
			GUI.DrawTexture (new Rect (10, 10, 128, 128), m_Rt);
	}
}