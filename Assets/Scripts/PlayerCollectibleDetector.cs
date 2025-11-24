using UnityEngine;

public class PlayerCollectibleDetector : MonoBehaviour
{
    private CollectibleManager manager;
    
    public void Init(CollectibleManager manager)
    {
        this.manager = manager;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.Collect(transform);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
