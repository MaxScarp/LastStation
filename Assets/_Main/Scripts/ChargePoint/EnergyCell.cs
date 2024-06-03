using UnityEngine;

public class EnergyCell : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
