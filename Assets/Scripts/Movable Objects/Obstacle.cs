using UnityEngine;
using static Utils;
using static GameConstants;

public sealed class Obstacle : Movable
{
    float pingPongSpeed;

    void Awake()
    {
        // Obstacle's moving speed and ping-pong speed depends on game difficulty
        moveSpeed = gameManager.difficulty == 0 ? EasyObstacleMoveSpeed : gameManager.difficulty == 1 ? MediumObstacleMoveSpeed : HardObstacleMoveSpeed;
        pingPongSpeed = gameManager.difficulty == 0 ? 0.3f : gameManager.difficulty == 1 ? 0.45f : 0.6f;
    }

    void OnEnable()
    {
        TeleportToRight();
        Move(MoveDirection.Left);
        pingPongSpeed = PercentChanceSuccess(50) ? pingPongSpeed : -pingPongSpeed;  // Randomize initial PingPong direction
    }

    // Teleport to the right side of the screen with random Y position (for pooling)
    public override void TeleportToRight() => transform.position = new Vector2(6f, Random.Range(-1f, 1f));

    // Obstacle only moves left ping-pong style (or stops, when player dies)
    public override void Move(MoveDirection direction)
    {
        if (direction == MoveDirection.Left) rb.velocity = new Vector2(moveSpeed, pingPongSpeed);
        else if (direction == MoveDirection.None) rb.velocity = Vector2.zero;
        else Debug.LogWarning("Invalid direction! Obstacle can only move left or stop moving!");
    }

    // When Obstacle touches upper or lower collider, it will change its "PingPong" movement direction
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collider"))
        {
            pingPongSpeed = -pingPongSpeed;
            rb.velocity = new Vector2(moveSpeed, pingPongSpeed);
        }
    }
}