using System.Collections.Generic;
using TheWarOfAddicts.GameFlow.InGame;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheWarOfAddicts.UI.InGame
{
    public abstract class ButtonGroupManager<T1, T2>: MonoBehaviour, IBattleResultLockable where T1: CooldownButton<T2> where T2: ScriptableObject
    {
        [SerializeField] [Tooltip("테스트 용 Data, 절대 개발이 완료된 뒤에 살려둬선 안 됨.")] private T2[] testData;
        
        [SerializeField] private Transform buttonGroup;
        [SerializeField] private T1 buttonPrefab;

        
        protected List<T1> Buttons { get; } = new(10);
        

        protected virtual void Awake()
        {
            if (!buttonGroup)
            {
                buttonGroup = transform;
            }
            
            CreateButtons(testData);
        }


        protected void CreateButtons(params T2[] dataes)
        {
            if (dataes is null) return;
            
            foreach (var data in dataes)
            {
                T1 button = Instantiate(buttonPrefab, buttonGroup);
                button.Init(data);
                Buttons.Add(button);
            }
        }


        public void Lock()
        {
            foreach (T1 button in Buttons)
            {
                button.Button.interactable = false;
            }
        }
    }
}