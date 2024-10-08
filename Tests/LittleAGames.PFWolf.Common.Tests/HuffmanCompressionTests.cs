using CSharpFunctionalExtensions;
using LittleAGames.PFWolf.Common.Compression;

namespace LittleAGames.PFWolf.Common.Tests
{
    [TestClass]
    public class HuffmanCompressionTests
    {
        private static string path = "D:\\wolf3d-v1.4-apogee";

        [TestMethod]
        public void Expand_huffnode_and_Tree_Compare()
        {
            var dictionaryFilePath = $"{path}\\VGADICT.WL6";
            var gfxFilePath = $"{path}\\VGAGRAPH.WL6";
            var headerFilePath = $"{path}\\VGAHEAD.WL6";

            var headerData = GetHeaderList(headerFilePath);
            var dictionaryData = File.ReadAllBytes(dictionaryFilePath);
            var gfxData = File.ReadAllBytes(gfxFilePath);
            ICompression compression = new HuffmanCompression(dictionaryData);

            const int StructPic = 0;
            var filePosition = headerData[StructPic]; // STRUCTPIC
            var chunkcomplen = headerData[StructPic + 1] - headerData[StructPic] - 4;

            var compseg = gfxData.Skip(filePosition + 4).Take(chunkcomplen).ToArray();
            var expanded = compression.Expand(compseg);
            var compressed = compression.Compress(expanded);

           // Assert.AreEqual(compressed.Length, compseg.Length);
            for(var i = 0; i < compressed.Length; i++)
            {
                Assert.AreEqual(compressed[i], compseg[i], $"Index: {i} values are mismatched.");
            }
        }
        private List<int> GetHeaderList(string headerFile)
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
    }
}