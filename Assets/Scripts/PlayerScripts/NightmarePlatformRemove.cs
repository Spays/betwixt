using UnityEngine;

namespace PlayerScripts
{
    public class NightmarePlatformRemove : MonoBehaviour
    {
        public void Remove()
        {
            gameObject.SetActive(false);
        }
    }
}