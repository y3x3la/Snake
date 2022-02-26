using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    private List<Transform> _segments = new List<Transform>();

    private GameObject player;

    public AudioClip audioClip;
    private AudioSource audioSource;

    public Transform segmentPrefab;

    public int initialSize = 4;
  



    private void Start()
    {
        ResetState();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = player.GetComponent<AudioSource>();
    }


    private void Update()
    {
        SetDirection();
    }

    private void SetDirection()
    {
        var newDirection = Vector2.zero;

        var axisX = Input.GetAxis("Horizontal");
        var axisY = Input.GetAxis("Vertical");

        if (axisY > 0.01)
            newDirection = Vector2.up;
        else if (axisY < -0.01)
            newDirection = Vector2.down;
        else if (axisX > 0.01)
            newDirection = Vector2.right;
        else if (axisX < -0.01)
            newDirection = Vector2.left;
        else return;

        if (_direction != -newDirection)
            _direction = newDirection;
    }



    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for(int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
           
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
            ScoreManager.score += 1;
            audioSource.Play();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Vector3 position = transform.position;

            if (_direction.x != 0f)
            {
                position.x = -other.transform.position.x + _direction.x;
            }
            else if (_direction.y != 0f)
            {
                position.y = -other.transform.position.y + _direction.y; 
            }

            transform.position = position;
        }
        else if (other.gameObject.CompareTag("SnakeSegm"))
        {
            ResetState();
        }
    }
}
