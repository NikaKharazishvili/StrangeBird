// Contains all magic numbers used throughout the game for easy adjustment and maintenance
public static class GameConstants
{
    // Maximum counts
    public const int MaxBirdStyles = 8;
    public const int MaxBackgroundStyles = 8;
    public const int MaxObstacleStyles = 6;

    // Costs
    public const int BirdCost = 50;
    public const int BackgroundCost = 50;
    public const int ObstacleCost = 50;
    public const int SkillCost = 100;

    // Probabilities
    public const int Skill1Level1CoinSpawnChance = 50;
    public const int Skill1Level2CoinSpawnChance = 70;
    public const int BirdSpawnChance = 40;
    public const int BirdMoveRightChance = 70;
    public const int BirdChatChance = 30;

    // InvokeRepeating delays
    public const float EasyObstaclesSpawnDelay = 1.5f;
    public const float MediumObstaclesSpawnDelay = 1.3f;
    public const float HardObstaclesSpawnDelay = 1f;
    public const float DayNightCycleDelay = 0.2f;
    public const float GainScoreDelay = 1f;
}