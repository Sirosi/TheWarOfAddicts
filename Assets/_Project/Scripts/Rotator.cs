using UnityEngine;

namespace TheWarOfAddicts
{
    public class Rotator: MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 360f;
        
            
        void Update()
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }
}