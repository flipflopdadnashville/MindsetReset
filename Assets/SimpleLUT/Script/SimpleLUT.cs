using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DigitalRuby.SimpleLUT
{
    [ExecuteInEditMode]
    public class SimpleLUT : MonoBehaviour
    {
        MazecraftGameManager instance;
        [SerializeField]
        private bool randomizeLUTSettings = true;

        [Tooltip("Shader to use for the lookup.")]
        public Shader Shader;
        private Shader previousShader;

        [Tooltip("Texture to use for the lookup. Make sure it has read/write enabled and mipmaps disabled. The height must equal the square root of the width.")]
        public Texture2D LookupTexture;
        private Texture2D previousTexture;
        public List<Texture2D> LookupTextures;

        [Range(0, 1)]
        [Tooltip("Lerp between original (0) and corrected color (1)")]
        public float Amount = 1.0f;

        [Tooltip("Tint color, applied to the final pixel")]
        public Color TintColor = Color.white;

        [Range(0, 360)]
        [Tooltip("Hue")]
        public float Hue = 0.0f;

        [Range(-1, 1)]
        [Tooltip("Saturation")]
        public float Saturation = 0.0f;

        [Range(-1, 1)]
        [Tooltip("Brightness")]
        public float Brightness = 0.0f;

        [Range(-1, 1)]
        [Tooltip("Contrast")]
        public float Contrast = 0.0f;

        [Range(0, 1)]
        [Tooltip("Sharpness")]
        public float Sharpness = 0.0f;

        public Material material;
        private Texture3D converted3DLut = null;
        private int lutSize;

        // Invoked when the value of the slider changes.
        public void ValueChangeCheck()
        {
            /*Amount = instance.amountSlider.value;
            Hue = instance.hueSlider.value;
            Saturation = instance.saturationSlider.value;
            Brightness = instance.brightnessSlider.value;
            Contrast = instance.contrastSlider.value;
            Sharpness = instance.sharpnessSlider.value;*/
        }

        private void CreateMaterial()
        {
            if (Shader == null)
            {
                material = null;
                Debug.LogError("Must set a shader to use LUT");
                return;
            }

            material = new Material(Shader);
            material.hideFlags = HideFlags.DontSave;
        }

        private void Start()
        {
            instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
            /*instance.amountSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
            instance.hueSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
            instance.saturationSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
            instance.brightnessSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		    instance.contrastSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		    instance.sharpnessSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});*/

            if (GetComponent<Camera>() == null)
            {
                Debug.LogError("This script must be attached to a Camera");
            }

            Amount = 0;
            Hue = 0;
            Saturation = 0;
            Brightness = 0;
            Contrast = 0;

            LookupTexture = LookupTextures[UnityEngine.Random.Range(0, LookupTextures.Count - 1)];
        }

        private void Update()
        {
            //CheckKey();

            if (Shader != previousShader)
            {
                previousShader = Shader;
                CreateMaterial();
            }

            if (LookupTexture != previousTexture)
            {
                previousTexture = LookupTexture;
                Convert(LookupTexture);
            }
        }

        private void OnDestroy()
        {
            if (converted3DLut != null)
            {
                DestroyImmediate(converted3DLut);
            }
            converted3DLut = null;
        }

        public void SetIdentityLut()
        {
            if (!SystemInfo.supports3DTextures)
            {
                return;
            }
            else if (converted3DLut != null)
            {
                DestroyImmediate(converted3DLut);
            }

            int dim = 16;
            var newC = new Color[dim * dim * dim];
            float oneOverDim = 1.0f / (1.0f * dim - 1.0f);

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    for (int k = 0; k < dim; k++)
                    {
                        newC[i + (j * dim) + (k * dim * dim)] = new Color((i * 1.0f) * oneOverDim, (j * 1.0f) * oneOverDim, (k * 1.0f) * oneOverDim, 1.0f);
                    }
                }
            }

            converted3DLut = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
            converted3DLut.SetPixels(newC);
            converted3DLut.Apply();
            lutSize = converted3DLut.width;
            converted3DLut.wrapMode = TextureWrapMode.Clamp;
        }

        public bool ValidDimensions(Texture2D tex2d)
        {
            if (tex2d == null)
            {
                return false;
            }

            int h = tex2d.height;
            if (h != Mathf.FloorToInt(Mathf.Sqrt(tex2d.width)))
            {
                return false;
            }
            return true;
        }

        internal bool Convert(Texture2D lookupTexture)
        {
            if (!SystemInfo.supports3DTextures)
            {
                Debug.LogError("System does not support 3D textures");
                return false;
            }
            else if (lookupTexture == null)
            {
                SetIdentityLut();
            }
            else
            {
                if (converted3DLut != null)
                {
                    DestroyImmediate(converted3DLut);
                }

                if (lookupTexture.mipmapCount > 1)
                {
                    Debug.LogError("Lookup texture must not have mipmaps");
                    return false;
                }

                try
                {
                    int dim = lookupTexture.width * lookupTexture.height;
                    dim = lookupTexture.height;

                    if (!ValidDimensions(lookupTexture))
                    {
                        Debug.LogError("Lookup texture dimensions must be a power of two. The height must equal the square root of the width.");
                        return false;
                    }

                    var c = lookupTexture.GetPixels();
                    var newC = new Color[c.Length];

                    for (int i = 0; i < dim; i++)
                    {
                        for (int j = 0; j < dim; j++)
                        {
                            for (int k = 0; k < dim; k++)
                            {
                                int j_ = dim - j - 1;
                                newC[i + (j * dim) + (k * dim * dim)] = c[k * dim + i + j_ * dim * dim];
                            }
                        }
                    }

                    converted3DLut = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
                    converted3DLut.SetPixels(newC);
                    converted3DLut.Apply();
                    lutSize = converted3DLut.width;
                    converted3DLut.wrapMode = TextureWrapMode.Clamp;
                }
                catch (Exception ex)
                {
                    Debug.LogError("Unable to convert texture to LUT texture, make sure it is read/write. Error: " + ex);
                }
            }

            return true;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (converted3DLut == null)
            {
                SetIdentityLut();
            }

            if (converted3DLut == null || material == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            material.SetTexture("_MainTex", source);
            material.SetTexture("_ClutTex", converted3DLut);
            material.SetFloat("_Amount", Amount);
            material.SetColor("_TintColor", TintColor);
            material.SetFloat("_Hue", Hue);
            material.SetFloat("_Saturation", Saturation + 1.0f);
            material.SetFloat("_Brightness", Brightness + 1.0f);
            material.SetFloat("_Contrast", Contrast + 1.0f);
            material.SetFloat("_Scale", (lutSize - 1) / (1.0f * lutSize));
            material.SetFloat("_Offset", 1.0f / (2.0f * lutSize));

            float actualSharpness = (Sharpness * 4.0f * 0.2f);
            material.SetFloat("_SharpnessCenterMultiplier", 1.0f + (4.0f * actualSharpness));
            material.SetFloat("_SharpnessEdgeMultiplier", actualSharpness);
            material.SetVector("_ImageWidthFactor", new Vector4(1.0f / (float)source.width, 0.0f, 0.0f, 0.0f));
            material.SetVector("_ImageHeightFactor", new Vector4(0.0f, 1.0f / (float)source.height, 0.0f, 0.0f));

            Graphics.Blit(source, destination, material, QualitySettings.activeColorSpace == ColorSpace.Linear ? 1 : 0);
        }

        void CheckKey() {
            /*if(Input.GetKey(KeyCode.Alpha9)){
                instance.hueSlider.value = 0;
                instance.brightnessSlider.value = 0;
                instance.contrastSlider.value = 0;
                instance.saturationSlider.value = 0;
            }*/

            /*if (instance.game.activeInHierarchy == false)
            {
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    ToggleLUTSettingsRandomization();
                }
            }*/
        }

        public void ToggleLUTSettingsRandomization()
        {
            randomizeLUTSettings = !randomizeLUTSettings;

            if (randomizeLUTSettings == true)
            {
                InvokeRepeating("RandomizeLUTSettings", .1f, 30f);
                //instance.SetNotificationSettings("Wallpaper randomization started.", "", "", instance.GetNotificationStatus());
            }
            else
            {
                CancelInvoke();
                //instance.SetNotificationSettings("Wallpaper randomization stopped.", "", "", instance.GetNotificationStatus());
                //instance.SetNotificationSettings("Tweak the lighting settings", "", "Click on the palette icon in the taskbar.", instance.GetNotificationStatus());
            }
        }

        public void RandomizeLUTSettings()
        {
            int randomNum = UnityEngine.Random.Range(0, 10);

            Brightness = UnityEngine.Random.Range(-.40f, .40f);
            Contrast = UnityEngine.Random.Range(-.25f, .25f);
            Amount = UnityEngine.Random.value;
            Hue = UnityEngine.Random.Range(0, 360);
            Saturation = UnityEngine.Random.Range(-1f, 1f);
            //Sharpness = UnityEngine.Random.value;

            /*if (Brightness < -.45f || Brightness > .45)
            {
                StartCoroutine(LerpBrightness(Brightness, .15f, true, 3f));
            }

            if (Contrast < -.25f || Contrast > .25)
            {
                Contrast = .1f;
            }*/

            /*if (randomNum % 3 == 0)
            {
                Saturation = Saturation * -1;
                StartCoroutine(LerpBrightness(Brightness, Brightness * -1, true, 3f));
                Contrast = Contrast * -1;
            }*/

            LookupTexture = LookupTextures[UnityEngine.Random.Range(0, LookupTextures.Count - 1)];
            
            //if (Sharpness > .7)
            //{

            //    Sharpness = 0f;
            //}
        }

        public void BlackScreen()
        {   
            StartCoroutine(LerpBrightness(Brightness, -1, false, 3.7f));
        }

        public IEnumerator LerpBrightness(float originalBrightness, float targetBrightness, bool waking, float duration) {
            float time = 0;
            while (time < duration)
            {
                float currentBrightness = Brightness;
                if (waking == false) {
                    if (currentBrightness > targetBrightness) {
                        Brightness = currentBrightness - .01f;
                    }
                }
                else if (waking == true) {
                    if (currentBrightness < targetBrightness) {
                        Brightness = currentBrightness + .01f;
                    }
                }
                time += Time.deltaTime;
                yield return null;
            }
            Brightness = targetBrightness;
        }
    }
}
