using UnityEngine;

public class Glass2Setup : MonoBehaviour
{
	public Shader m_Sdr;
	public Texture2D m_Bump;
	[Range(0f, 0.1f)] public float m_BumpScale = 0.15f;
	public Texture2D m_Tint;
	public Color m_TintColor = Color.white;
	[Range(0f, 1f)]	public float m_Opacity = 0.1f;
	public bool m_Blur = false;
	Renderer[] m_Rds;
	
	void Start ()
	{
		m_Rds = GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < m_Rds.Length; i++)
			m_Rds[i].material.shader = m_Sdr;
	}
	void Update ()
	{
		for (int i = 0; i < m_Rds.Length; i++)
		{
			Renderer rd = m_Rds[i];
			if (m_Blur)
				rd.material.EnableKeyword ("CRYSTAL_GLASS_BLUR");
			else
				rd.material.DisableKeyword ("CRYSTAL_GLASS_BLUR");
			rd.material.SetTexture ("_BumpTex", m_Bump);
			rd.material.SetTexture ("_TintTex", m_Tint);
			rd.material.SetFloat ("_BumpScale", m_BumpScale);
			rd.material.SetFloat ("_Opacity", m_Opacity);
			rd.material.SetColor ("_TintColor", m_TintColor);
		}
	}
}