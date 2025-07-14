using UnityEngine;

namespace Player
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        // Inspector variables
        [Header("Movement")]
        public float airAcceleration = 10f; // How fast to accelerate horizontally in air
        public float maxAirSpeed = 5f; // Max horizontal speed in air

        [Header("Jumping")]
        public float jumpForce = 10f; // Initial upward force for jump
        public float doubleJumpForce = 8f; // Upward force for double jump (usually slightly less than jumpForce)
        public bool enableDoubleJump = true; // Enable/disable double jump feature
        public float jumpCutMultiplier = 0.5f; // Reduce velocity when jump button released (for variable height)
        public float risingGravityScale = 1f; // Gravity multiplier when going up (lower for floatier jumps)
        public float fallingGravityScale = 2f; // Gravity multiplier when falling (higher for snappier descent)
        
        [Header("Timers")]
        public float coyoteTime = 0.1f; // Time after leaving ground to still jump
        public float jumpBufferTime = 0.1f; // Time to buffer jump input before landing
        public float jumpCooldown = 0.2f; // Delay after landing before next jump is allowed
    }
}