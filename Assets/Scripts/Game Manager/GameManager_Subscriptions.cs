using UnityEngine;

// Partial class for managing player event subscriptions and responses in the GameManager
public partial class GameManager : MonoBehaviour
{
    void SubscribeToPlayerEvents()
    {
        player.OnDeath += StopAllObjects;
        player.OnRespawn += RespawnAllObjects;
        player.OnCoinTake += TakeCoin;
    }

    // Increments coin count and updates UI (called when Player collects a coin)
    void TakeCoin()
    {
        coin += 1;
        currentCoins += 1;
        coinText.text = coin.ToString();
    }

    // Stops Coins and Obstacles movement, cancels spawns, and updates death stats (called on Player death)
    void StopAllObjects()
    {
        foreach (Movable coin in coinsToPool) coin.Move(Movable.MoveDirection.None);
        foreach (Movable obstacle in obstaclesToPool) obstacle.Move(Movable.MoveDirection.None);
        if (spawnBirdsOption) foreach (Bird bird in birdsToPool) bird.FlyAwayAfterPlayerDeath();

        CancelInvoke();

        totalDeaths += 1;
        deathText.text = $"Total Deaths: {totalDeaths}\nHigh Score: {highScore}\nCoins Collected This Round: {currentCoins}";
        currentCoins = 0;
        this.Wait(0.5f, () => menu.SetActive(true));
    }

    // Disables all pooled objects, restarts gameplay loops, resets score and updates UI (called on Player respawn)
    void RespawnAllObjects()
    {
        foreach (Movable coin in coinsToPool) coin.gameObject.SetActive(false);
        foreach (Movable obstacle in obstaclesToPool) obstacle.gameObject.SetActive(false);
        foreach (Movable bird in birdsToPool) bird.gameObject.SetActive(false);

        StartGameplayLoops();

        score = 0;
        scoreText.text = score + " / " + highScore;
    }
}