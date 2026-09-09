using UnityEngine;

namespace TheWarOfAddicts.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T: Component
    {
        public static T Instance { get; private set; } = null;
    
    
        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = GetComponent<T>();
            }
            else
            {
                DestroyImmediate(this);
            }
        }
    }
}
