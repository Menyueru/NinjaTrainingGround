using UnityEngine;

namespace Assets.Scripts
{
    public class DisableLadderCollider: MonoBehaviour
    {
        public Collider2D PlatformCollider;
        public LadderTopTrigger topTrigger;
        public OneWayPlatform oneWayPlatform;

        public void Update()
        {
            if (oneWayPlatform.oneway || (topTrigger.onLadderTop && Input.GetAxis("Vertical") < 0))
            {
                PlatformCollider.enabled = false;
            }
            else
            {
                PlatformCollider.enabled = true;
            }
        }
    }
}
