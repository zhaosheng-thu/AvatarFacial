using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Oculus.Movement.Tracking;

public class FileManager : MonoBehaviour
{

    public OVRController OVRController;
    public FileSelector FileSelector;
    private string filepathFace, filepathEyes, filepathBody, filepathVoice;

    [Tooltip("���Ե�����")]
    public string nameOfExp;
    [Tooltip("ģʽѡ��")]
    public TypeOfExp typeOfExp;
    [Tooltip("����ѡ��")]
    public VRVideoPlayer.mp4filepath videoType;
    private void Awake()
    {
        
        DateTime currentDate = DateTime.Now;
        int currentMonth = currentDate.Month;
        // ��ȡ��ǰ��
        int currentDay = currentDate.Day;
        int currentHour = currentDate.Hour;
        int currentMin = currentDate.Minute;
        int currentSec = currentDate.Second;
        string timeState = string.Format("MM{0}DD{1}_{2}_{3}_{4}", currentMonth, currentDay, currentHour, currentMin, currentSec);
        Debug.Log("time" + timeState);
        //chatģʽ����Ҫ���Ƿ���Ҫ��¼��Ҫ�Ļ�Ҫ�ع�filepath��ֻ�ǻطŵĻ�choose����
        if ((int)typeOfExp == 2 && OVRController.isFromFile == true)
        {
            filepathFace = FileSelector.filepathFace;
            filepathEyes = FileSelector.filepathEyes;
            filepathBody = FileSelector.filepathBody;
            filepathVoice = FileSelector.filepathWav;
        }

        //����ģʽ�ɼ����Թۿ�ʱ�ı������chatģʽ��������ʱ�ع�filepath
        else
        {
            //"E:\Unity workspce\Avatar environment\Assets"
            if ((int)typeOfExp == 2)
            {
                BlendshapeModifier blendshapeModifier = this.GetComponent<BlendshapeModifier>();
                nameOfExp = blendshapeModifier.GetGlobalMultiplier().ToString() + "times_" + nameOfExp;
                filepathFace = Path.Combine(Application.dataPath, "File/VRchat/Face/face_" + nameOfExp + "_" + videoType + "_" + timeState + ".txt");
                filepathEyes = Path.Combine(Application.dataPath, "File/VRchat/Eye/eye_" + nameOfExp + "_" + videoType + "_" + timeState + ".txt");
                filepathBody = Path.Combine(Application.dataPath, "File/VRchat/Body/body_" + nameOfExp + "_" + videoType + "_" + timeState + ".txt");
                filepathVoice = Path.Combine(Application.dataPath, "File/VRchat/Voice/voice_" + nameOfExp + "_" + videoType + "_" + timeState + ".wav");
            }
            else
            {
                filepathFace = Path.Combine(Application.dataPath, "File/Videoplayer/Face/face_" + nameOfExp + "_" + videoType + "_" + timeState + ".txt");
                filepathEyes = Path.Combine(Application.dataPath, "File/Videoplayer/Eye/eye_" + nameOfExp + "_" + videoType + "_" + timeState + ".txt");
                filepathBody = Path.Combine(Application.dataPath, "File/Videoplayer/Body/body_" + nameOfExp + "_" + videoType + "_" + timeState + ".txt");
                filepathVoice = Path.Combine(Application.dataPath, "File/Videoplayer/Voice/voice_" + nameOfExp + "_" + videoType + "_" + timeState + ".wav");
            }

        }
        OVRController.filePathFace = filepathFace;
        OVRController.filePathEyes = filepathEyes;
        OVRController.filePathBody = filepathBody;
        OVRController.filePathVoice = filepathVoice;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum TypeOfExp
    {
        Invalid = -1,
        Videoplayer = 1,
        Chat = 2
    }

}
