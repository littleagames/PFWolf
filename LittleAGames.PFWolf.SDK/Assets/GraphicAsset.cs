namespace LittleAGames.PFWolf.SDK.Assets;

public class GraphicAsset : VgaAsset
{
    public GraphicAsset()
    {
        AssetType = AssetType.Graphic;
    }
    
    public Dimension Dimensions { get; set; } = null!;
}

public class PngGraphicAsset : GraphicAsset
{
    
}