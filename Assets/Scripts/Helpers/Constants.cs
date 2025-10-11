// Contains all configurable values and magic numbers used throughout the game
public static class Constants
{
    // ---------- Maximum counts ----------
    public const int MaxBirdTypes = 8;
    public const int MaxBackgroundTypes = 8;
    public const int MaxObstacleTypes = 6;

    // ---------- Buying Costs ----------
    public const int BirdUnlockCost = 50;
    public const int BackgroundUnlockCost = 50;
    public const int ObstacleUnlockCost = 50;
    public const int SkillUnlockCost = 100;

    // ---------- Probabilities (percent) ----------
    public const int Skill1Level1CoinChance = 50;
    public const int Skill1Level2CoinChance = 70;
    public const int BirdSpawnChance = 40;
    public const int BirdMoveRightChance = 70;
    public const int BirdChatChance = 30;

    // ---------- Spawn delays ----------
    public const float EasyObstacleSpawnDelay = 1.5f;
    public const float MediumObstacleSpawnDelay = 1.25f;
    public const float HardObstacleSpawnDelay = 1f;

    // ---------- Score gain per tick by difficulty ----------
    public const float ScoreGainInterval = 1f;
    public const int EasyScoreIncrement = 2;
    public const int MediumScoreIncrement = 3;
    public const int HardScoreIncrement = 4;


    public const float DayNightCycleInterval = 0.2f;

    // ---------- Movement speeds by difficulty ----------
    public const float EasyBirdSpeed = -5f;
    public const float MediumBirdSpeed = -6f;
    public const float HardBirdSpeed = -7f;

    public const float EasyObstacleSpeed = -3f;
    public const float MediumObstacleSpeed = -4f;
    public const float HardObstacleSpeed = -5f;

    public const float EasyCoinSpeed = -3f;
    public const float MediumCoinSpeed = -4f;
    public const float HardCoinSpeed = -5f;
}
