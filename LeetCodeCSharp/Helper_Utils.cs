namespace LeetCodeCSharp;

public partial class UnitTest
{
    /// <summary>更具数组生成树</summary>
    /// <param name="array">可空的,层序遍历的数组</param>
    /// <returns>基于TreeNode生成的树结构</returns>
    public static TreeNode CreateTree(int?[] array)
    {
        if (array.Length == 0 || array[0] == null)
        {
            return null!;
        }

        var root  = new TreeNode(array[0]!.Value);
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        for (var i = 1 ; i < array.Length ; i++)
        {
            if (queue.Count == 0)
            {
                break;
            }

            var node = queue.Dequeue();
            if (array[i] != null)
            {
                node.left = new TreeNode(array[i]!.Value);
                queue.Enqueue(node.left);
            }

            i++;
            if (i < array.Length && array[i] != null)
            {
                node.right = new TreeNode(array[i]!.Value);
                queue.Enqueue(node.right);
            }
        }

        return root;
    }


    /// <summary>更具数组字符串生成树</summary>
    /// <param name="arrayString">层序遍历的数组字符串</param>
    /// <returns>基于TreeNode生成的树结构</returns>
    public static TreeNode CreateTree(string arrayString)
    {
        arrayString = arrayString.Trim('[', ']');
        var array = arrayString.Split(',');
        var nodes = new int?[array.Length];
        for (var i = 0 ; i < array.Length ; i++)
        {
            if (array[i] == "null")
            {
                nodes[i] = null;
            }
            else
            {
                nodes[i] = int.Parse(array[i]);
            }
        }

        return CreateTree(nodes);
    }

    public int RangeSumBST(TreeNode root, int low, int high)
    {
        var sum   = 0;
        var stack = new Stack<TreeNode>();

        stack.Push(root);

        while (stack.Count > 0)
        {
            var list = stack.ToList();
            stack.Clear();
            foreach (var node in list)
            {
                if (node == null)
                {
                    continue;
                }

                if (node.val >= low && node.val <= high)
                {
                    sum += node.val;
                }

                stack.Push(node.left);
                stack.Push(node.right);
            }
        }

        return sum;
    }


  

    public static readonly HashSet<int> PrimeSet = [];


    public long CountPaths(int n, int[][] edges)
    {
        var ans = 0;

        foreach (var _edge in edges)
        {
            var edge1 = _edge[0];
            var edge2 = _edge[1];
            var count = 0;
            if (PrimeSet.Contains(edge1))
            {
                count++;
            }
            else
            {
                if (IsPrime(edge1))
                {
                    PrimeSet.Add(edge1);
                    count++;
                }
            }

            if (PrimeSet.Contains(edge2))
            {
                count++;
            }
            else
            {
                if (IsPrime(edge2))
                {
                    PrimeSet.Add(edge2);
                    count++;
                }
            }

            if (count == 1)
            {
                ans++;
            }
        }

        return ans;
    }

    public static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }

        for (var i = 2 ; i * i <= number ; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    [TestCase(10, ExpectedResult = 4)]
    public int CountPrimes(int n)
    {
        if (n < 3) return 0;

        Span<bool> isPrime = stackalloc bool[n];

        isPrime.Fill(true);

        var sqrt = (int)Math.Sqrt(n) + 1;

        for (var i = 2 ; i < sqrt ; i++)
        {
            if (!isPrime[i])
            {
                continue;
            }

            //为什么从i*i开始,因为i*2,i*3这些数已经被i*2,i*3这些数标记过了
            for (var j = i * i ; j < n ; j += i)
            {
                isPrime[j] = false;
            }
        }

        return isPrime.Count(true) - 2;
    }



    public class Solution
    {
        [TestCase("010",  ExpectedResult = "001")]
        [TestCase("0101", ExpectedResult = "1001")]
        [Repeat(100000)]
        public string MaximumOddBinaryNumber(string s)
        {
            var popCount = -1;

            foreach (var c in s)
            {
                popCount += c - '0';
            }

            Span<char> span = s.ToCharArray();

            span.Fill('0');

            span[^1] = '1';

            for (var i = 0 ; i < popCount ; i++)
            {
                span[i] = '1';
            }


            return span.ToString();
        }

        [TestCase("010",  ExpectedResult = "001")]
        [TestCase("0101", ExpectedResult = "1001")]
        [Repeat(100000)]
        public string MaximumOddBinaryNumber2(string s)
        {
            // 计算原始字符串中1的个数
            var popCount = s.Count(c => c == '1');

            // 分配内存，并全部初始化为'0'
            Span<char> result = stackalloc char[s.Length];
            result.Fill('0');

            // 将足够数量的'1'放在左边
            for (var i = 0 ; i < popCount - 1 ; i++)
            {
                result[i] = '1';
            }

            // 确保二进制数是奇数
            result[^1] = '1';

            return new(result);
        }

        [TestCase("010",  ExpectedResult = "001")]
        [TestCase("0101", ExpectedResult = "1001")]
        [Repeat(100000)]
        public string MaximumOddBinaryNumber3(string s)
        {
            var zeroAndOne = new[] { 0, 0 };


            foreach (var c in s)
            {
                zeroAndOne[c - '0']++;
            }

            StringBuilder result = new();

            result.Append('1', zeroAndOne[1] - 1);
            result.Append('0', zeroAndOne[0]);


            return result.Append('1').ToString();
        }

        //
        // 2864. 最大二进制奇数   简单
        //     给你一个 二进制 字符串 s ，其中至少包含一个 '1' 。
        //
        // 你必须按某种方式 重新排列 字符串中的位，使得到的二进制数字是可以由该组合生成的 最大二进制奇数 。
        //
        // 以字符串形式，表示并返回可以由给定组合生成的最大二进制奇数。
        //
        // 注意 返回的结果字符串 可以 含前导零。
        //
        //
        //
        // 示例 1：
        //
        // 输入：s = "010"
        // 输出："001"
        // 解释：因为字符串 s 中仅有一个 '1' ，其必须出现在最后一位上。所以答案是 "001" 。
        // 示例 2：
        //
        // 输入：s = "0101"
        // 输出："1001"
        // 解释：其中一个 '1' 必须出现在最后一位上。而由剩下的数字可以生产的最大数字是 "100" 。所以答案是 "1001" 。
        //
        //
        // 提示：
        //
        // 1 <= s.length <= 100
        // s 仅由 '0' 和 '1' 组成
        //     s 中至少包含一个 '1'

        /// <summary>更具树生成数组</summary>
        /// <param name="root">树的根节点</param>
        /// <returns>层序遍历的数组</returns>
        public static int?[] CreateArray(TreeNode root)
        {
            if (root == null)
            {
                return Array.Empty<int?>();
            }

            var list  = new List<int?>();
            var queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node == null)
                {
                    list.Add(null);
                }
                else
                {
                    list.Add(node.val);
                    queue.Enqueue(node.left);
                    queue.Enqueue(node.right);
                }
            }

            return list.ToArray();
        }


        /// <summary>更具树生成数组字符串</summary>
        /// <param name="root">树的根节点</param>
        /// <returns>层序遍历的数组字符串</returns>
        public static string CreateArrayString(TreeNode root)
        {
            var array = CreateArray(root);
            // 移除末尾的null

            var count = array.Length;
            for (var i = array.Length - 1 ; i >= 0 ; i--)
            {
                if (array[i] == null)
                {
                    count--;
                }
                else
                {
                    break;
                }
            }

            Array.Resize(ref array, count);

            return $"[{string.Join(',', array.Select(x => x == null ? "null" : x.ToString()))}]";
        }
    }
}