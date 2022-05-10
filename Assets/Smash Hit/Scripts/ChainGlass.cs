using UnityEngine;

namespace Smash_Hit.Scripts
{
    public class ChainGlass : Glass
    {
        protected override void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.tag.Contains("Ball")) return;
            var parent = gameObject.transform.parent;
            Destroy(parent.GetComponent<HingeJoint>());
            Destroy(parent.GetComponent<Rigidbody>());
            base.OnCollisionEnter(other);
        }

        protected override void RemoveBroken()
        {
            Destroy(gameObject.transform.parent.parent.gameObject);
        }
    }
}
