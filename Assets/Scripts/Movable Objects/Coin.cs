using UnityEngine;
using static GameConstants;

public sealed class Coin : Movable
{
    void Awake()
    {
        // Coin's moving speed depends on game difficulty
        moveSpeed = gameManager.difficulty == 0 ? EasyCoinMoveSpeed : gameManager.difficulty == 1 ? MediumCoinMoveSpeed : HardCoinMoveSpeed;
    }

    void OnEnable()
    {
        TeleportToRight();
        Move(MoveDirection.Left);
    }

    // Teleport to the right side of the screen with random Y position (for pooling)
    public override void TeleportToRight() => transform.position = new Vector2(8f, Random.Range(-2f, 2f));

    // Coin only moves left (or stops, when player dies)
    public override void Move(MoveDirection direction)
    {
        if (direction == MoveDirection.Left) rb.velocity = new Vector2(moveSpeed, 0f);
        else if (direction == MoveDirection.None) rb.velocity = Vector2.zero;
        else Debug.LogWarning("Invalid direction! Coin can only move left or stop moving!");
    }
}