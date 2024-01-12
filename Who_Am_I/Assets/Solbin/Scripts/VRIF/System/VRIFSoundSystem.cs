using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 소리
public enum PlayerClip
{
    walking,
    normalPoo,
    toiletPoo,
    toiletPush
}

// 도구 소리
public enum ToolClip
{
    nerfGunShoot,
    netSwing,
    dropBobber
}

// 물고기 소리
public enum FishClip
{
    fishBite, // 물고기 입질
    flap // 물고기 퍼덕
}

public class VRIFSoundSystem : MonoBehaviour
{
    public static VRIFSoundSystem Instance;

    [Header("Audio Source")]
    private AudioSource m_AudioSource = default;

    [Header("플레이어 사운드")]
    public AudioClip walkingClip; // 걷는 사운드
    public AudioClip normalPooClip; // 일반 배출 사운드
    public AudioClip toiletPooClip; // 화장실 배출 사운드
    public AudioClip toiletPushClip; // 화장실 물 내리는 사운드

    [Header("도구 사운드")]
    public AudioClip nerfGunShootClip; // 너프건 발사 사운드
    public AudioClip netSwingClip; // 곤충 채집망을 휘두르는 사운드
    public AudioClip dropBobberClip; // 찌가 물 속에 빠지는 사운드

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();    
    }

    #region 사운드 재생/정지
    /// <summary>
    /// 매개변수로 입력받은 소리를 재생하는 메서드: 플레이어 
    /// </summary>
    public void PlayerSound(PlayerClip clipName_, bool play_)
    {
        switch(clipName_)
        {
            case PlayerClip.walking:
                m_AudioSource.clip = walkingClip;
                break;

            case PlayerClip.normalPoo:
                m_AudioSource.clip = normalPooClip;
                break;

            case PlayerClip.toiletPoo:
                m_AudioSource.clip = toiletPooClip;
                break;

            case PlayerClip.toiletPush:
                m_AudioSource.clip = toiletPushClip;
                break;
        }

        if (play_) { m_AudioSource.PlayOneShot(m_AudioSource.clip); } // play_가 참이면 해당 사운드 중첩 재생
        else { m_AudioSource.Stop(); } // 해당 사운드 정지
    }

    /// <summary>
    /// 매개변수로 입력받은 소리를 재생하는 메서드: 플레이어 도구
    /// </summary>
    public void ToolSound(ToolClip clipName_, bool play_)
    {
        //switch (clipName_)
        //{

        //}

        if (play_) { m_AudioSource.Play(); } // play_가 참이면 해당 사운드 재생
        else { m_AudioSource.Stop(); } // 해당 사운드 정지
    }

    /// <summary>
    /// 매개변수로 입력받은 소리를 재생하는 메서드: 물고기
    /// </summary>
    public void FishSound(FishClip clipName_, bool play_)
    {
        //switch (clipName_)
        //{

        //}

        if (play_) { m_AudioSource.Play(); } // play_가 참이면 해당 사운드 재생
        else { m_AudioSource.Stop(); } // 해당 사운드 정지
    }
    #endregion

    /// <summary>
    /// 매개변수로 전달받은 오디오 클립이 재생 중인지 알아내는 메서드
    /// </summary>
    public void CheckPlaying()
    {

    }
}
