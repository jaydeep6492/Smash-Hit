using UMX.Managers;
using UnityEngine;

public class PlayerPath : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag.Contains("Broken") || other.gameObject.tag.Contains("Ball")) ||
            other.gameObject.tag.Contains("Door"))
            return; 
        Debug.Log(other.gameObject.name);
        Manager.GameManager.Points(-10);
    }
}
