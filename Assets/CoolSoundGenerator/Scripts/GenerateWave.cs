using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GenerateWave : MonoBehaviour
{

    public float frequencyLeftChannel;
    public float frequencyRightChannel;
    public float sampleRate = 44100;
    public float waveLengthInSeconds = 1.0f;

    AudioSource audioSource;
    AudioSource audioSourceForPlaySavedSound;

    public Text frequencyLeftChannelText;
    public Text frequencyRighttChannelText;
    public Text volumeText;

    public Slider sliderLeftChannel;
    public Slider sliderRighttChannel;
    public Slider sliderVolume;
    public Slider sliderFrequencyCorrection;

    public GameObject sineOFFPictureLeftChannel;
    public GameObject sineBlackScreenLeftChannel;
    public GameObject sineOFFPictureRightChannel;
    public GameObject sineBlackScreenRightChannel;
    public GameObject mergePicture;
    public GameObject morseKeyDown;
    public GameObject morseKeyUp;
    public GameObject playPicture;
    public GameObject pausePicture;

    public static float varSinForGrapherLeftChannel;
    public static float varSinForGrapherRightChannel;
    public GameObject GraphParticleSystemForLeftChannel;
    public GameObject GraphParticleSystemForRightChannel;

    public Toggle toggleSineWave;
    public Toggle toggleTriangleWave;
    public Toggle toggleSawWave;
    public Toggle toggleSquareWave;
    public Toggle toggleTangentWave;
    public Toggle toggleMergeLeftAndRightChannel;

    private int headerSize = 44; //default for uncompressed wav
    private bool recordMode = false;
    private FileStream fileStream;
    private int bufferSize;
    private int numBuffers;
    private int outputRate = 44100;
    public GameObject TextRecStart;
    public GameObject ButtonSaveTextSTOP;
    public GameObject ButtonSaveTextSTART;
    public Button ButtonSave;
    public Button buttonMorse;
    public GameObject ImageHand;
    public Button ButtonStartStop;

    public static int TypeOfWave = 1; //1-Sine; 2-Triangle; 3-Saw; 4-Square; 5-Tangent

    int timeIndex = 0;
    bool start_ = false;
    bool soundGeneratorIsWorking = false;
    private bool squareWaveAmplitudeBool = false;
    private int squareWaveAmplitude = 1;
    private int varForTriangle = 0;
    public static float amplitudeWave = 1.0f;

    //for triangle
    private float x_;
    private float x__;
    private float devideFactor;
    private float delta;
    private float deltaBlockMax;
    private bool triangleDOWN = false;

    
    //<Setting initial parameters>
    void Start()
    {
        //for save FrequencyCorrection on disk
        sliderFrequencyCorrection.value = PlayerPrefs.GetFloat("FrequencyCorrection");

        AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
        AudioConfiguration config = AudioSettings.GetConfiguration();
        config.sampleRate = outputRate;

        toggleMergeLeftAndRightChannel.isOn = false;

        frequencyLeftChannel = 440.0f;
        frequencyRightChannel = 440.0f;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
        audioSource.spatialBlend = 0; //makes the sound full 2D

        audioSourceForPlaySavedSound = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.volume = 1.0f;
        audioSource.spatialBlend = 0; //makes the sound full 2D
    }

   
    void Update()
    {

        //<for good work on differenent screens>
        float aspect = (float)Screen.width / (float)Screen.height;

        if (aspect > 1.2f)
        {
            Camera.main.fieldOfView = 81.0f;
            if (aspect > 1.4f)
            {
                Camera.main.fieldOfView = 74.0f;
                if (aspect > 1.6f)
                {
                    Camera.main.fieldOfView = 65.0f;
                    if (aspect > 1.8f)
                    {
                        Camera.main.fieldOfView = 60.0f;
                        if (aspect > 2.0f)
                        {
                            Camera.main.fieldOfView = 55.0f;

                        }
                    }
                }
            }
        }
        //</for good work on differenent screens>

        //<for ButtonMorse>
        if ((ObjectEvents.ObjectPressed == true) && (ObjectEvents.nameObject == "ButtonMorse"))
        {
            if (!audioSource.isPlaying)
            {
                timeIndex = 0;  //timer resets before playing sound
                audioSource.Play();
                frequencyLeftChannel = 440.0f;
                frequencyRightChannel = 440.0f;
                sliderLeftChannel.value = 0.022f;
                sliderRighttChannel.value = 0.022f;
                sineOFFPictureLeftChannel.SetActive(false);
                sineOFFPictureRightChannel.SetActive(false);
                morseKeyUp.SetActive(false);
                morseKeyDown.SetActive(true);

                sineOFFPictureLeftChannel.SetActive(false);
                sineBlackScreenLeftChannel.SetActive(true);
                sineOFFPictureRightChannel.SetActive(false);
                sineBlackScreenRightChannel.SetActive(true);
                GraphParticleSystemForLeftChannel.SetActive(true);
                GraphParticleSystemForRightChannel.SetActive(true);
            }
        }
        else if (soundGeneratorIsWorking != true)
        {
            audioSource.Stop();
            sineOFFPictureLeftChannel.SetActive(true);
            sineOFFPictureRightChannel.SetActive(true);
            morseKeyUp.SetActive(true);
            morseKeyDown.SetActive(false);

            sineOFFPictureLeftChannel.SetActive(true);
            sineBlackScreenLeftChannel.SetActive(false);
            sineOFFPictureRightChannel.SetActive(true);
            sineBlackScreenRightChannel.SetActive(false);
            GraphParticleSystemForLeftChannel.SetActive(false);
            GraphParticleSystemForRightChannel.SetActive(false);
        }
        //</for ButtonMorse>

        //<for channel panels>
        frequencyLeftChannelText.text = frequencyLeftChannel.ToString();
        frequencyRighttChannelText.text = frequencyRightChannel.ToString();
        volumeText.text = audioSource.volume.ToString();

        if (toggleMergeLeftAndRightChannel.isOn == true)
        {
            if (ObjectEvents.nameObject == "SliderLeftChannel")
            {
                sliderRighttChannel.value = sliderLeftChannel.value;
            }
            if (ObjectEvents.nameObject == "SliderRightChannel")
            {
                sliderLeftChannel.value = sliderRighttChannel.value;
            }
        }

        frequencyLeftChannel = sliderLeftChannel.value * 20000.0f;
        frequencyRightChannel = sliderRighttChannel.value * 20000.0f;
        audioSource.volume = sliderVolume.value;
        amplitudeWave = sliderVolume.value;
        //</for channel panels>

        //<for start/stop audio playback>
        if (start_ == true)
        {
            if (!audioSource.isPlaying)
            {
                timeIndex = 0;  //timer resets before playing sound
                audioSource.Play();
                start_ = !start_;
                soundGeneratorIsWorking = true;
                sineOFFPictureLeftChannel.SetActive(false);
                sineBlackScreenLeftChannel.SetActive(true);
                sineOFFPictureRightChannel.SetActive(false);
                sineBlackScreenRightChannel.SetActive(true);
                GraphParticleSystemForLeftChannel.SetActive(true);
                GraphParticleSystemForRightChannel.SetActive(true);
                playPicture.SetActive(false);
                pausePicture.SetActive(true);
                ButtonSave.interactable = true;
                ImageHand.SetActive(true);
            }
            else
            {

                recordMode = false;
                if (recordMode == true)
                {
                    WriteHeader();
                }
                TextRecStart.SetActive(false);
                ButtonSaveTextSTOP.SetActive(false);
                ButtonSaveTextSTART.SetActive(true);
                StopAllCoroutines();


                audioSource.Stop();
                start_ = !start_;
                soundGeneratorIsWorking = false;
                sineOFFPictureLeftChannel.SetActive(true);
                sineBlackScreenLeftChannel.SetActive(false);
                sineOFFPictureRightChannel.SetActive(true);
                sineBlackScreenRightChannel.SetActive(false);
                GraphParticleSystemForLeftChannel.SetActive(false);
                GraphParticleSystemForRightChannel.SetActive(false);
                playPicture.SetActive(true);
                pausePicture.SetActive(false);
                ButtonSave.interactable = false;
                ImageHand.SetActive(false);
            }
        }

        if ((ObjectEvents.nameObject != "ButtonMorse"))
        {
            float EPSILON = 0;
            if ((System.Math.Abs(frequencyLeftChannel) < EPSILON) || (soundGeneratorIsWorking == false))
            {
                    GraphParticleSystemForLeftChannel.SetActive(false);
                    sineOFFPictureLeftChannel.SetActive(true);
                    sineBlackScreenLeftChannel.SetActive(false);
            }
            else
            {
                sineOFFPictureLeftChannel.SetActive(false);
                sineBlackScreenLeftChannel.SetActive(true);
                GraphParticleSystemForLeftChannel.SetActive(true);
            }
            if ((System.Math.Abs(frequencyRightChannel) < EPSILON) || (soundGeneratorIsWorking == false))
            {
                    GraphParticleSystemForRightChannel.SetActive(false);
                    sineOFFPictureRightChannel.SetActive(true);
                    sineBlackScreenRightChannel.SetActive(false);
            }
            else
            {
                sineOFFPictureRightChannel.SetActive(false);
                sineBlackScreenRightChannel.SetActive(true);
                GraphParticleSystemForRightChannel.SetActive(true);
            }
        }
        //</for start/stop audio playback>
    }


    //<for play sound>
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)  //i = i + channels

        {
            data[i] = CreateWave(timeIndex, frequencyLeftChannel, sampleRate);

            if (channels == 2)
            {
                data[i + 1] = CreateWave(timeIndex, frequencyRightChannel, sampleRate);
                timeIndex++;
                if (timeIndex >= (sampleRate * waveLengthInSeconds))  //sampleRate = 44100 //waveLengthInSeconds = 1.0f
                {
                    timeIndex = 0;
                }
            }

            timeIndex++;

            if (timeIndex >= (sampleRate * waveLengthInSeconds))  //sampleRate = 44100 //waveLengthInSeconds = 1.0f
            {
                timeIndex = 0;
            }
        }

        //for save wav
        if (recordMode)
        {
            ConvertAndWrite(data); //audio data is interlaced
        }
    }


    //<for play saved wave file>
    public void playSavedFile()
    {
        if ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor))
        {
            AudioClip audio_ = Resources.Load<AudioClip>("CSG_sound");  //without file extension            audioSourceForPlaySavedSound.clip = audio_;
            if (!audioSourceForPlaySavedSound.isPlaying)
            {
                audioSourceForPlaySavedSound.Play();
            }
            else
            {
                audioSourceForPlaySavedSound.Stop();
            }
        }
        else
        {            
            if (audioSourceForPlaySavedSound.isPlaying)
            {
                audioSourceForPlaySavedSound.Stop();
            }
            else
            {
                StartCoroutine(GetAudioClip());
            }
        }
    }


    //<for iOS and Android>
    IEnumerator GetAudioClip()
    {
        //iOS: file:///var/mobile/Containers/Data/Application/79C57EE1-A458-4D34-AD8C-0A3BDD553DF6/Documents/CSG_sound.wav
        //Android:
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + Path.Combine(Application.persistentDataPath, "CSG_sound.wav"), AudioType.WAV))
        {
            yield return request.SendWebRequest();
           
            if (request.isHttpError && request.isNetworkError)
            {
                //Debug.Log("ERROR!!!!!! " + www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(request);
                audioSourceForPlaySavedSound.clip = myClip;
                if (!audioSourceForPlaySavedSound.isPlaying)
                {
                    audioSourceForPlaySavedSound.Play();
                }
                else
                {
                    audioSourceForPlaySavedSound.Stop();
                }
            }
        }
    }


    //<for save wav>
    public void saveWaveFile()
    {
        if (!recordMode)
        {
            ButtonStartStop.interactable = false;
            recordMode = true;
            // StartWriting("CSG_sound.wav");
            //JWR Edit
            StartWriting("Left_" + frequencyLeftChannel.ToString() + "Right_" + frequencyRightChannel.ToString() + "_sound.wav");
            TextRecStart.SetActive(true);
            //TextRecStop.SetActive(false);
            ButtonSaveTextSTOP.SetActive(true);
            ButtonSaveTextSTART.SetActive(false);

            StartCoroutine(PauseCoroutineForFlashSimple(0.5f));
        }
        else
        {
            recordMode = false;
            WriteHeader();
            TextRecStart.SetActive(false);
            ButtonSaveTextSTOP.SetActive(false);
            ButtonSaveTextSTART.SetActive(true);
            StopAllCoroutines();
            ButtonStartStop.interactable = true;

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();

#elif UNITY_IOS
    //Debug.Log("Unity iPhone");

#else
    //Debug.Log("Any other platform");

#endif

        }
    }


    //<for save wav file>
    public void StartWriting(string name)
    {
        //iOS: Application.persistentDataPath points to / var / mobile / Containers / Data / Application /< guid >/ Documents.
        //Android: Application.persistentDataPath points to / storage / emulated / 0 / Android / data /< packagename >/ files on most devices(some older phones might point to location on SD card if present),
        //the path is resolved using android.content.Context.getExternalFilesDir.
        //on a Mac is written into the user Library folder. (This folder is often hidden.) In recent Unity releases user data is written into
        //~/ Library / Application Support / company name / product name.Older versions of Unity wrote into the ~/ Library / Caches folder, or ~/ Library / Application Support / unity.company name.product name.
        //These folders are all searched for by Unity. The application finds and uses the oldest folder with the required data on your system.

        if ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor))
        {
            fileStream = new FileStream("./Assets/CoolSoundGenerator/Resources/" + name, FileMode.Create);
            //fileStream = new FileStream("./Assets/" + name, FileMode.Create);
        }
        else //iOS and Android
        {
            string tempPath = Path.Combine(Application.persistentDataPath, name);
            //print("tempPath_iOS = " + tempPath);  //tempPath_iOS = /var/mobile/Containers/Data/Application/79C57EE1-A458-4D34-AD8C-0A3BDD553DF6/Documents/CSG_sound.wav
            fileStream = new FileStream(tempPath, FileMode.Create);
        }

        byte emptyByte = new byte();

        for (int i = 0; i < headerSize; i++) //preparing the header
        {
            fileStream.WriteByte(emptyByte);
        }
    }

    //<for save wav file>
    void ConvertAndWrite(float[] dataSource)
    {
        Int16[] intData = new Int16[dataSource.Length];
        //converting in 2 steps : float[] to Int16[], //then Int16[] to Byte[]

        Byte[] bytesData = new Byte[dataSource.Length * 2];
        //bytesData array is twice the size of
        //dataSource array because a float converted in Int16 is 2 bytes.

        int rescaleFactor = 32767; //to convert float to Int16

        for (int i = 0; i < dataSource.Length; i++)
        {
            intData[i] = (Int16)(dataSource[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }


    //<for save wav file>
    void WriteHeader()
    {
        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(two);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate_ = BitConverter.GetBytes(outputRate);
        fileStream.Write(sampleRate_, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(outputRate * 4);

        fileStream.Write(byteRate, 0, 4);

        UInt16 four = 4;
        Byte[] blockAlign = BitConverter.GetBytes(four);
        fileStream.Write(blockAlign, 0, 2);

        UInt16 sixteen = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(sixteen);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] dataString = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(dataString, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(fileStream.Length - headerSize);
        fileStream.Write(subChunk2, 0, 4);

        fileStream.Close();
    }


    //<Create a wave>
    public float CreateWave(int timeIndex_, float frequency, float sampleRate)
    {
        frequency = frequency + (frequencyLeftChannel * sliderFrequencyCorrection.value) / 100.0f;

        if (TypeOfWave == 1)
        {
            //<SINE>
            varSinForGrapherLeftChannel = (frequencyLeftChannel / 20000) * 12;
            varSinForGrapherRightChannel = (frequencyRightChannel / 20000) * 12;
            return (Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate)) * amplitudeWave;
            //</SINE>
        }
        else if (TypeOfWave == 2)
        {
            //<TRIANGLE>
            x__ = 0;
            devideFactor = frequency / 1000;
            delta = 1 / (float)300; // 1/100 = 0.01  
            deltaBlockMax = (300 / devideFactor) / 300; 
            if (triangleDOWN == false) { x_ = x_ + delta; }
            if (triangleDOWN == true) { x_ = x_ - delta; }
            if (x_ >= deltaBlockMax)
            {
                triangleDOWN = true;
            }
            if (x_ <= 0) { x_ = 0; triangleDOWN = false; }
            x__ = x_ * devideFactor;
            varSinForGrapherLeftChannel = frequencyLeftChannel;
            varSinForGrapherRightChannel = frequencyRightChannel;
            return x__ * amplitudeWave;
            //</Triangle>
        }
        else if (TypeOfWave == 3)
        {
            //<SAW>
            if ((timeIndex_ * (frequency / 5) / sampleRate) >= 1)
            {
                timeIndex = 0;
            }
            varSinForGrapherLeftChannel = frequencyLeftChannel;
            varSinForGrapherRightChannel = frequencyRightChannel;
            return (timeIndex_ * frequency / sampleRate) * amplitudeWave;
            //</SAW>
        }
        else if (TypeOfWave == 4)
        {
            //<SQUARE>
            if ((timeIndex_ * (frequency / 5) / sampleRate) >= 1)
            {
                timeIndex = 0;
                squareWaveAmplitudeBool = !squareWaveAmplitudeBool;
                if (squareWaveAmplitudeBool == true) { squareWaveAmplitude = 1; }
                if (squareWaveAmplitudeBool == false) { squareWaveAmplitude = 0; }
            }
            varSinForGrapherLeftChannel = frequencyLeftChannel;
            varSinForGrapherRightChannel = frequencyRightChannel;
            return (squareWaveAmplitude) * amplitudeWave;
            //</SQUARE>
        }
        else if (TypeOfWave == 5)
        {
            //<TAN>
            varSinForGrapherLeftChannel = (frequencyLeftChannel / 20000) * 12;
            varSinForGrapherRightChannel = (frequencyRightChannel / 20000) * 12;
            return (Mathf.Tan(2 * Mathf.PI * timeIndex * frequency / sampleRate)) * amplitudeWave;
            //</TAN>
        }
        else
        {
            return 1;
        }        
    }


    public void resetFrequencyCorrectionByDefault()
    {
        sliderFrequencyCorrection.value = -5.0f;
    }


    //<for start/stop button>
    public void StartStop()
    {
        start_ = !start_;
    }


    //<for toggles>
    public void toggleFrequencyCorrection()
    {
        PlayerPrefs.SetFloat("FrequencyCorrection", sliderFrequencyCorrection.value);
    }


    public void toggleTypeOfWave()
    {
        if (toggleSineWave.isOn == true) { TypeOfWave = 1; }
        if (toggleTriangleWave.isOn == true) { TypeOfWave = 2; }
        if (toggleSawWave.isOn == true) { TypeOfWave = 3; }
        if (toggleSquareWave.isOn == true) { TypeOfWave = 4; }
        if (toggleTangentWave.isOn == true) { TypeOfWave = 5; }   
    }

 
    public void toggleMergeLeftAndRightChannelIsChanged()
    {
        if (toggleMergeLeftAndRightChannel.isOn == true)
        {
            mergePicture.SetActive(true);
            frequencyLeftChannel = 440.0f;
            frequencyRightChannel = 440.0f;
            sliderLeftChannel.value = 0.022f;
            sliderRighttChannel.value = 0.022f;
        }
        else
        {
            mergePicture.SetActive(false);
        }
    }
    //</for toggles>

    private IEnumerator PauseCoroutineForFlashSimple(float pauseTime_)
    {
        yield return new WaitForSeconds(pauseTime_); 

        TextRecStart.SetActive(!TextRecStart.activeSelf);
        StartCoroutine(PauseCoroutineForFlashSimple(0.5f));
    }

}


