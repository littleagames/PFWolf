namespace LittleAGames.PFWolf.SDK.Utilities;

public static class ArrayExtensions
{
    public static T[] To1DArray<T>(this T[,] input)
    {
        var rows = input.GetLength(0);
        var cols = input.GetLength(1);
        if (input.Length != rows * cols)
            throw new ArgumentException("The size of the input array must match the specified dimensions.");

        T[] result = new T[rows * cols];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                result[x * rows + y] = input[x, y];
            }
        }

        return result;
    }
    public static T[,] To2DArray<T>(this T[] input, int rows, int cols)
    {
        if (input.Length != rows * cols)
            throw new ArgumentException("The size of the input array must match the specified dimensions.");

        T[,] result = new T[rows, cols];

        for (int i = 0; i < input.Length; i++)
        {
            int row = i / cols; // Calculate the row index
            int col = i % cols; // Calculate the column index
            result[row, col] = input[i];
        }

        return result;
    }
}