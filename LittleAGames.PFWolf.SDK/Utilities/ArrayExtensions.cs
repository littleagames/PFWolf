namespace LittleAGames.PFWolf.SDK.Utilities;

public static class ArrayExtensions
{
    public static void Fill<T>(this T[,] array, T value)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        
        for (var row = 0; row < array.GetLength(0); row++)
        {
            for (var column = 0; column < array.GetLength(1); column++)
            {
                array[row, column] = value;
            }
        }
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