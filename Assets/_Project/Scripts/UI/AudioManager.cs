using System.Collections;
using TheWarOfAddicts.Core;
using UnityEngine;

namespace TheWarOfAddicts.UI
{
    public class AudioManager: Singleton<AudioManager>
    {
        [SerializeField] private AudioClip mouseHoverSound;
        [SerializeField] private AudioClip mouseExitSound;
        [SerializeField] private AudioClip mouseClickSound;
        
        [SerializeField] private AudioClip allowSound;
        [SerializeField] private AudioClip denySound;
        [SerializeField] private AudioClip outSound;
        
        [SerializeField] private AudioClip upgradeSound;
        
        [SerializeField] private AudioClip[] bgmSounds;
        
        [SerializeField] private AudioSource sfxSource = null;
        [SerializeField] private AudioSource backgroundSource = null;


        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(BGMCo());
        }


        public void PlayAudio(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.MouseHover:
                    sfxSource.PlayOneShot(mouseHoverSound);
                    break;
                case AudioType.MouseExit:
                    sfxSource.PlayOneShot(mouseExitSound);
                    break;
                case AudioType.MouseClick:
                    sfxSource.PlayOneShot(mouseClickSound);
                    break;
                case AudioType.Allow:
                    sfxSource.PlayOneShot(allowSound);
                    break;
                case AudioType.Deny:
                    sfxSource.PlayOneShot(denySound);
                    break;
                case AudioType.Out:
                    sfxSource.PlayOneShot(outSound);
                    break;
                case AudioType.Upgrade:
                    sfxSource.PlayOneShot(upgradeSound);
                    break;
            }
        }

        private IEnumerator BGMCo()
        {
            int cnt = 0;
            
            while (true)
            {
                AudioClip clip = bgmSounds[cnt++];
                backgroundSource.PlayOneShot(clip);
                
                yield return new WaitForSeconds(clip.length + 2f);
                backgroundSource.Stop();

                if (cnt >= bgmSounds.Length)
                {
                    cnt = 0;
                    for (int i = 0; i < bgmSounds.Length; i++)
                    {
                        int r = Random.Range(0, bgmSounds.Length);
                        (bgmSounds[i], bgmSounds[r]) = (bgmSounds[r], bgmSounds[i]);
                    }
                }
            }
        }
    }
}