using UnityEngine;

public class Door : MonoBehaviour
{

    #region Private Fields

    private bool m_Animate;
    
    [SerializeField] private Animator[] m_DoorCircle;
    [SerializeField] private Animator[] m_Door;
    
    #endregion

    #region Unity CallBAcks

    private void OnTriggerEnter(Collider other)
    {
        m_Animate = true;
    }
    
    private void Update()
    {
        if(!m_Animate)
            return;
        m_DoorCircle[0].SetBool("Turn",true);
        m_DoorCircle[1].SetBool("Turn",true);
        if (!m_DoorCircle[0].GetCurrentAnimatorStateInfo(0).IsName("Stay") ||
            !m_DoorCircle[1].GetCurrentAnimatorStateInfo(0).IsName("Stay")) return;
        m_DoorCircle[0].SetBool("Open",true);
        m_Door[0].SetBool("Open",true);
        m_DoorCircle[1].SetBool("Open",true);
        m_Door[1].SetBool("Open",true);
        Invoke("CloseDoor",5f);
        m_Animate = false;
    }
    #endregion

    void CloseDoor()
    {
        m_DoorCircle[0].SetBool("Turn",false);
        m_DoorCircle[1].SetBool("Turn",false);
        m_DoorCircle[0].SetBool("Open",false);
        m_Door[0].SetBool("Open",false);
        m_DoorCircle[1].SetBool("Open",false);
        m_Door[1].SetBool("Open",false);
    }
    
    
}
