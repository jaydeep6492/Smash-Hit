using UnityEngine;

namespace Smash_Hit.Scripts
{
    public class BrokenGlass : Glass
    {
        protected override void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Contains("Ball"))
            {
                base.OnCollisionEnter(other);
            }
        }
    }
}    
