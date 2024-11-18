using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

public class ChangeWallpaperShader : MonoBehaviour
{
    // Toggle between Diffuse and Transparent/Diffuse shaders
    // when space key is pressed
    public MazecraftGameManager instance;
    public List<Shader> shaderList = new List<Shader>();
    Shader defaultShader;
    Shader shader1;
    Shader shader2;
    Shader shader3;
    //Shader shader4;
    //public Object[] shaders;
    //public Object[] textures;
    RawImage image;
    //public Object[] materials;
    public GameObject wallpaper;
    int i = 0; 
    int j = 0;
    int k = 0;
    public bool randomizeWallpaper = false;
    public bool controlKaleidoscope = false;
    public float randomizeInterval = 300f;
    public GameObject flame;
    public GameObject lightRays;
    public GameObject kaleidoscopeControlsToggle;

    //public string key;
    AsyncOperationHandle<Shader> opShaderHandle;
    AsyncOperationHandle<Texture2D> opTextureHandle;
    AsyncOperationHandle<Material> opMaterialHandle;

    //[SerializeField] private List<AssetReference> _shaderReferences;
    int shaderIndex = 0;
    int textureIndex = 0;
    int materialIndex = 0;
    float currentSegmentCount;


    void Start()
    {
        kaleidoscopeControlsToggle.SetActive(false);
        image = GetComponent<RawImage> ();
        image.material.SetFloat("_SegmentCount", Random.Range(-20, 20));
        //textures = Resources.LoadAll("Textures", typeof(Texture));
        //materials = Resources.LoadAll("WallpaperMaterials", typeof(Material));
        //shaders = Resources.LoadAll("Shaders", typeof(Shader));
        //key = "Shaders";

        //foreach (Object shader in shaders){

        //    if (shader.ToString().Contains("Hidden"))
        //    {
        //        //Debug.Log(shader.ToString());
        //    }
        //    else
        //    {
        //        shaderList.Add((Shader)shader);
        //    }
        //}
        shader1 = Shader.Find("UltraEffects/Kaleidoscope");
        shader2 = Shader.Find("Visualab/FlameEye");
        shader3 = Shader.Find("Standard");
        ////this is cool, and we should find a place to use it, but it's not good like this...
        ////shader4 = Shader.Find("Procedural Toolkit/Examples/Animation");
        shaderList.Add(shader1);
        shaderList.Add(shader2);
        shaderList.Add(shader3);
        ////shaders.Add(shader4);
        //defaultShader = image.material.shader;
        ////RandomizeWallpaper();
    }

    public IEnumerator LoadShader(int shaderIndex)
    {
        opShaderHandle = Addressables.LoadAssetAsync<Shader>(instance._preloadedShaderKeys[Random.Range(0, instance._preloadedShaderKeys.Count -1)]);
        yield return opShaderHandle;

        //Debug.Log("opShaderHandle result is: " + opShaderHandle.Result);

        if (opShaderHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Shader currentShader = opShaderHandle.Result;
            image.material.shader = currentShader;
        }
    }

    public IEnumerator LoadTexture(int textureIndex)
    {
        opTextureHandle = Addressables.LoadAssetAsync<Texture2D>(instance._preloadedTextureKeys[Random.Range(0, instance._preloadedTextureKeys.Count - 1)]);
        yield return opTextureHandle;

        //Debug.Log("opTextureHandle result is: " + opTextureHandle.Result);

        if (opTextureHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Texture2D currentTexture = opTextureHandle.Result;
            image.texture = currentTexture;
        }
    }

    public IEnumerator LoadMaterial(int materialIndex)
    {
        opMaterialHandle = Addressables.LoadAssetAsync<Material>(instance._preloadedMaterialKeys[Random.Range(0, instance._preloadedMaterialKeys.Count - 1)]);
        yield return opMaterialHandle;

        //Debug.Log("opMaterialHandle result is: " + opMaterialHandle.Result);

        if (opMaterialHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Material currentMaterial = opMaterialHandle.Result;
            image.material = currentMaterial;
        }
    }

    void OnDestroy()
    {
        if (opShaderHandle.IsValid())
        {
            Addressables.Release(opShaderHandle);
        }

        if (opTextureHandle.IsValid())
        {
            Addressables.Release(opTextureHandle);
        }

        if (opMaterialHandle.IsValid())
        {
            Addressables.Release(opMaterialHandle);
        }
    }

    void Update()
    {
        if(currentSegmentCount > 500 || currentSegmentCount < -500)
        {
            image.material.SetFloat("_SegmentCount", Random.Range(-20,20));
        }

        /*if (image.material.name == "Mandlebrot")
        {
            if (wallpaper.GetComponent<MandlebrotBackground>().enabled == false)
            {
                wallpaper.GetComponent<MandlebrotBackground>().enabled = true;
            }
        }
        else if (image.material.name != "Mandlebrot")
        {
            wallpaper.GetComponent<MandlebrotBackground>().enabled = false;
        }*/

        if (controlKaleidoscope)
        {
            KaleidescopeController(true);
            //currentSegmentCount = image.material.GetFloat("_SegmentCount");
            //float newSegmentCount = currentSegmentCount - .005f;
            //image.material.SetFloat("_SegmentCount", newSegmentCount);
        }
        //Debug.Log("Shader is Kaleidoscope. Activating Kaleidoscope Controller.");

        /*if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ToggleWallpaperRandomization();
        }*/
    }

    public void ToggleWallpaperRandomization()
    {
        //randomizeWallpaper = !randomizeWallpaper;

        //2024-11-12 taking out kaleidoscope for now
        /*if (randomizeWallpaper == true)
        {
            flame.SetActive(false);
            lightRays.SetActive(false);
            kaleidoscopeControlsToggle.SetActive(true);
            InvokeRepeating("RandomizeWallpaper", .1f, randomizeInterval); 
            instance.SetNotificationSettings("Kaleidoscope mode activated.", "", "", instance.GetNotificationStatus());
        }
        else
        {
            CancelInvoke();
            flame.SetActive(true);
            lightRays.SetActive(true);
            kaleidoscopeControlsToggle.SetActive(false);
            instance.SetNotificationSettings("Eternal flame mode activated.", "", "", instance.GetNotificationStatus());
            //instance.SetNotificationSettings("Tweak the lighting settings", "", "Click on the palette icon in the taskbar.", instance.GetNotificationStatus());
        }*/
    }

    public void ToggleKaleidoscopeControls()
    {
        controlKaleidoscope = !controlKaleidoscope;

        string message = "";

        if (controlKaleidoscope)
        {
            instance.SetNotificationSettings(message, "Kaleidoscope controls enabled", "Kaleidoscope controls enabled", true);
        }
        else
        {
            instance.SetNotificationSettings(message, "Kaleidoscope controls disabled", "Kaleidoscope controls disabled", true);
        }
    }

    private void RandomizeWallpaper()
    {
        image.material.shader = shader1;

        int randomNum = Random.Range(0, 6);

        if (randomNum < 4)
        {
            if (textureIndex < instance._preloadedTextureKeys.Count - 1)
            {
                textureIndex++;
            }
            else
            {
                textureIndex = 0;
            }

            //Debug.Log("textureIndex is: " + shaderIndex);

            if (wallpaper.activeInHierarchy == true)
            {
                StartCoroutine(LoadTexture(textureIndex));
            }

            //ChangeShaderTexture();
            //InvokeRepeating("ChangeShaderTexture", .1f, 3f);
        }
        else if (randomNum == 4)
        {
            if (materialIndex < instance._preloadedShaderKeys.Count - 1)
            {
                shaderIndex++;
            }
            else
            {
                shaderIndex = 0;
            }

            //Debug.Log("materialIndex is: " + shaderIndex);

            StartCoroutine(LoadShader(shaderIndex));
            //StartCoroutine(LoadMaterial(materialIndex));
            //ChangeMaterial();
            //InvokeRepeating("ChangeShader", .1f, 3f);
        }
        else if (randomNum > 4)
        {
            if (shaderIndex < instance._preloadedShaderKeys.Count - 1)
            {
                shaderIndex++;
            }
            else
            {
                shaderIndex = 0;
            }

            //Debug.Log("shaderIndex is: " + shaderIndex);

            StartCoroutine(LoadShader(shaderIndex));
            //ChangeShader();
            //InvokeRepeating("ChangeMaterial", .1f, 3f);
        }
        else
        {
            //This should never happen... just putting it here to avoid errors
            //StartCoroutine(LoadMaterial(materialIndex));
            //ChangeMaterial();
            //InvokeRepeating("ChangeMaterial", .1f, 3f);
        }
    }

    public void RevealPhoto()
    {
        image.material.shader = shader3; 
    }
    
    public void ChangeShader(){
        //if(instance.game.activeInHierarchy == false){
            i++;
            if(i == shaderList.Count){
                i = 0;
            }

            image.material.shader = shaderList[Random.Range(0, shaderList.Count - 1)];
            //Debug.Log("Current Shader is: " + image.material.shader);
        //}
    }

    /*public void ChangeShaderTexture(){
        //if(instance.game.activeInHierarchy == false){
            j++;
            if(j == textures.Length){
                image.texture = null;
            }
            else if(j == textures.Length + 1){
                j = 0;
            }

            image.texture = (Texture)textures[Random.Range(0, textures.Length - 1)];
        //}
    }*/

    /*public void ChangeMaterial()
    {
        //if(instance.game.activeInHierarchy == false){
            k++;
            if(k == materials.Length){
                k = 0;
            }
           
            image.texture = null;
            image.material = (Material)materials[k];
            defaultShader = image.material.shader;
            //Debug.Log("defaultShader is: " + defaultShader);
        //}
    }*/

    public void SetShaderToDefaults(){
        /*if (instance.game.activeInHierarchy == false)
        {*/
            if (image.material.name == "Mandlebrot")
            {
                image.texture = null;
                image.material.shader = shader3;

            }
            else
            {
                image.texture = null;
                image.material.shader = shader1;
            }
        /*}*/
    }

    public void KaleidescopeController(bool active){

        if (active == true)
        {
            currentSegmentCount = image.material.GetFloat("_SegmentCount");
            float newSegmentCount = currentSegmentCount - .005f;
            image.material.SetFloat("_SegmentCount", newSegmentCount);

            if (Input.GetMouseButton(0))
            {
                //Debug.Log("Detected mouse click");
                if (Input.GetAxis("Mouse X") < 0)
                {
                    //Debug.Log("5 was pressed");
                    float currentSegmentCount = image.material.GetFloat("_SegmentCount");
                    //Debug.Log("Current segment count is: " + currentSegmentCount);
                    newSegmentCount = currentSegmentCount + .50f;
                    //Debug.Log("New segment count is: " + newSegmentCount);
                    image.material.SetFloat("_SegmentCount", newSegmentCount);
                }
                else if (Input.GetAxis("Mouse X") > 0)
                {
                    //Debug.Log("5 was pressed");
                    float currentSegmentCount = image.material.GetFloat("_SegmentCount");
                    //Debug.Log("Current segment count is: " + currentSegmentCount);
                    newSegmentCount = currentSegmentCount - .50f;
                    //Debug.Log("New segment count is: " + newSegmentCount);
                    image.material.SetFloat("_SegmentCount", newSegmentCount);
                }

            }
        }

        /*if (controlKaleidoscope){
            if (Input.GetMouseButton(0))
            {
                //Debug.Log("5 was pressed");
                currentSegmentCount = image.material.GetFloat("_SegmentCount");
                //Debug.Log("Current segment count is: " + currentSegmentCount);
                newSegmentCount = currentSegmentCount - .15f;
                //Debug.Log("New segment count is: " + newSegmentCount);
                image.material.SetFloat("_SegmentCount", newSegmentCount);
            }
            if (Input.GetMouseButton(1))
            {
                //Debug.Log("5 was pressed");
                float currentSegmentCount = image.material.GetFloat("_SegmentCount");
                //Debug.Log("Current segment count is: " + currentSegmentCount);
                newSegmentCount = currentSegmentCount + .15f;
                //Debug.Log("New segment count is: " + newSegmentCount);
                image.material.SetFloat("_SegmentCount", newSegmentCount);
            }
        }*/
    }

    void OnApplicationQuit()
    {
        SetShaderToDefaults();
    }
}