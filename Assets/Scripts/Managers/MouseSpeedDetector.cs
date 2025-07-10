using UnityEngine;

namespace Managers
{
    public class MouseSpeedDetector : MonoBehaviour
    {
        private Vector3 previousMousePosition;
        
        public float GetMouseSpeed()
        {
            var position = Input.mousePosition;
            var delta = (position - previousMousePosition).magnitude;
            
            previousMousePosition = position;
            
            return delta;
        }

        private void Update()
        {
            Debug.Log(GetMouseSpeed());
        }
    }
}