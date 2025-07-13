using UnityEngine;

public class FXFeedBack : MonoBehaviour
{

    public GameObject FeedBackPrefab;
    public AttackPreviewManager attackPreviewManager;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(contact.normal, Vector3.forward);
        Vector3 position = contact.point;
        Instantiate(FeedBackPrefab, position, rotation);
        attackPreviewManager.CharacterMode = AttackPreviewManager.Mode.Idle;
        Destroy(collision.transform.gameObject);
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
