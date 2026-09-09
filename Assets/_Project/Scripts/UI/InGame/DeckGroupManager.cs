using System.Collections.Generic;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.Unit;
using UnityEngine;

namespace TheWarOfAddicts.UI.InGame
{
    // 유저의 댁 정보를 받아서, 해당 댁을 생성해주기만 하면 됨.
    public class DeckGroupManager: ButtonGroupManager<SpawnButton, UnitData>
    {
        private static readonly KeyCode[] HoyKeys =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
        };
        
        protected override void Awake()
        {
            base.Awake();

            // 스모키드는 지역 덱만 사용 가능.
            if (PlayerData.Instance.RaceData.canUseDeck)
            {
                CreateButtons(PlayerData.Instance?.Deck);
            }
            else
            {
                CreateButtons(SessionData.SelectedStageData.playerDeck);
            }
        }

        void Update()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Input.GetKeyDown(HoyKeys[i]))
                {
                    Buttons[i].ClickButton();
                }
            }
        }
    }
}