using System.Collections;
using UMX.Managers;
using UnityEngine;

public class BallShoot : MonoBehaviour
{
    private Vector3 m_Direction;
    private bool m_Throwing;
    private bool m_GameOver;

    private void Awake()
    {
        if (Manager.GameManager.NumberOfBall == 1)
        {
            m_GameOver = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        m_Throwing = false;
        if (other.gameObject.tag.Contains("Pyramid"))
        { 
          m_GameOver = false;
          Manager.GameManager.Points(3);
          Manager.GameManager.obstaclesPosition.Add(other.transform.parent.position);
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        if(!m_Throwing)
            return;
        transform.position += (m_Direction)*Time.deltaTime;
    }
    public void SetPath(Vector3 path,float Multiply)
    {
        path.z =10f;
        m_Direction = (Camera.main.ScreenToWorldPoint(path) - transform.position)*Multiply;
        m_Throwing = true;
        StartCoroutine(DestroyBall());
    }
    private IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(3f);
        if (m_GameOver && Manager.GameManager.NumberOfBall == 0)
            Manager.GameManager.GameOver(); Debug.Log("From Balls");
        Destroy(gameObject);
    }
}
