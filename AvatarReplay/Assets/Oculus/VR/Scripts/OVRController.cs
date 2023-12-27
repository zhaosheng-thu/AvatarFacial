using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class OVRController : MonoBehaviour
{
    /// <summary>
    /// ��Ҫ���س�ʼ���ı���
    /// </summary>
    public OVRFaceExpressions ovrFaceExpressions;
    public OVRBody ovrBody;
    public OVREyeGaze ovrEyeGazeLeft;
    public OVREyeGaze ovrEyeGazeRight;
    // AudioSource���
    public AudioSource audioSource;
    private AudioClip recordedClip, playClip;
    private const int bufferSize = 8192; // ��������С
    // �ļ�����·��
    [HideInInspector]
    public string filePathFace;
    [HideInInspector]
    public string filePathEyes;
    [HideInInspector]
    public string filePathBody;
    [HideInInspector]
    public string filePathVoice;
    private List<string> faceList;
    private List<string> eyeList;
    private List<string> bodyList;
    //�Ƿ�Ϊ�ط�
    public bool isFromFile = false;
    private bool isFirstPlay = true;

    /// <summary>
    /// FaceExpressions����60���weights�Ķ�Ӧ����
    /// </summary>
    private readonly OVRFaceExpressions.FaceExpression[] interstedFaceExpressions = new OVRFaceExpressions.FaceExpression[] {
        OVRFaceExpressions.FaceExpression.BrowLowererL,
        OVRFaceExpressions.FaceExpression.BrowLowererR,
        OVRFaceExpressions.FaceExpression.CheekPuffL,
        OVRFaceExpressions.FaceExpression.CheekPuffR,
        OVRFaceExpressions.FaceExpression.CheekRaiserL,
        OVRFaceExpressions.FaceExpression.CheekRaiserR,
        OVRFaceExpressions.FaceExpression.CheekSuckL,
        OVRFaceExpressions.FaceExpression.CheekSuckR,
        OVRFaceExpressions.FaceExpression.ChinRaiserB,
        OVRFaceExpressions.FaceExpression.ChinRaiserT,
        OVRFaceExpressions.FaceExpression.DimplerL,
        OVRFaceExpressions.FaceExpression.DimplerR,
        OVRFaceExpressions.FaceExpression.EyesClosedL,
        OVRFaceExpressions.FaceExpression.EyesClosedR,
        OVRFaceExpressions.FaceExpression.EyesLookDownL,
        OVRFaceExpressions.FaceExpression.EyesLookDownR,
        OVRFaceExpressions.FaceExpression.EyesLookLeftL,
        OVRFaceExpressions.FaceExpression.EyesLookLeftR,
        OVRFaceExpressions.FaceExpression.EyesLookRightL,
        OVRFaceExpressions.FaceExpression.EyesLookRightR,
        OVRFaceExpressions.FaceExpression.EyesLookUpL,
        OVRFaceExpressions.FaceExpression.EyesLookUpR,
        OVRFaceExpressions.FaceExpression.InnerBrowRaiserL,
        OVRFaceExpressions.FaceExpression.InnerBrowRaiserR,
        OVRFaceExpressions.FaceExpression.JawDrop,
        OVRFaceExpressions.FaceExpression.JawSidewaysLeft,
        OVRFaceExpressions.FaceExpression.JawSidewaysRight,
        OVRFaceExpressions.FaceExpression.JawThrust,
        OVRFaceExpressions.FaceExpression.LidTightenerL,
        OVRFaceExpressions.FaceExpression.LidTightenerR,
        OVRFaceExpressions.FaceExpression.LipCornerDepressorL,
        OVRFaceExpressions.FaceExpression.LipCornerDepressorR,
        OVRFaceExpressions.FaceExpression.LipCornerPullerL,
        OVRFaceExpressions.FaceExpression.LipCornerPullerR,
        OVRFaceExpressions.FaceExpression.LipFunnelerLB,
        OVRFaceExpressions.FaceExpression.LipFunnelerLT,
        OVRFaceExpressions.FaceExpression.LipFunnelerRB,
        OVRFaceExpressions.FaceExpression.LipFunnelerRT,
        OVRFaceExpressions.FaceExpression.LipPressorL,
        OVRFaceExpressions.FaceExpression.LipPressorR,
        OVRFaceExpressions.FaceExpression.LipPuckerL,
        OVRFaceExpressions.FaceExpression.LipPuckerR,
        OVRFaceExpressions.FaceExpression.LipStretcherL,
        OVRFaceExpressions.FaceExpression.LipStretcherR,
        OVRFaceExpressions.FaceExpression.LipSuckLB,
        OVRFaceExpressions.FaceExpression.LipSuckLT,
        OVRFaceExpressions.FaceExpression.LipSuckRB,
        OVRFaceExpressions.FaceExpression.LipSuckRT,
        OVRFaceExpressions.FaceExpression.LipTightenerL,
        OVRFaceExpressions.FaceExpression.LipTightenerR,
        OVRFaceExpressions.FaceExpression.LipsToward,
        OVRFaceExpressions.FaceExpression.LowerLipDepressorL,
        OVRFaceExpressions.FaceExpression.LowerLipDepressorR,
        OVRFaceExpressions.FaceExpression.MouthLeft,
        OVRFaceExpressions.FaceExpression.MouthRight,
        OVRFaceExpressions.FaceExpression.NoseWrinklerL,
        OVRFaceExpressions.FaceExpression.NoseWrinklerR,
        OVRFaceExpressions.FaceExpression.OuterBrowRaiserL,
        OVRFaceExpressions.FaceExpression.OuterBrowRaiserR,
        OVRFaceExpressions.FaceExpression.UpperLidRaiserL,
        OVRFaceExpressions.FaceExpression.UpperLidRaiserR,
        OVRFaceExpressions.FaceExpression.UpperLipRaiserL,
        OVRFaceExpressions.FaceExpression.UpperLipRaiserR,

    };
    /// <summary>
    /// �۾�ID
    /// </summary>
    private readonly OVREyeGaze.EyeId[] eyeIds = new OVREyeGaze.EyeId[]{
        OVREyeGaze.EyeId.Left,
        OVREyeGaze.EyeId.Right,
    };
    private string[] stringArrayFace;
    private string[] stringArrayEye;
    private string[] stringArrayBody;
    string faceExpressionWeightStr;
    string eyeGazeStr;
    string bodyStr;

    private void Awake()
    {

        if (isFromFile)
        {
            // ��ȡ.wav�ļ�������ΪAudioClip
            playClip = LoadWavFile(filePathVoice);

        }

    }

    // Start is called before the first frame update
    void Start()
    {

        faceList = new List<string>();
        eyeList = new List<string>();
        bodyList = new List<string>();
        //��OVRFaceExpression�е���
        //indexOfFrame = 0;
        if (isFromFile)
        {
            try
            {
                stringArrayFace = File.ReadAllLines(filePathFace);
                stringArrayEye = File.ReadAllLines(filePathEyes);
                stringArrayBody = File.ReadAllLines(filePathBody);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("FileNotFoundException");
            }


        }

    }

    /// <summary>
    /// Fix�й̶�ʱ��ν�����copy��list�У���������д��
    /// </summary>
    private void FixedUpdate()
    {

        if (!isFromFile && faceExpressionWeightStr != null)
        {
            faceList.Add(faceExpressionWeightStr);
            eyeList.Add(eyeGazeStr);
            bodyList.Add(bodyStr);
        }

    }

    // Update is called once per frame
    void Update()
    {

        //�ɼ�
        if (ovrFaceExpressions.ValidExpressions && !isFromFile)
        {
            GetFaceExpression(interstedFaceExpressions, out faceExpressionWeightStr);
            //FileWriter(faceExpressionWeightStr,filePathFace);
            GetEyeGaze(out eyeGazeStr);
            //FileWriter(eyeGazeStr, filePathEyes);
            GetBodyState(out bodyStr);
            //FileWriter(bodyStr, filePathBody);
            // ��¼�����ݴ洢��������
            if (!Microphone.IsRecording(null))
            {
                // ��ʼ��¼���豸����ʼ¼��
                recordedClip = Microphone.Start(null, false, 240, 44100);
            }
            else
            {
                float[] buffer = new float[bufferSize];
                int position = Microphone.GetPosition(null);
                recordedClip.GetData(buffer, position);
                //Debug.Log("Start record" + Time.time);
            }
        }

        if (isFromFile)
        {

            // ������Ƶ �ֶ�����
            if (ovrFaceExpressions.getIndexOfFrame() > stringArrayFace.Length - 2 && !isFirstPlay)
            {
                audioSource.Play();
            }

            //Debug.Log("frame" + ovrFaceExpressions.getIndexOfFrame());
            if (isFirstPlay && ovrFaceExpressions.getIndexOfFrame() > 4)
            {
                PlayAudioClip();

                isFirstPlay = false;
            }
        }

    }


    //destroy
    void OnDestroy()
    {
        if (!isFromFile)
        {
            StopRecordVoice();
            for (int i = 0; i < Math.Min(Math.Min(faceList.Count, eyeList.Count), bodyList.Count); i++)
            {
                FileWriter(faceList[i], filePathFace);
                FileWriter(eyeList[i], filePathEyes);
                FileWriter(bodyList[i], filePathBody);
            }
        }

    }


    /// <summary>
    /// �������ݻ�ȡ
    /// </summary>
    /// <param name="interstedFaceExpressions"></param>
    /// <param name="faceExpressionWeight"></param>
    /// <returns></returns>
    public bool GetFaceExpression(OVRFaceExpressions.FaceExpression[] interstedFaceExpressions, out string faceExpressionWeight)
    {
        faceExpressionWeight = null;
        foreach (OVRFaceExpressions.FaceExpression value in interstedFaceExpressions)
        {
            // Returns weight of expression ranged between 0.0 to 100.0.
            if (ovrFaceExpressions.TryGetFaceExpressionWeight(value, out float weight))
            {
                string expressionWeight = "(" + value.ToString() + "," + ((float)Math.Round(weight, 6)).ToString() + ")";
                if (!value.Equals(OVRFaceExpressions.FaceExpression.UpperLipRaiserR))
                {
                    expressionWeight += ';';
                }
                faceExpressionWeight += expressionWeight;
            }
        }
        //Debug.Log("FaceExpression" + faceExpressionWeight);
        return true;
    }

    /// <summary>
    /// ���������λ�úͿ����ȡ
    /// </summary>
    /// <param name="EyeGaze"></param>
    /// <returns></returns>
    public bool GetEyeGaze(out string EyeGaze)
    {
        EyeGaze = null;
        if (ovrEyeGazeLeft.TryGetEyeGazeState(eyeIds[0], out Vector3 position, out Quaternion rotation))
        {
            EyeGaze = position.ToString() + ";" + rotation.ToString();
        }
        if (ovrEyeGazeRight.TryGetEyeGazeState(eyeIds[1], out position, out rotation))
        {
            string tmpGaze = "/" + position.ToString() + ";" + rotation.ToString();
            EyeGaze += tmpGaze;
        }
        //Debug.Log("EyeGaze" + EyeGaze);
        return true;
    }

    /// <summary>
    /// �õ�����׷�ٵ�pos��rot
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public bool GetBodyState(out string body)
    {
        body = null;
        if (ovrBody.TryGetBodyState(out List<OVRBody.PosAndRot> pos))
        {
            int count = 0;
            foreach (OVRBody.PosAndRot posAndRot in pos)
            {
                if (count != 0) body += "/";
                count++;
                string tmpBody = String.Format("{0};{1}", posAndRot.vector3F.ToString(), posAndRot.Quatf.ToString());
                body += tmpBody;
            }
        }
        //Debug.Log("BodyState");
        return true;
    }

    /// <summary>
    /// �沿_currentFaceState�ı�
    /// </summary>
    /// <param name="_currentFaceState"></param>
    /// <param name="weight"></param>
    /// <param name="faceExpression"></param>
    /// <returns></returns>
    public bool TrtSetFaceExpression(int indexOfFrame, ref OVRPlugin.FaceState _currentFaceState)
    {
        string fileBuffer = stringArrayFace[indexOfFrame];
        string[] singleFaceExpression = fileBuffer.Split(';');
        int count = 0;
        foreach (string value in singleFaceExpression)
        {
            OVRFaceExpressions.FaceExpression faceExpression = interstedFaceExpressions[count++];
            float weight = float.Parse(value.Replace("(", "").Replace(")", "").Split(",")[1]);
            if ((int)faceExpression < _currentFaceState.ExpressionWeights.Length)
            {
                _currentFaceState.ExpressionWeights[(int)faceExpression] = weight;
            }
            else
                return false;
        }
        return true;
    }

    /// <summary>
    /// �۲�׷�ٵĸı�
    /// </summary>
    /// <param name="indexOfFrame"></param>
    /// <param name="ApplyPosition"></param>
    /// <param name="ApplyRotation"></param>
    /// <param name="vector"></param>
    /// <param name="quaternion"></param>
    /// <param name="eye"></param>
    /// <returns></returns>
    public bool TrySetEyeGaze(int indexOfFrame, bool ApplyPosition, bool ApplyRotation, ref Vector3 vector, ref Quaternion quaternion, OVREyeGaze.EyeId eye)
    {
        string fileBuffer = stringArrayEye[indexOfFrame++];
        string[] singleEyeState = fileBuffer.Split('/');
        //�õ���/���۾��Ĵ洢����
        string thisEyeState = singleEyeState[(int)eye];
        string position = thisEyeState.Split(";")[0];
        string rotation = thisEyeState.Split(";")[1];
        float xPosition = float.Parse(position.Replace("(", "").Replace(")", "").Split(",")[0]);
        float yPosition = float.Parse(position.Replace("(", "").Replace(")", "").Split(",")[1]);
        float zPosition = float.Parse(position.Replace("(", "").Replace(")", "").Split(",")[2]);
        float xRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[0]);
        float yRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[1]);
        float zRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[2]);
        float wRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[3]);
        //���applyΪ�������
        if (ApplyPosition)
        {
            vector = new Vector3(xPosition, yPosition, zPosition);
        }
        if (ApplyRotation)
        {
            quaternion = new Quaternion(xRotation, yRotation, zRotation, wRotation);
        }
        return true;
    }

    /// <summary>
    /// ����pos��rot�ĸı�
    /// </summary>
    /// <param name="indexOfFrame"></param>
    /// <param name="_bodyState"></param>
    /// <returns></returns>
    public bool TrySetBody(int indexOfFrame, ref OVRPlugin.BodyState _bodyState)
    {
        string fileBuffer = stringArrayBody[indexOfFrame++];
        string[] singleBodyState = fileBuffer.Split('/');
        for (var i = 0; i < _bodyState.JointLocations.Length; i++)
        {
            //���Ȳ��ȴ洢������������Ҫ����Ĳ�ͬ
            if (_bodyState.JointLocations.Length != singleBodyState.Length)
                return false;
            else
            {
                string position = singleBodyState[i].Split(";")[0];
                string rotation = singleBodyState[i].Split(";")[1];
                float xPosition = float.Parse(position.Replace("(", "").Replace(")", "").Split(",")[0]);
                float yPosition = float.Parse(position.Replace("(", "").Replace(")", "").Split(",")[1]);
                float zPosition = float.Parse(position.Replace("(", "").Replace(")", "").Split(",")[2]);
                float xRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[0]);
                float yRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[1]);
                float zRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[2]);
                float wRotation = float.Parse(rotation.Replace("(", "").Replace(")", "").Split(",")[3]);

                if (_bodyState.JointLocations[i].OrientationValid)
                {
                    OVRPlugin.Quatf quatf = new() { w = wRotation, x = xRotation, y = yRotation, z = zRotation };
                    _bodyState.JointLocations[i].Pose.Orientation = quatf;
                }
                if (_bodyState.JointLocations[i].PositionValid)
                {
                    OVRPlugin.Vector3f vector3F = new() { x = xPosition, y = yPosition, z = zPosition };
                    _bodyState.JointLocations[i].Pose.Position = vector3F;
                }
            }

        }
        return true;

    }

    /// <summary>
    /// �ļ�д��
    /// </summary>
    /// <param name="str"></param>
    /// <param name="filePath"></param>
    public void FileWriter(string str, string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }

        // ʹ�� StreamWriter ���ļ�����д���ı�
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(str);
            
        }
    }

    /// <summary>
    /// .wav�ļ�д��
    /// </summary>
    /// <param name="samples"></param>
    /// <param name="channels"></param>
    /// <param name="sampleRate"></param>
    /// <returns></returns>
    public byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        int sampleCount = samples.Length;
        int subchunk2Size = sampleCount * channels * sizeof(short);
        int chunkSize = 36 + subchunk2Size;

        using (MemoryStream stream = new MemoryStream())
        {
            // д��.wav�ļ�ͷ
            stream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            stream.Write(System.BitConverter.GetBytes(chunkSize), 0, 4);
            stream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);
            stream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
            stream.Write(System.BitConverter.GetBytes(16), 0, 4); // subchunk1Size
            stream.Write(System.BitConverter.GetBytes((short)1), 0, 2); // audioFormat (PCM)
            stream.Write(System.BitConverter.GetBytes((short)channels), 0, 2); // numChannels
            stream.Write(System.BitConverter.GetBytes(sampleRate), 0, 4); // sampleRate
            stream.Write(System.BitConverter.GetBytes(sampleRate * channels * sizeof(short)), 0, 4); // byteRate
            stream.Write(System.BitConverter.GetBytes((short)(channels * sizeof(short))), 0, 2); // blockAlign
            stream.Write(System.BitConverter.GetBytes((short)16), 0, 2); // bitsPerSample
            stream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
            stream.Write(System.BitConverter.GetBytes(subchunk2Size), 0, 4);

            // д����Ƶ����
            foreach (float sample in samples)
            {
                short sampleValue = (short)(sample * 32767);
                stream.Write(System.BitConverter.GetBytes(sampleValue), 0, 2);
            }
            return stream.ToArray();
        }
    }

    /// <summary>
    /// ֹͣ������¼
    /// </summary>
    private void StopRecordVoice()
    {
        Microphone.End(null); // ֹͣ¼��

        // ��¼������ת��Ϊ�ֽ�����
        float[] samples = new float[recordedClip.samples];
        recordedClip.GetData(samples, 0);
        // ����һ���ֽ��������ڴ洢.wav�ļ�ͷ����Ƶ����
        byte[] wavData = ConvertToWav(samples, recordedClip.channels, recordedClip.frequency);
        // ����Ϊ.wav�ļ�
        File.WriteAllBytes(filePathVoice, wavData);
        Debug.Log("Saved wav file: " + filePathVoice);
    }

    /// <summary>
    /// wav�ļ�����
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private AudioClip LoadWavFile(string filePath)
    {
        // ��ȡ��Ƶ�ļ�����
        byte[] audioData = System.IO.File.ReadAllBytes(filePath);
        // ���ֽ�����ת��Ϊ16λ��������������
        short[] audioSamples = new short[audioData.Length / 2];
        Buffer.BlockCopy(audioData, 0, audioSamples, 0, audioData.Length);
        // �����������������һ��Ϊ����������
        float[] normalizedSamples = new float[audioSamples.Length];
        const float conversionFactor = 1.0f / 32768.0f; // ��һ������
        for (int i = 0; i < audioSamples.Length; i++)
        {
            normalizedSamples[i] = audioSamples[i] * conversionFactor;
        }
        // ����һ���µ�AudioClip��������Ƶ����
        //���һ����������ֵ��ָʾ�Ƿ�AudioClip����Ϊ��ʽ���š��������Ϊtrue�����������Ƶ���ڼ���ʱ���ţ������ڽϴ����Ƶ�ļ���
        AudioClip audioClip = AudioClip.Create("wavClip", normalizedSamples.Length, 1, 44100, false);
        audioClip.SetData(normalizedSamples, 0);

        return audioClip;
    }

    /// <summary>
    /// wav�ļ�����
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayAudioClip()
    {
        // ����AudioSource����Ƶ����
        audioSource.clip = playClip;
        // ������Ƶ
        audioSource.Play();
    }


    ///
    private IEnumerator LoadWavFile(string filePath, Action<AudioClip> callback)
    {
        string fullPath = filePath;
        using (UnityWebRequest www = UnityWebRequest.Get(fullPath))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load audio file: " + www.error);
                callback?.Invoke(null);
                yield break;
            }

            byte[] audioData = www.downloadHandler.data;

            short[] audioSamples = new short[audioData.Length / 2];
            Buffer.BlockCopy(audioData, 0, audioSamples, 0, audioData.Length);

            float[] normalizedSamples = new float[audioSamples.Length];
            const float conversionFactor = 1.0f / 32768.0f;
            for (int i = 0; i < audioSamples.Length; i++)
            {
                normalizedSamples[i] = audioSamples[i] * conversionFactor;
            }

            const float segmentDuration = 1f; // ÿ�εĳ���ʱ�䣨�룩
            int numSegments = Mathf.CeilToInt(normalizedSamples.Length / (44100f * segmentDuration));
            int samplesPerSegment = Mathf.CeilToInt(normalizedSamples.Length / numSegments);

            int totalSamples = numSegments * samplesPerSegment; // ��������

            AudioClip audioClip = AudioClip.Create("wavClip", totalSamples, 1, 44100, false);

            for (int i = 0; i < numSegments; i++)
            {
                int startIndex = i * samplesPerSegment;
                float[] segmentSamples = new float[samplesPerSegment];
                Array.Copy(normalizedSamples, startIndex, segmentSamples, 0, samplesPerSegment);
                audioClip.SetData(segmentSamples, startIndex);

                if (i == 0)
                {
                    //AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
                else
                {
                    yield return new WaitForSeconds(segmentDuration);
                }
            }

            callback?.Invoke(audioClip);
        }
    }


    private void OnAudioLoaded(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            Debug.Log("�ڻص������в���");
            audioSource.clip = audioClip;
            // ������Ƶ
            audioSource.Play();
        }
        else
        {
            // ����ʧ��ʱ�Ĵ���
            Debug.LogError("Failed to load audio clip");
        }
    }

}


