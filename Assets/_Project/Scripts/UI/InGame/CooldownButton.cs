using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.InGame
{
    public abstract class CooldownButton<T>: MonoBehaviour where T: ScriptableObject
    {
        [SerializeField] private T data;
        [SerializeField] protected TextMeshProUGUI costText;
        [SerializeField] protected Image icon;
        [SerializeField] private Image mask;


        public T Data => data;
        public Button Button => button;

        protected abstract float Cooldown { get; }
        protected abstract bool IsReady { get; }
        
        public bool Interactable => button.interactable;


        private Button button = null;
        private Coroutine cooldownCoroutine = null;
        private float lastSpawn = 0f;


        void Awake()
        {
            // WeaponData를 직접 참조시켜서 테스트하기위한 용도
            if (data)
            {
                Init(data);
            }
        }


        public virtual void Init(T data)
        {
            this.data = data;
            
            button = GetComponent<Button>();
            button.onClick.AddListener(ClickButton);
            lastSpawn = -Cooldown;
            
            StartCooldown(0.5f);
        }


        public void StartCooldown(float delay)
        {
            if (cooldownCoroutine is not null)
            {
                StopCoroutine(cooldownCoroutine);
            }
            cooldownCoroutine = StartCoroutine(FillImageCo(delay));
        }
        
        public void ClickButton()
        {
            if (button.interactable)
            {
                if (!IsReady)
                {
                    AudioManager.Instance.PlayAudio(AudioType.Deny);
                    return;
                }
                
                lastSpawn = Time.time;
                AudioManager.Instance.PlayAudio(AudioType.Allow);
                OnClick();
                StartCooldown(Cooldown);
            }
        }

        
        protected abstract void OnClick();
        

        private IEnumerator FillImageCo(float delay)
        {
            float timer = 0f;
            button.interactable = false;
            while (timer < delay)
            {
                timer += Time.deltaTime;
                mask.fillAmount = (delay - timer) / delay;

                yield return null;
            }
            mask.fillAmount = 0;
            button.interactable = true;
            cooldownCoroutine = null;
        }
    }
}