using UnityEngine;

public class RegionTile : MonoBehaviour
{
    public Vector3Int worldPosition;

    public float noiseValueCrossingX;
    public float noiseValueCrossingY;

    public bool withObject = false;

    public enum regionTileType
    {
        grass,
        water,
        mountan
    }
    public regionTileType tileType;

    private void Start()
    {
        if (tileType == regionTileType.water)
            GetComponent<Node>().isWalkable = false;
        if (tileType == regionTileType.mountan)
            GetComponent<Node>().isWalkable = false;
        if (tileType == regionTileType.grass)
            GetComponent<Node>().isWalkable = true;

        withObject = false;

        setPosition();
        setName();
    }

    private void setPosition()
    {
        worldPosition = new Vector3Int((int)this.transform.localPosition.x + GetComponentInParent<Region>().worldPositionRegion.x, (int)this.transform.localPosition.y + GetComponentInParent<Region>().worldPositionRegion.y, (int)this.transform.localPosition.z);
    }
    private void setName()
    {
        var x = this.transform.localPosition.x + GetComponentInParent<Region>().worldPositionRegion.x;
        var y = this.transform.localPosition.y + GetComponentInParent<Region>().worldPositionRegion.y;

        gameObject.name = "Tile" + ": WorldPosition" + "(" + x + ", " + y + ")";
    }

    public void setMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }

    public Vector2Int getPositionInWorld()
    {
        //Bug ist hier !!

        var x = transform.localPosition.x + GetComponentInParent<Region>().worldPositionRegion.x;
        var y = transform.localPosition.y + GetComponentInParent<Region>().worldPositionRegion.y;

        ///

        return new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
}
