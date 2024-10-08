namespace LittleAGames.PFWolf.FileManager;

public class VgaFileManager
{
    private readonly string dictionaryFile;
    private readonly string headerFile;
    private readonly string dataFile;

    public VgaFileManager(string dictionaryFile, string headerFile, string dataFile)
    {
        this.dictionaryFile = dictionaryFile;
        this.headerFile = headerFile;
        this.dataFile = dataFile;
    }

    public ICompression GetDictionary()
    {
        var data = File.ReadAllBytes(dictionaryFile);
        return new HuffmanCompression(data);
    }

    public Result<List<int>> GetHeaderList()
    {
        var data = File.ReadAllBytes(headerFile);
        var graphicStartIndexes = new List<int>((data.Length / 3));

        for (var i = 0; i < (data.Length / 3); i++)
        {
            var d0 = data[i * 3];
            var d1 = data[i * 3 + 1];
            var d2 = data[i * 3 + 2];
            int val = d0 | d1 << 8 | d2 << 16;
            var grStart = val == 0x00FFFFFF ? -1 : val;
            graphicStartIndexes.Add(grStart);
        }

        return graphicStartIndexes;
    }

    public Result<VgaData> GetMetadataList()
    {
        return GetHeaderList()
            .Check(headerData =>
            {
                const int StructPic = 0;
                var gfxFileData = File.ReadAllBytes(dataFile);
                var filePosition = headerData[StructPic]; // STRUCTPIC
                var chunkcomplen = headerData[StructPic + 1] - headerData[StructPic] - 4;

                var compseg = gfxFileData.Skip(filePosition + 4).Take(chunkcomplen).ToArray();
                var destTable = GetDictionary().Expand(compseg);
                //var numPics = 144;// 132; // 144 for apogee
                //var destTable2 = GetDictionary().Expand(compseg, length: numPics*(sizeof(short)*2));
                return Result.Success(); // TODO: Returns a result
            })
            .Map(headerData => new VgaData
            {
                NumChunks = headerData.Count - 1
            });
    }
}
