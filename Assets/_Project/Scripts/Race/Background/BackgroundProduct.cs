using UnityEngine;

namespace TheWarOfAddicts.Race.Background
{
    public class BackgroundProduct: MonoBehaviour
    {
        [SerializeField] public Transform[] children;
        [SerializeField] public byte weight = 1;


        void Reset()
        {
            children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = transform.GetChild(i);
            }
        }
    }
}