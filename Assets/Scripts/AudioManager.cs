//// 反编译自程序集： Assembly-CSharp.dll

using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public MenuToggle soundToggle;

    public MenuToggle musicToggle;

    public AudioSource _mainBackgroundMusic;

    public AudioSource _soundSource;

    public AudioSource _enemySource;

    public AudioClip _fistHitSound;

    public AudioClip _fistMissSound;

    public AudioClip _swordHitSound;

    public AudioClip _swordMissSound;

    public AudioClip _daggerSound;

    public AudioClip _axeSound;

    public AudioClip _spearSound;

    public AudioClip _bodyHit;

    public AudioClip _coinCollectSound;

    public AudioClip _startGongSound;


    public AudioClip _startFatalitySound;

    public AudioClip _endFatalitySound;

    public AudioClip _startingSound;

    public AudioClip _gotItemBeltSound;

    public AudioClip _gotItemCoinSound;

    public AudioClip _gotItemItemSound;


    public AudioClip _itemScrollSound;

    public AudioClip _tapSound;

    public AudioClip[] _hitEnemy;
    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }

    public void Awake()
    {
        AudioManager.instance = this;
        MusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SoundVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("SoundVolume", 1f));
        ApplyVolume();
    }
    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        ApplyVolume();
    }
    public void SetSoundVolume(float value)
    {
        SoundVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        ApplyVolume();
    }
    private void ApplyVolume()
    {
        if (_mainBackgroundMusic != null) _mainBackgroundMusic.volume = MusicVolume;
        if (_soundSource != null) _soundSource.volume = SoundVolume;
        if (_enemySource != null) _enemySource.volume = SoundVolume;
    }

    public void Start()
    {
      
    }

    public void Update()
    {
        if (this.soundToggle.isOn)
        {
            if (_soundSource.mute)
            {
                _soundSource.mute = false;
            }
            if (_enemySource.mute)
            {
                _enemySource.mute = false;
            }
        } else
        {
            if (!_soundSource.mute)
            {
                _soundSource.mute = true;
            }
            if (!_enemySource.mute)
            {
                _enemySource.mute = true;
            }
        }
        if (this.musicToggle.isOn)
        {
            if(_mainBackgroundMusic.mute)
            _mainBackgroundMusic.mute = false;
        } else
        {
            if (!_mainBackgroundMusic.mute)
                _mainBackgroundMusic.mute = true;
        }

    }

    public void OnGameStart()
    {
        if (SceneManager.instance.firstStart)
        {
            SceneManager.instance.firstStart = false;
        }
        else
        {
            this.StartGongSound();
        }
    }

    public void OnGameOver()
    {
        this.StartGongSound();
    }

    public void HitSound(bool isFist, bool isMiss, bool isCoin, string currentIdWeapon)
    {
       
        if (!SceneManager.instance.gameStarted)
        {
            return;
        }
        switch (currentIdWeapon)
        {
            case "Fist":
                if (isCoin)
                {

                    _soundSource.PlayOneShot(_fistMissSound);
                }
                else if (isMiss)
                {

                    _soundSource.PlayOneShot(_fistMissSound);
                }
                else
                {

                    _soundSource.PlayOneShot(_fistHitSound);
                }
                break;
            case "Dagger":
                if (isCoin)
                {

                    _soundSource.PlayOneShot(_daggerSound);
                }
                else if (isMiss)
                {

                    _soundSource.PlayOneShot(_daggerSound);
                }
                else
                {

                    _soundSource.PlayOneShot(_daggerSound);
                    _enemySource.PlayOneShot(_bodyHit);
                }
         
                break;
            case "Axe":
                if (isCoin)
                {

                    _soundSource.PlayOneShot(_axeSound);
                }
                else if (isMiss)
                {

                    _soundSource.PlayOneShot(_axeSound);
                }
                else
                {

                    _soundSource.PlayOneShot(_axeSound);
                    _enemySource.PlayOneShot(_bodyHit);
                }
            
                break;
            case "Spear":
                if (isCoin)
                {

                    _soundSource.PlayOneShot(_spearSound);
                }
                else if (isMiss)
                {

                    _soundSource.PlayOneShot(_spearSound);
                }
                else
                {

                    _soundSource.PlayOneShot(_spearSound);
                    _enemySource.PlayOneShot(_bodyHit);
                }
       
                break;
            case "Sword1h":
                if (isCoin)
                {

                    _soundSource.PlayOneShot(_swordMissSound);
                }
                else if (isMiss)
                {

                    _soundSource.PlayOneShot(_swordMissSound);
                }
                else
                {

                    _soundSource.PlayOneShot(_swordMissSound);
                    _enemySource.PlayOneShot(_bodyHit);
                }
                break;
            case "Sword2h":
                if (isCoin)
                {

                    _soundSource.PlayOneShot(_swordMissSound);
                }
                else if (isMiss)
                {

                    _soundSource.PlayOneShot(_swordMissSound);
                }
                else
                {

                    _soundSource.PlayOneShot(_swordMissSound);
                    _enemySource.PlayOneShot(_bodyHit);
                }
                break;
        }
        //if (isFist)
        //{
        //    if (isCoin)
        //    {
            
        //        _soundSource.PlayOneShot(_fistMissSound);
        //    }
        //    else if (isMiss)
        //    {
          
        //        _soundSource.PlayOneShot(_fistMissSound);
        //    }
        //    else
        //    {
      
        //        _soundSource.PlayOneShot(_fistHitSound);
        //    }
        //}
        //else if (isCoin)
        //{
          
        //    _soundSource.PlayOneShot(_swordMissSound);
        //}
        //else if (isMiss)
        //{
        
        //    _soundSource.PlayOneShot(_swordMissSound);
        //}
        //else
        //{
        
        //    _soundSource.PlayOneShot(_swordHitSound);
        //}
    }

    public void CoinCollectSound()
    {
       
        _soundSource.PlayOneShot(_coinCollectSound);
    }

    public void StartGongSound()
    {
        _soundSource.PlayOneShot(_startGongSound);
    }

   

    public void StartFatalitySound()
    {
       
        _soundSource.PlayOneShot(_startFatalitySound);
    }

    public void EndFatalitySound()
    {
        _soundSource.PlayOneShot(_endFatalitySound);
    }

    public void GotItemCoinSound()
    {
        _soundSource.PlayOneShot(_gotItemCoinSound);
    }

    public void GotItemItemSound()
    {
        _soundSource.PlayOneShot(_gotItemItemSound);
    }

    public void GotItemBeltSound()
    {

        _soundSource.PlayOneShot(_gotItemBeltSound);
    }

    public void ItemScrollSound()
    {

        _soundSource.PlayOneShot(_itemScrollSound);
    }

    public void TapSound()
    {
        _soundSource.PlayOneShot(_tapSound);
    }
    public void GotHit()
    {
        _enemySource.PlayOneShot(_hitEnemy[UnityEngine.Random.Range(0,4)]);
    }
}
