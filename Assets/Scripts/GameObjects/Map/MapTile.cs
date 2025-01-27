using UnityEngine;
using System.Collections.Generic;

public class MapTile : MonoBehaviour
{
    public enum TileType
    {
        land,
        water,
        mountan
    }
    public TileType type;

    public List<Information> informations = new List<Information>();

    public float mapShapeNoiseValue;
    public float mountanNoiseValue;

    public Vector2 position;

    private Sprite sprite;

    private void Start()
    {
        position = new Vector2(this.transform.position.x, this.transform.position.y);
        setName();
    }
    private void setName()
    {
        gameObject.name = "MapTile: MapPosition" + "(" + this.transform.position.x + ", " + this.transform.position.y + ")";
    }

    public Sprite getSprite()
    {
        return sprite;
    }
    public TileType getMapTileType()
    {
        return type;
    }

    public void setSprite(Sprite spr)
    {
        GetComponent<SpriteRenderer>().sprite = spr;
        sprite = spr;
    }
    public void setMapTileType(TileType tileType)
    {
        type = tileType;
    }
}
