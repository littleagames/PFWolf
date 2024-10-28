namespace ClickAndDragToLoadFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The idea here is if you click/drag files into the exe, they come in as args
            // Then you find all the file args, and check em (they should really only be valid pk3 files)
            Console.WriteLine("Hello, World!");
            foreach (var arg in args)
                Console.WriteLine($"arg: {arg}");

            // arg is a file path

            Console.ReadLine();
        }
    }
}
