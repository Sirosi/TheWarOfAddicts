using TheWarOfAddicts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using AudioType = TheWarOfAddicts.UI.AudioType;

namespace TheWarOfAddicts.GameFlow.StageSelect
{
    public class StageNode: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private StageNode[] requiredNodes;
        
        [SerializeField] private bool interactable = true;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color disabledColor;
            
        [SerializeField] private StageData.StageData stageData;
        
        
        public StageData.StageData StageData => stageData;


        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
                if (interactable)
                {
                    spriteRenderer.color = normalColor;
                }
                else
                {
                    spriteRenderer.color = disabledColor;
                }
            }
        }


        void Start()
        {
            bool hasRequired = true;
            foreach (StageNode node in requiredNodes)
            {
                if (!PlayerData.Instance.IsClearStage(node.StageData.nameCode))
                {
                    hasRequired = false;
                    break;
                }
            }
            
            Interactable = hasRequired;
        }

        void OnValidate()
        {
            Interactable = interactable;
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
            {
                AudioManager.Instance.PlayAudio(AudioType.Deny);
                return;
            }
            
            AudioManager.Instance.PlayAudio(AudioType.Allow);
            StageSelectManager.Instance.OpenStageInfoPopup(stageData);
        }
    }
}