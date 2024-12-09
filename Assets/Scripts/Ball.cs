using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {        
        transform.Translate(transform.forward * Time.deltaTime * speed,
            Space.World);
		if (transform.position.z <= Camera.main.transform.position.z)
		{
			//FindObjectOfType<GameManager>().EndLife();
		}
    }
	
	public void OnMouseDown()
    {
        GameManager.score++;
        Debug.Log(GameManager.score);
        Destroy(gameObject);		
    }
	
    public void OnCollisionEnter(Collision other)    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Oof! You hit the player!");  
            Destroy(gameObject);
            FindObjectOfType<GameManager>().EndLife();
        }
    }
	
	
	
}