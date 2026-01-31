using System;
using Unity.Cinemachine;
using UnityEngine;

public class transition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polygonCollider2D;
    CinemachineConfiner2D cmc2d;

    [SerializeField] Directions direction;

    enum Directions {Up, Down, Left, Right}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cmc2d = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cmc2d.BoundingShape2D = polygonCollider2D;
            changePosPlayer(collision.gameObject);
        }
    }

    void changePosPlayer(GameObject player)
    {
        Vector3 pos = player.transform.position;
        switch (direction)
        {
            case Directions.Up:
                pos.y += 1.5f;
                break;
            case Directions.Down:
                pos.y -= 1.5f;
                break;
            case Directions.Left:
                pos.x -= 1.5f;
                break;
            case Directions.Right:
                pos.x += 1.5f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        player.transform.position = pos;
    }
}
