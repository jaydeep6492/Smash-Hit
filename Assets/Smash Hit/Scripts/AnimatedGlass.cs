using UnityEngine;

public class AnimatedGlass : Glass
{
    private float m_AnimationSpeed = 5f;
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Ball"))
        {
            base.OnCollisionEnter(other);
        }
    }
    private void Update()
    {
        if (transform.parent.localPosition.y < 3)
        {
            m_AnimationSpeed = 5f;    
        }
        else if (transform.parent.localPosition.y > 11f)
        {
            m_AnimationSpeed = -5f;
        }
        transform.parent.localPosition += new Vector3(0,m_AnimationSpeed,0) * Time.deltaTime;
    }
}
