using UnityEngine;
using System;

namespace Resurces
{
    public class TeleportFlowerResurce:MonoBehaviour , ResurcesInterface
    {
        [field: SerializeField] private int _resursIndex;

        public static event Action<int[]> SendMessage;
        
        public int resursIndex
        {
            get { return _resursIndex; }
            set
            {
                _resursIndex = value;
                //OnChange?.Invoke(_resursIndex);
            }
        }

        [field: SerializeField] public int resursValue { get; set; }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            SendMessage?.Invoke(new int[2]{_resursIndex, resursValue});
            Debug.Log("Flower increased " + resursValue);
            SelfKill();
        }

        public void SelfKill()
        {
            Destroy(this.gameObject);
        }
    }
}