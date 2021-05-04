using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController: MonoBehaviour
{
    public static float BGMvol=-25;
    public static float SEvol=-25;
    [SerializeField] Slider BGMSlider=null;
    [SerializeField] Slider SESlider=null;
    [SerializeField] private AudioMixer audioMixer = null;


    [SerializeField] AudioSource gameStart=null;
    [SerializeField] AudioSource gameFinish= null;
    [SerializeField] AudioSource moveTurnStart= null;
    [SerializeField] AudioSource myTurn= null;
    [SerializeField] AudioSource putcandy= null;
    [SerializeField] AudioSource turnLimitOver = null;
    [SerializeField] AudioSource destroyEffect= null;
    [SerializeField] GameObject timer= null;
    [SerializeField] AudioSource click= null;
    [SerializeField] AudioSource click1= null;    


    private void Awake()
    {
        BGMSlider.value = BGMvol;
        SESlider.value = SEvol;
    }


    public void SetBGM(float volume)
    {
        BGMvol = volume;
        Debug.Log(BGMvol);
        audioMixer.SetFloat("BGMVol", volume);
    }

    public void SetSE(float volume)
    {
        SEvol = volume;
        Debug.Log(SEvol);
        audioMixer.SetFloat("SEVol", volume);
    }



    public void GameStartSE()
    {
        gameStart.PlayOneShot(gameStart.clip);
    }
    public void GameFinishSE()
    {
        gameFinish.PlayOneShot(gameFinish.clip);
    }
    public void MoveTurnStartSE()
    {
        moveTurnStart.PlayOneShot(moveTurnStart.clip);
    }

    public void MyTurnSE()
    {
        myTurn.PlayOneShot(myTurn.clip);
    }

    public void TurnLimitOverSE()
    {
        turnLimitOver.PlayOneShot(turnLimitOver.clip);
    }
    public void DestroyEffectSE()
    {
        destroyEffect.PlayOneShot(destroyEffect.clip);
    }
    public void TimerSEOn()
    {
        timer.SetActive(true);
    }
    public void TimerSEOff()
    {
        timer.SetActive(false);
    }

    public void PutCandySE()
    {
        putcandy.PlayOneShot(putcandy.clip);
    }
    public void ClickSE()
    {
        click.PlayOneShot(click.clip);
    }
    public void Click1SE()
    {
        click1.PlayOneShot(click1.clip);
    }


}
