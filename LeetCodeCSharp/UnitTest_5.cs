namespace LeetCodeCSharp;

public partial class UnitTest
{
    [Test]
    public void Test5()
    {
        // Example usage:
        var n    = 9; // Binary: 10011100
        var next = Gosper_Hack(n);

        while (n != 0)
        {
            Console.WriteLine($"Next higher number with same bit count as {ToBinary(n)} is {ToBinary(next)}");

            n    = next;
            next = Gosper_Hack(n);
        }
    }


   



    private static string ToBinary(int decimalNumber)
    {
        if (decimalNumber == 0) return "0";

        string binaryNumber = "";
        while (decimalNumber > 0)
        {
            int remainder = decimalNumber % 2;
            binaryNumber  =  remainder + binaryNumber;
            decimalNumber /= 2;
        }

        return binaryNumber;
    }
}