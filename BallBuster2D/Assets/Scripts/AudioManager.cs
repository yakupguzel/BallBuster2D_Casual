using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bombExplosionSound;
    [SerializeField] private AudioSource boxExplosionSound;
    [SerializeField] private AudioSource ballThrowSound;




    public void PlayBombExplosionSound()
    {
        bombExplosionSound.Stop();
        bombExplosionSound.Play();
    }

    public void PlayBoxExplosionSound()
    {
        boxExplosionSound.Stop();
        boxExplosionSound.Play();
    }

    public void PlayBallThrowSound()
    {
        ballThrowSound.Stop();
        ballThrowSound.Play();
    }
}
