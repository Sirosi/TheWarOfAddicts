using TheWarOfAddicts.Core;
using TheWarOfAddicts.Race;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheWarOfAddicts.GameFlow.Lobby
{
    public class LobbyManager: Singleton<LobbyManager>
    {
        public void SelectRace(RaceData raceData)
        {
            PlayerData.Instance.Init(raceData);
            
            SceneManager.LoadScene("Prologue");
        }

        public void LoadGame()
        {
            if (PlayerData.LoadGame())
            {
                SceneManager.LoadScene("StageSelect");
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}