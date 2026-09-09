using System;
using System.Collections.Generic;
using Lean.Pool;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.Unit;
using UnityEngine;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class DeckBuildManager: MonoBehaviour
    {
        [SerializeField] private DeckSlot addedPrefab;
        [SerializeField] private DeckSlot adderPrefab;
        
        [SerializeField] private RectTransform addedGroup;
        [SerializeField] private RectTransform adderGroup;


        private int DeckSize => PlayerData.Instance.DeckSize;
        
        
        private readonly List<DeckSlot> addedSlots = new(8);
        private readonly List<DeckSlot> adderSlots = new(8);
        

        public void Open()
        {
            gameObject.SetActive(true);
            
            foreach (DeckSlot slot in adderSlots)
            {
                LeanPool.Despawn(slot);
            }
            adderSlots.Clear();
            
            foreach (UnitData unit in PlayerData.Instance.RaceData.raceUnits)
            {
                if (PlayerData.Instance.HasUnlockUnit(unit.nameCode))
                {
                    DeckSlot slot = LeanPool.Spawn(adderPrefab, adderGroup);
                    slot.UnitData = unit;
                    slot.OnClick = OnClickAdder;
                    
                    adderSlots.Add(slot);
                }
            }

            UnitData[] deck = PlayerData.Instance.Deck;
            for (int i = 0; i < DeckSize; i++)
            {
                DeckSlot slot = LeanPool.Spawn(addedPrefab, addedGroup);
                slot.UnitData = deck.Length > i ? deck[i] : null;
                slot.OnClick = OnClickAdded;
                
                addedSlots.Add(slot);
            }
        }
        
        public void Close()
        {
            Stack<UnitData> units = new(addedSlots.Count);
            
            while (addedSlots.Count > 0)
            {
                DeckSlot slot = addedSlots[0];
                if (slot.UnitData)
                {
                    units.Push(slot.UnitData);
                }

                LeanPool.Despawn(slot);
                addedSlots.RemoveAt(0);
            }

            PlayerData.Instance.Deck = units.ToArray();
            
            gameObject.SetActive(false);
        }


        private void OnClickAdder(DeckSlot slot, UnitData unit)
        {
            bool isAdded = false;
            foreach (DeckSlot deckSlot in addedSlots)
            {
                if (deckSlot.UnitData != unit) continue;
                
                deckSlot.OnPointerClick(null);
                return;
            }
            foreach (DeckSlot deckSlot in addedSlots)
            {
                if (deckSlot.UnitData) continue;
                
                isAdded = true;
                deckSlot.UnitData = unit;
                AudioManager.Instance.PlayAudio(AudioType.Allow);
                break;
            }
            AudioManager.Instance.PlayAudio(AudioType.Deny);
        }
        private void OnClickAdded(DeckSlot slot, UnitData unit)
        {
            slot.UnitData = null;
            AudioManager.Instance.PlayAudio(AudioType.Out);
        }
    }
}