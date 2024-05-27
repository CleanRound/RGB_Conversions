public struct RGBColor
{
    public int Red { get; }
    public int Green { get; }
    public int Blue { get; }

    public RGBColor(int red, int green, int blue)
    {
        if (red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
            throw new ArgumentOutOfRangeException("RGB values must be between 0 and 255");

        Red = red;
        Green = green;
        Blue = blue;
    }

    public string ToHex()
    {
        return $"#{Red:X2}{Green:X2}{Blue:X2}";
    }

    public (double Hue, double Saturation, double Lightness) ToHSL()
    {
        double r = Red / 255.0;
        double g = Green / 255.0;
        double b = Blue / 255.0;

        double max = Math.Max(Math.Max(r, g), b);
        double min = Math.Min(Math.Min(r, g), b);
        double delta = max - min;

        double hue = 0;
        if (delta > 0)
        {
            if (max == r)
            {
                hue = (g - b) / delta + (g < b ? 6 : 0);
            }
            else if (max == g)
            {
                hue = (b - r) / delta + 2;
            }
            else
            {
                hue = (r - g) / delta + 4;
            }
            hue /= 6;
        }

        double lightness = (max + min) / 2;
        double saturation = delta == 0 ? 0 : delta / (1 - Math.Abs(2 * lightness - 1));

        return (hue * 360, saturation * 100, lightness * 100);
    }

    public (double Cyan, double Magenta, double Yellow, double Black) ToCMYK()
    {
        if (Red == 0 && Green == 0 && Blue == 0)
        {
            return (0, 0, 0, 1);
        }

        double r = Red / 255.0;
        double g = Green / 255.0;
        double b = Blue / 255.0;

        double k = 1 - Math.Max(Math.Max(r, g), b);
        double c = (1 - r - k) / (1 - k);
        double m = (1 - g - k) / (1 - k);
        double y = (1 - b - k) / (1 - k);

        return (c, m, y, k);
    }

    public override string ToString()
    {
        return $"RGB({Red}, {Green}, {Blue})";
    }
}

public class Program
{
    public static void Main()
    {
        RGBColor color = new RGBColor(255, 165, 0);

        string hex = color.ToHex();
        Console.WriteLine($"{color} in HEX: {hex}");

        var (hue, saturation, lightness) = color.ToHSL();
        Console.WriteLine($"{color} in HSL: ({hue:F2}, {saturation:F2}%, {lightness:F2}%)");

        var (cyan, magenta, yellow, black) = color.ToCMYK();
        Console.WriteLine($"{color} in CMYK: ({cyan:F2}, {magenta:F2}, {yellow:F2}, {black:F2})");
    }
}
