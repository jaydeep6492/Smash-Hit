using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField] protected GameObject m_BrokenGlass;

    [SerializeField] protected GameObject m_ParentGlass;
    // Start is called before the first frame update
    protected virtual void OnCollisionEnter(Collision other)
    {
        m_BrokenGlass.SetActive(true);
        Invoke("RemoveBroken",10f);
        gameObject.SetActive(false);
    }
    protected virtual void RemoveBroken()
    {
        m_BrokenGlass.SetActive(false);
        gameObject.SetActive(true);
        if (gameObject.transform.parent.tag.Contains("Obstacles"))
        {
            var CreatedObject = Instantiate(m_ParentGlass, transform.parent.position, Quaternion.identity, transform.parent.parent);
            CreatedObject.transform.localScale = transform.parent.localScale;
        }
        Destroy(m_BrokenGlass.transform.parent.gameObject);
    }
}
