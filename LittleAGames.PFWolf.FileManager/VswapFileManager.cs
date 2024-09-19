namespace LittleAGames.PFWolf.FileManager;

public class VswapFileManager
{
    private readonly string swapDataFile;

    public VswapFileManager(string swapDataFile)
    {
        this.swapDataFile = swapDataFile;
    }

    public Result<SwapData> GetData()
    {
        var data = File.ReadAllBytes(swapDataFile);
        var swapData = new SwapData(data);
        return swapData;
    }

    public Result<IEnumerable<SwapEntry>> GetSwapEntries()
    {
        var data = File.ReadAllBytes(swapDataFile);
        var swapData = new SwapData(data);
        return swapData.GetEntries();
    }
}
