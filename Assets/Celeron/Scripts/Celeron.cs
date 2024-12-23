using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Celeron : MonoBehaviour
{
	private GameObject	 timescaleSlider;
	public float         defaultPitch = 1F;
	public float		 minSpeed = 0;
	public float		 speed1;
	public float		 speed2;
	public float		 defaultSpeed = 100;
	public float		 speed3;
	public float	     speed4;
	public float		 maxSpeed = 500;
	public float         gameSpeed = 100;

	public TimeSpan      relativeTimer;
	public string        relativeTimerText;
	public float         relativeTime;

	public TimeSpan      absoluteTimer;
	public string        absoluteTimerText;
	public float         absoluteTime;

	public AudioSource   audio1;
	public AudioSource   audio2; 
	public AudioSource   audio3;
	public AudioSource   audio4;
	public AudioSource   audio5; 
	public AudioSource   audio6;
	public AudioSource   audio7;
	public AudioSource   audio8; 
	public AudioSource   audio9;

	// Use this for initialization
	void Start () {
		timescaleSlider = GameObject.Find("Time Slider");
		timescaleSlider.GetComponent<Slider>().value = 1;

		gameSpeed = defaultSpeed;

		if (audio1 != null) { audio1 = audio1.GetComponent<AudioSource>(); }
		if (audio2 != null) { audio2 = audio2.GetComponent<AudioSource>(); }
		if (audio3 != null) { audio3 = audio3.GetComponent<AudioSource>(); }
		if (audio4 != null) { audio4 = audio4.GetComponent<AudioSource>(); }
		if (audio5 != null) { audio5 = audio5.GetComponent<AudioSource>(); }
		if (audio6 != null) { audio6 = audio6.GetComponent<AudioSource>(); }
		if (audio7 != null) { audio7 = audio7.GetComponent<AudioSource>(); }
		if (audio8 != null) { audio8 = audio8.GetComponent<AudioSource>(); }
		if (audio9 != null) { audio9 = audio9.GetComponent<AudioSource>(); }

		if (audio1 != null) { audio1.pitch = defaultPitch; }
		if (audio2 != null) { audio2.pitch = defaultPitch; }
		if (audio3 != null) { audio3.pitch = defaultPitch; }
		if (audio4 != null) { audio4.pitch = defaultPitch; }
		if (audio5 != null) { audio5.pitch = defaultPitch; }
		if (audio6 != null) { audio6.pitch = defaultPitch; }
		if (audio7 != null) { audio7.pitch = defaultPitch; }
		if (audio8 != null) { audio8.pitch = defaultPitch; }
		if (audio9 != null) { audio9.pitch = defaultPitch; }
		  
	}
	
	// Update is called once per frame
	void Update ()
	{
		Time.timeScale = (timescaleSlider.GetComponent<Slider>().value);

		absoluteTime += Time.unscaledDeltaTime;
		absoluteTimer = TimeSpan.FromSeconds(absoluteTime);

		relativeTime += Time.deltaTime;
		relativeTimer = TimeSpan.FromSeconds(relativeTime);

		if (audio1 != null) { audio1.pitch = defaultPitch * Time.timeScale; } if (audio2 != null) { audio2.pitch = defaultPitch * Time.timeScale; }
		if (audio3 != null) { audio3.pitch = defaultPitch * Time.timeScale; } if (audio4 != null) { audio4.pitch = defaultPitch * Time.timeScale; }
		if (audio5 != null) { audio5.pitch = defaultPitch * Time.timeScale; } if (audio6 != null) { audio6.pitch = defaultPitch * Time.timeScale; }
		if (audio7 != null) { audio7.pitch = defaultPitch * Time.timeScale; } if (audio8 != null) { audio8.pitch = defaultPitch * Time.timeScale; }
		if (audio9 != null) { audio9.pitch = defaultPitch * Time.timeScale; }
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Celeron))]
[CanEditMultipleObjects]

public class CeleronEditor : Editor
{
	SerializedProperty defaultPitchProp, minSpeedProp, defaultSpeedProp, maxSpeedProp, gameSpeedProp,

	audio1Prop, audio2Prop, audio3Prop, audio4Prop, audio5Prop,
	audio6Prop, audio7Prop, audio8Prop, audio9Prop;

	void OnEnable()
	{
		// Setup the SerializedProperties.
		defaultPitchProp  = serializedObject.FindProperty("defaultPitch");
		minSpeedProp      = serializedObject.FindProperty("minSpeed");
		defaultSpeedProp  = serializedObject.FindProperty("defaultSpeed");
		maxSpeedProp      = serializedObject.FindProperty("maxSpeed");
		gameSpeedProp     = serializedObject.FindProperty("gameSpeed");

		audio1Prop        = serializedObject.FindProperty("audio1"); audio2Prop = serializedObject.FindProperty("audio2"); audio3Prop = serializedObject.FindProperty("audio3");
		audio4Prop        = serializedObject.FindProperty("audio4"); audio5Prop = serializedObject.FindProperty("audio5"); audio6Prop = serializedObject.FindProperty("audio6");
		audio7Prop        = serializedObject.FindProperty("audio7"); audio8Prop = serializedObject.FindProperty("audio8"); audio9Prop = serializedObject.FindProperty("audio9");
	}

	public override void OnInspectorGUI()
	{
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();

		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));
		Celeron celeron = (Celeron)target;
		// Color.green = new Color ((138F/255F), (255F/255F), (167F/255F));
		// Color.gray = new Color (0.8F, 0.8F, 0.8F);

		celeron.speed1 = Mathf.Round(celeron.minSpeed + ((celeron.defaultSpeed - celeron.minSpeed) * 0.33F));
		celeron.speed2 = Mathf.Round(celeron.minSpeed + ((celeron.defaultSpeed - celeron.minSpeed) * 0.67F));
		celeron.speed3 = Mathf.Round(celeron.defaultSpeed + ((celeron.maxSpeed - celeron.defaultSpeed) * 0.33F));
		celeron.speed4 = Mathf.Round(celeron.defaultSpeed + ((celeron.maxSpeed - celeron.defaultSpeed) * 0.67F));
        EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("                  C E L E R O N", EditorStyles.largeLabel);
		GUI.color = new Color ((102F/255F), (255F/255F), (156F/255F));
		EditorGUILayout.Space();
		celeron.absoluteTimerText = string.Format("{0:D2}:{1:D2}:{2:D2}", celeron.absoluteTimer.Hours, celeron.absoluteTimer.Minutes, celeron.absoluteTimer.Seconds);
		EditorGUILayout.LabelField(celeron.absoluteTimerText, EditorStyles.helpBox);
		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("  a c c e l e r a t e    y o u r   v i s i o n", EditorStyles.label);
		GUI.color = new Color ((255F/255F), (108F/255F), (108F/255F));
		EditorGUILayout.Space();
		celeron.relativeTimerText = string.Format("{0:D2}:{1:D2}:{2:D2}", celeron.relativeTimer.Hours, celeron.relativeTimer.Minutes, celeron.relativeTimer.Seconds);
		EditorGUILayout.LabelField(celeron.relativeTimerText, EditorStyles.helpBox);
		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		gameSpeedProp.floatValue = (int)EditorGUILayout.Slider("", gameSpeedProp.floatValue, 0, maxSpeedProp.floatValue);

		EditorGUILayout.BeginHorizontal();
		GUI.color = new Color ((255F/255F), (108F/255F), (108F/255F)); if (GUILayout.Button ("MIN",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = minSpeedProp.floatValue; }
		GUI.color = new Color ((255F/255F), (144F/255F), (103F/255F)); if (GUILayout.Button ("   ",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = celeron.speed1; }
		GUI.color = new Color ((255F/255F), (202F/255F), (92F/255F)); if (GUILayout.Button ("   ",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = celeron.speed2; }
		GUI.color = new Color ((253F/255F), (255F/255F), (90F/255F)); if (GUILayout.Button ("MID",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = defaultSpeedProp.floatValue; }
		GUI.color = new Color ((206F/255F), (255F/255F), (109F/255F)); if (GUILayout.Button ("   ",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = celeron.speed3; }
		GUI.color = new Color ((141F/255F), (255F/255F), (138F/255F)); if (GUILayout.Button ("   ",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = celeron.speed4; }
		GUI.color = new Color ((102F/255F), (255F/255F), (156F/255F)); if (GUILayout.Button ("MAX",  EditorStyles.miniButton)) { gameSpeedProp.floatValue = maxSpeedProp.floatValue; }

		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		GUI.color = new Color ((255F/255F), (108F/255F), (108F/255F)); EditorGUILayout.PropertyField(minSpeedProp, new GUIContent(""));
		GUI.color = new Color ((255F/255F), (144F/255F), (103F/255F)); EditorGUILayout.LabelField("", celeron.speed1.ToString(), EditorStyles.numberField, GUILayout.MaxWidth(50));
		GUI.color = new Color ((255F/255F), (202F/255F), (92F/255F)); EditorGUILayout.LabelField("", celeron.speed2.ToString(), EditorStyles.numberField, GUILayout.MaxWidth(50));
		GUI.color = new Color ((253F/255F), (255F/255F), (90F/255F)); EditorGUILayout.PropertyField(defaultSpeedProp, new GUIContent(""));
		GUI.color = new Color ((206F/255F), (255F/255F), (109F/255F)); EditorGUILayout.LabelField("", celeron.speed3.ToString(), EditorStyles.numberField, GUILayout.MaxWidth(50));
		GUI.color = new Color ((141F/255F), (255F/255F), (138F/255F)); EditorGUILayout.LabelField("", celeron.speed4.ToString(), EditorStyles.numberField, GUILayout.MaxWidth(50));
		GUI.color = new Color ((102F/255F), (255F/255F), (156F/255F)); EditorGUILayout.PropertyField(maxSpeedProp, new GUIContent(""));

		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		defaultPitchProp.floatValue = EditorGUILayout.Slider("d e f a u l t   p i t c h", defaultPitchProp.floatValue, -3, 3);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("i n - g a m e   p i t c h", EditorStyles.label);
		GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F));
		EditorGUILayout.LabelField((defaultPitchProp.floatValue * Time.timeScale).ToString(), EditorStyles.toolbarTextField);
		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("a f f e c t e d   a u d i o   s o u r c e s", EditorStyles.label);

		GUI.color = new Color ((255F/255F), (255F/255F), (255F/255F));
		if (GUILayout.Button ("Clear All", EditorStyles.miniButton))
		{
			audio1Prop.objectReferenceValue = null; audio2Prop.objectReferenceValue = null; audio3Prop.objectReferenceValue = null;
			audio4Prop.objectReferenceValue = null; audio5Prop.objectReferenceValue = null; audio6Prop.objectReferenceValue = null;
			audio7Prop.objectReferenceValue = null; audio8Prop.objectReferenceValue = null; audio9Prop.objectReferenceValue = null;
		}
		GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F));

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if (audio1Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio1Prop, new GUIContent(""));
		if (audio2Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio2Prop, new GUIContent(""));
		if (audio3Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio3Prop, new GUIContent(""));
		EditorGUILayout.EndHorizontal(); EditorGUILayout.BeginHorizontal();
		if (audio4Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio4Prop, new GUIContent(""));
		if (audio5Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio5Prop, new GUIContent(""));
		if (audio6Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio6Prop, new GUIContent(""));
		EditorGUILayout.EndHorizontal(); EditorGUILayout.BeginHorizontal();
		if (audio7Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio7Prop, new GUIContent(""));
		if (audio8Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio8Prop, new GUIContent(""));
		if (audio9Prop.objectReferenceValue != null) { GUI.color = new Color ((150F/255F), (209F/255F), (255F/255F)); } else { GUI.color = new Color ((225F/255F), (225F/255F), (225F/255F)); }
		EditorGUILayout.PropertyField(audio9Prop, new GUIContent(""));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties();
	}
}
#endif