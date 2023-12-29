public class SizeComparer : IComparer<string>
{
    private static readonly List<string> SizeOrder = new List<string> { "XS", "S", "M", "L", "XL", "XXL" };

    public int Compare(string x, string y)
    {
        if (int.TryParse(x, out var numX) && int.TryParse(y, out var numY))
        {
            return numX.CompareTo(numY);
        }
        else if (int.TryParse(x, out numX))
        {
            return -1;
        }
        else if (int.TryParse(y, out numY))
        {
            return 1;
        }
        else
        {
            // Both are alphabetic, compare according to custom order
            var indexX = SizeOrder.IndexOf(x);
            var indexY = SizeOrder.IndexOf(y);

            if (indexX == -1) indexX = int.MaxValue;
            if (indexY == -1) indexY = int.MaxValue;

            return indexX.CompareTo(indexY);
        }
    }
}