namespace LittleAGames.PFWolf.SDK.Utilities;

public class RotateExtensions
{
    public static byte[,] Rotate(byte[,] original, double angle)
    {
        int rows = original.GetLength(0);
        int cols = original.GetLength(1);
        double radians = angle * Math.PI / 180.0;

        // Calculate center of the image
        double centerX = rows / 2.0;
        double centerY = cols / 2.0;

        // Determine bounds of the rotated image
        byte[,] rotated = new byte[rows, cols];

        // Rotate each pixel
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Map rotated pixel back to original image coordinates
                double x = i - centerX;
                double y = j - centerY;

                double originalX = x * Math.Cos(-radians) - y * Math.Sin(-radians) + centerX;
                double originalY = x * Math.Sin(-radians) + y * Math.Cos(-radians) + centerY;

                // Check if the original coordinates are within bounds
                int originalRow = (int)Math.Round(originalX);
                int originalCol = (int)Math.Round(originalY);

                if (originalRow >= 0 && originalRow < rows && originalCol >= 0 && originalCol < cols)
                {
                    rotated[i, j] = original[originalRow, originalCol];
                }
                else
                {
                    rotated[i, j] = 0xff; // Set to a default value (e.g., background color)
                }
            }
        }

        return rotated;
    }
}