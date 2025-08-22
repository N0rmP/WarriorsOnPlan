using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class audioComponent {
    private AudioSource sourceBGM;
    private AudioSource sourceSE;

    private AudioMixer mixer;
    private AudioMixerGroup[] arrAMG;

    // convertNormToDecibel converts normalized value to decibel, please ensure parNorm is float value between 0.0001~1
    // the limitation of parNorm range and (* 20) are both because volumes in AudioMixer can be in range of -80 ~ 0
    public static float convertNormToDecibel(float parNorm) {
        return Mathf.Log10(parNorm) * 20;
    }

    public audioComponent() {
        mixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        sourceBGM = gameManager.GM.gameObject.AddComponent<AudioSource>();
        sourceBGM.loop = true;
        sourceSE = gameManager.GM.gameObject.AddComponent<AudioSource>();

        // set AudioMixerGroup
        arrAMG = mixer.FindMatchingGroups("Master");
        mixer.SetFloat("pitchBGM", 1f);        
        foreach (AudioMixerGroup amg in arrAMG) {
            switch (amg.name){
                case "Master":
                    // 난 복학생이야... 안녕?...
                    break;
                case "BGM":
                    sourceBGM.outputAudioMixerGroup = amg;
                    break;
                case "SE":
                    sourceSE.outputAudioMixerGroup = amg;
                    break;
                default:
                    Debug.Log("audioComponent.Awake failed to find adequate audioSource for AudioMixerGroup - AudioMixerGroup : " + amg.name);
                    break;
            }
        }

        // ★ 테스트용 파이널뽈 재생, 이 코드뿐 아니라 파이널뽈 mp3 파일 자체도 절대 실제 출시 빌드에 포함되지 않게 조심하기
        // playBGM(Resources.Load<AudioClip>("Audio/Fall Guys-Final Fall"));
    }

    #region Source & Mixer
    public void setVolume(float parVolumerMaster = 0.7f, float parVolumerBGM = 0.7f, float parVolumeSE = 0.7f) {
        mixer.SetFloat("volumeMaster", convertNormToDecibel(parVolumerMaster));
        mixer.SetFloat("volumeBGM", convertNormToDecibel(parVolumerBGM));
        mixer.SetFloat("volumeSE", convertNormToDecibel(parVolumeSE));
    }

    // playBGM also set ptichBGM to 1f, you should set it again to change BGM speed
    public void playBGM(AudioClip parClipBGM) {
        sourceBGM.clip = parClipBGM;
        mixer.SetFloat("pitchBGM", 1f);
        sourceBGM.Play();
    }

    public void playSE(AudioClip parClipSE) {
        sourceSE.PlayOneShot(parClipSE);
    }

    public void setPitchBGM(float parPitch) {
        mixer.SetFloat("pitchBGM", parPitch);
    }
    #endregion Source & Mixer

    #region test
    public void testMixerGroupStatus() {
        float tempFloat;
        StringBuilder tempSB = new StringBuilder("audioComponent.testVolume\nvolumeMaster : ");

        mixer.GetFloat("volumeMaster", out tempFloat);
        tempSB.Append(tempFloat.ToString());
        tempSB.Append("\nvolumeBGM : ");
        mixer.GetFloat("volumeBGM", out tempFloat);
        tempSB.Append(tempFloat.ToString());
        tempSB.Append("\nvolumeBGM : ");
        mixer.GetFloat("volumeSE", out tempFloat);
        tempSB.Append(tempFloat.ToString());
        tempSB.Append("\npitchBGM : ");
        mixer.GetFloat("pitchBGM", out tempFloat);
        tempSB.Append(tempFloat.ToString());

        Debug.Log(tempSB.ToString());
    }
    #endregion test
}
