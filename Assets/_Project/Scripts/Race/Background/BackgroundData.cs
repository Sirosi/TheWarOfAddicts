using UnityEngine;

namespace TheWarOfAddicts.Race.Background
{
    [CreateAssetMenu(menuName = "Race/New Background Data", fileName = "New Background Data")]
    public class BackgroundData: ScriptableObject
    {
        [Tooltip("배경 연출용 프리팹들")] public BackgroundProduct[] products;
        [Tooltip("프리팹 배치 간격")] public float intervalWidth = 6f;
    }
}