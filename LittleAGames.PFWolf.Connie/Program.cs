namespace LittleAGames.PFWolf.Connie;

internal class Program
{
    static void Main(string[] args)
    {
        ConsoleFileClient consoleFileClient = new ConsoleFileClient();
        consoleFileClient.FindAvailableGames();
        
        var picks = new Dictionary<int, (string Text, Func<Result>? WorkTask)>
        {
            { -1, ("GAME VERSIONS", null) },
            // Checking files for specific versions of the game

            { -100, ("AUDIO FILES", null) },
            { 100, ("Display Audio Header Data", consoleFileClient.GetAudioHeaderList) },
            { 101, ("Display Audio File Data", consoleFileClient.GetAudioDataList) },

            { -200, ("GAME MAPS FILES", null) },
            { 200, ("Display Map Header", consoleFileClient.GetMapHeader) },
            { 201, ("Display Map Header Segments", consoleFileClient.GetMapHeaderSegments) },
            { 202, ("Display Map File Data", consoleFileClient.GetMapDataList) },

            { -300, ("VGA FILES", null) },
            { 300, ("Display Huffman Tree", consoleFileClient.GetVgaHuffmanTree) },
            // 300 TODO: Maybe "display" the node table from dictionary?
            { 301, ("Display VGA Header Data", consoleFileClient.GetVgaHeaderList) },
            { 302, ("Display VGA Assets", consoleFileClient.GetVgaAssets) },

            { -400, ("VSWAP FILES", null) },
            { 400, ("Display Swap Header Data", consoleFileClient.GetVswapHeaderList) },
            { 401, ("Display Swap Entry Data", consoleFileClient.GetVswapDataList) },

            { -1000, ("WAD FILES", null) },

            { -2000, ("PK3 FILES", null) },
            { 2000, ("Display PFWolf Directory Listing", consoleFileClient.GetPk3DirectoryList) },
            { 2001, ("Display PFWolf Pack Assets", consoleFileClient.GetPk3Assets) },

            { -2, ("APPLICATION", null) },
            { 0, ("Exit", ExitProgram) }
        };

        UIBuilder.RenderMenu(picks!);
    }

    private static Result ExitProgram()
    {
        System.Environment.Exit(1);
        return Result.Success();
    }
}
