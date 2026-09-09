using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI
{
    public class CustomButton: Button, IPointerEnterHandler, IPointerExitHandler
    {
        private const float ANIM_TIME = 0.1f;
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.1f, ANIM_TIME).SetEase(Ease.InOutCirc);
            AudioManager.Instance.PlayAudio(AudioType.MouseHover);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, ANIM_TIME).SetEase(Ease.InOutCirc);
            AudioManager.Instance.PlayAudio(AudioType.MouseExit);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
           transform.DOScale(Vector3.one, ANIM_TIME).SetEase(Ease.InOutCirc);
           AudioManager.Instance.PlayAudio(AudioType.MouseClick);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.1f, ANIM_TIME).SetEase(Ease.InOutCirc);
        }
    }
}