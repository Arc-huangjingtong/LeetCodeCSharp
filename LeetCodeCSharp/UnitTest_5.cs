namespace LeetCodeCSharp;

public partial class UnitTest
{
    [Test]
    public void Test5()
    {
        // Example usage:
        var n    = 1; // Binary: 10011100
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



    public class Solution_1766
    {
        public int[] GetCoprimes(int[] nums, int[][] edges)
        {
            var length = nums.Length;
            var dict   = new Dictionary<int, HashSet<int>>();

            for (var i = 0 ; i < length ; i++)
            {
                dict.TryAdd(i, []);
            }

            foreach (var edge in edges)
            {
                dict[edge[0]].Add(edge[1]);
            }

            var ans = new int[length];

            for (var i = 0 ; i < length ; i++)
            {
                ans[i] = -1;
            }

            var primes = new HashSet<int>();

            return ans;


            void DFS(int node, int depth, int parent, int[] nums) { }
        }


        private static bool GDC(int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a == 1;
        }
    }



    public class Solution_2923
    {
        public int FindChampion(int[][] grid)
        {
            Span<bool> team = new bool[grid.Length];

            team.Fill(true);

            for (var i = 0 ; i < grid.Length ; i++)
            {
                for (var j = 0 ; j < grid.Length ; j++)
                {
                    if (grid[j][i] == 1)
                    {
                        team[i] = false;
                    }
                }
            }

            return team.IndexOf(true);
        }

        public int FindChampion2(int[][] grid)
        {
            int n = grid.Length, result = 0;

            for (var i = 0 ; i < n ; i++)
            {
                if (grid[i][result] == 1) result = i;
            }

            GC.Collect();

            return result;
        }


        [Test]
        public void Test()
        {
            int[][] grid   = [[0, 0, 0], [1, 0, 0], [1, 1, 0]];
            var     result = FindChampion(grid);
            Console.WriteLine(result);
        }
    }



    public ref struct RefMyStruct(int value)
    {
        public int Value = value;
        
        public void SetValue(int value)
        {
            Value = value;
        }
        
        public static ref RefMyStruct GetRef(ref RefMyStruct myStruct)
        {
            return ref myStruct;
        }
    }


    public struct MyStruct(int value)
    {
        public int Value = value;
        
        public void SetValue(int value)
        {
            Value = value;
        }
        
        public static ref MyStruct GetRef(ref MyStruct myStruct)
        {
            return ref myStruct;
        }
    }
    
    [Test]
    public void Test()
    {
        var myStruct = new MyStruct(1);
        ref var refMyStruct = ref MyStruct.GetRef(ref myStruct);
        refMyStruct.SetValue(2);
        Console.WriteLine(myStruct.Value);
    }
    
}