using UnityEngine;
using UnityEngine.U2D;

// Loader for player cosmetics (backgrounds/pipes). Uses PlayerPrefs to determine which cosmetic is active.
public sealed class SpriteAtlasLoader : MonoBehaviour
{
    [SerializeField] SpriteAtlas spriteAtlas;
    enum SpriteType { Background, Obstacle }
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] SpriteType type;

    void Awake()
    {
        if (type == SpriteType.Background)
            sprite.sprite = spriteAtlas.GetSprite("Background" + (PlayerPrefs.GetInt("BackgroundSelected", 0) + 1) + "_0");
        else
            sprite.sprite = spriteAtlas.GetSprite("Pipe" + (PlayerPrefs.GetInt("ObstacleSelected", 0) + 1) + "_0");
    }
}