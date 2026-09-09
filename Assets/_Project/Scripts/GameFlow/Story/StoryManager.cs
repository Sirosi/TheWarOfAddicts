using TheWarOfAddicts.GameFlow;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheWarOfAddicts.GameFlow.Story
{
    public class StoryManager: MonoBehaviour
    {
        [SerializeField] private GameObject[] prologues;
        
        void Awake()
        {
            for (int i = 0; i < prologues.Length; i++)
            {
                prologues[i].SetActive((int)PlayerData.Instance.RaceData.raceType == i);
            }
        }
        
        public void StartButton(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}