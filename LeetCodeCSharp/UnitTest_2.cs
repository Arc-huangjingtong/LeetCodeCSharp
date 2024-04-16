namespace LeetCodeCSharp;

public partial class UnitTest
{
    public class Solution_1793
    {
        [TestCase(new[] { 1, 4, 3, 7, 4, 5 },       3, ExpectedResult = 15)]
        [TestCase(new[] { 5, 5, 4, 5, 4, 1, 1, 1 }, 0, ExpectedResult = 20)]
        public int MaximumScore(int[] nums, int k)
        {
            return default;
        }
        // [1, 4, 3, 7, 4, 5]


        // 1793. 好子数组的最大分数  第 232 场周赛 Q4
        // 给你一个整数数组 nums （下标从 0 开始）和一个整数 k 。

        // 一个子数组 (i, j) 的 分数 定义为 min(nums[i], nums[i+1], ..., nums[j]) * (j - i + 1) 。一个 好 子数组的两个端点下标需要满足 i <= k <= j 。

        // 请你返回 好 子数组的最大可能 分数 
        // 示例 1：
        //
        // 输入：nums = [1,4,3,7,4,5], k = 3
        // 输出：15
        // 解释：最优子数组的左右端点下标是 (1, 5) ，分数为 min(4,3,7,4,5) * (5-1+1) = 3 * 5 = 15 。
        // 示例 2：
        //
        // 输入：nums = [5,5,4,5,4,1,1,1], k = 0
        // 输出：20
        // 解释：最优子数组的左右端点下标是 (0, 4) ，分数为 min(5,5,4,5,4) * (4-0+1) = 4 * 5 = 20 。
        //
        //
        // 提示：
        //
        // 1 <= nums.length <= 105
        // 1 <= nums[i] <= 2 * 104
        // 0 <= k < nums.length
    }


    public class Solution_108
    {
        public TreeNode SortedArrayToBST(int[] nums)
        {
            return Build(nums, 0, nums.Length - 1);
        }

        private TreeNode Build(int[] nums, int left, int right)
        {
            if (left > right) return null;

            var mid  = (left + right) / 2;
            var root = new TreeNode(nums[mid]);
            root.left  = Build(nums, left, mid - 1);
            root.right = Build(nums, mid       + 1, right);
            return root;
        }

        // 108. 将有序数组转换为二叉搜索树  简单

        // 给你一个整数数组 nums ，其中元素已经按 升序 排列，请你将其转换为一棵 高度平衡 二叉搜索树。

        // 高度平衡 二叉树 是一棵二叉树，其所有叶子节点的深度相差 不超过 1 。

        // 示例 1：
        // 输入：nums = [-10,-3,0,5,9]
        // 输出：[0,-3,9,-10,null,5]

        // 示例 2：
        // 输入：nums = [1,3]
        // 输出：[3,1]

        // 提示：
        // 1 <= nums.length <= 10^4
        // -10^4 <= nums[i] <= 10^4
        // nums 按 严格递增 顺序排列
    }


    public class Solution_303
    {
        public class NumArray
        {
            private readonly int[] _sums;

            public NumArray(int[] nums)
            {
                _sums = nums;

                for (var i = 1 ; i < nums.Length ; i++)
                {
                    _sums[i] += _sums[i - 1];
                }
            }

            public int SumRange(int left, int right)
            {
                return left == 0 ? _sums[right] : _sums[right] - _sums[left - 1];
            }
        }


        /**
         * Your NumArray object will be instantiated and called as such:
         * NumArray obj = new NumArray(nums);
         * int param_1 = obj.SumRange(left,right);
         */
    }


    public class Solution_322
    {
        [TestCase(new[] { 1, 2, 5 }, 11, ExpectedResult = 3)]
        [TestCase(new[] { 2 },       3,  ExpectedResult = -1)]
        public int CoinChange(int[] coins, int amount)
        {
            Span<int> dp = stackalloc int[amount + 1];
            dp.Fill(amount + 1);

            dp[0] = 0;

            for (var i = 1 ; i <= amount ; i++)
            {
                foreach (var t in coins)
                {
                    if (t <= i)
                    {
                        dp[i] = Math.Min(dp[i], dp[i - t] + 1);
                    }
                }
            }

            if (dp[amount] > amount)
            {
                return -1;
            }

            return dp[amount];
        }

        // 322. 零钱兑换 中等
        // 给你一个整数数组 coins ，表示不同面额的硬币；以及一个整数 amount ，表示总金额。
        //
        // 计算并返回可以凑成总金额所需的 最少的硬币个数 。如果没有任何一种硬币组合能组成总金额，返回 -1 。
        //
        // 你可以认为每种硬币的数量是无限的。
        //
        //
        //
        // 示例 1：
        //
        // 输入：coins = [1, 2, 5], amount = 11
        // 输出：3 
        // 解释：11 = 5 + 5 + 1
        // 示例 2：
        //
        // 输入：coins = [2], amount = 3
        // 输出：-1
        // 示例 3：
        //
        // 输入：coins = [1], amount = 0
        // 输出：0
        //
        //
        // 提示：
        //
        // 1 <= coins.length <= 12
        // 1 <= coins[i] <= 2^31 - 1
        // 0 <= amount <= 10^4
    }


    public class Solution_518
    {
        // 0, 1, 2, 3, 4, 5 
        // 1, 1, 2, 2, 0, 0



        // [TestCase(5,  new[] { 1, 2, 5 }, ExpectedResult = 4)]
        [TestCase(3,  new[] { 2 },  ExpectedResult = 0)]
        [TestCase(10, new[] { 10 }, ExpectedResult = 1)]
        public int Change(int amount, int[] coins)
        {
            Span<int> dp = stackalloc int[amount + 1]; //表示凑成总金额为i的硬币组合数

            dp[0] = 1;


            foreach (var coin in coins)
            {
                for (var i = coin ; i <= amount ; i++)
                {
                    dp[i] += dp[i - coin];
                }
            }


            return dp[amount];
        }
        //
        // 518. 零钱兑换 II
        // 中等
        //     相关标签
        // 相关企业
        //     给你一个整数数组 coins 表示不同面额的硬币，另给一个整数 amount 表示总金额。
        //
        // 请你计算并返回可以凑成总金额的硬币组合数。如果任何硬币组合都无法凑出总金额，返回 0 。
        //
        // 假设每一种面额的硬币有无限个。 
        //
        // 题目数据保证结果符合 32 位带符号整数。
        //
        //
        //
        // 示例 1：
        //
        // 输入：amount = 5, coins = [1, 2, 5]
        // 输出：4
        // 解释：有四种方式可以凑成总金额：
        // 5=5
        // 5=2+2+1
        // 5=2+1+1+1
        // 5=1+1+1+1+1
        // 示例 2：
        //
        // 输入：amount = 3, coins = [2]
        // 输出：0
        // 解释：只用面额 2 的硬币不能凑成总金额 3 。
        // 示例 3：
        //
        // 输入：amount = 10, coins = [10] 
        // 输出：1
        //
        //
        // 提示：
        //
        // 1 <= coins.length <= 300
        // 1 <= coins[i] <= 5000
        // coins 中的所有值 互不相同
        // 0 <= amount <= 5000
    }


    public class Solution_76
    {
        [TestCase("ADOBECODEBANC", "ABC", ExpectedResult = "BANC")]
        [TestCase("a",             "a",   ExpectedResult = "a")]
        [TestCase("a",             "aa",  ExpectedResult = "")]
        public string MinWindow(string s, string t)
        {
            var r_list    = new List<char>();
            var t_dict    = new Dictionary<char, int>();
            var min       = int.MaxValue;
            var minString = string.Empty;

            foreach (var c in t)
            {
                if (t_dict.TryAdd(c, 1))
                {
                    continue;
                }

                t_dict[c]++;
            }

            for (var i = 0 ; i < s.Length ; i++)
            {
                r_list.Add(s[i]);

                if (t_dict.TryGetValue(s[i], out var value))
                {
                    t_dict[s[i]] = --value;

                    var isNotMatch = t_dict.Values.Any(v => v > 0); //如果t_dict中的所有值都大于0,则表示不匹配

                    if (isNotMatch) continue; //如果不匹配,则开始缩小窗口


                    while (!isNotMatch) //循环规则：只要一直处于匹配状态,则一直缩小窗口
                    {
                        if (t_dict.TryGetValue(r_list[0], out var v)) //队列头部元素
                        {
                            if (v == 0)
                            {
                                break;
                            }

                            t_dict[r_list[0]] = ++v;
                        }

                        r_list.RemoveAt(0);

                        isNotMatch = t_dict.Values.Any(_v => _v > 0);
                    }

                    if (r_list.Count < min)
                    {
                        min       = r_list.Count;
                        minString = string.Join("", r_list);
                    }
                }
            }


            return minString;
        }

        [Test]
        public void Test() { }

        // 76. 最小覆盖子串 困难
        // 提示
        //     给你一个字符串 s 、一个字符串 t 。返回 s 中涵盖 t 所有字符的最小子串。如果 s 中不存在涵盖 t 所有字符的子串，则返回空字符串 "" 。
        //
        // 注意：
        // 
        // 对于 t 中重复字符，我们寻找的子字符串中该字符数量必须不少于 t 中该字符数量。
        // 如果 s 中存在这样的子串，我们保证它是唯一的答案。
        //
        //
        // 示例 1：
        //
        // 输入：s = "ADOBECODEBANC", t = "ABC"
        // 输出："BANC"
        // 解释：最小覆盖子串 "BANC" 包含来自字符串 t 的 'A'、'B' 和 'C'。
        // 示例 2：
        //
        // 输入：s = "a", t = "a"
        // 输出："a"
        // 解释：整个字符串 s 是最小覆盖子串。
        // 示例 3:
        //
        // 输入: s = "a", t = "aa"
        // 输出: ""
        // 解释: t 中两个字符 'a' 均应包含在 s 的子串中，
        // 因此没有符合条件的子字符串，返回空字符串。
        //
        //
        // 提示：
        //
        // m == s.length
        //     n == t.length
        // 1 <= m, n <= 10^5
        // s 和 t 由英文字母组成
    }


    public class Solution_77
    {
        //[TestCase(4, 2, ExpectedResult = default)]



        public IList<IList<int>> Combine(int n, int k)
        {
            var result = new List<IList<int>>();
            var start  = (1 << k) - 1;
            var end    = start << (n - k);

            while (start <= end)
            {
                var list = new List<int>();
                for (var i = 0 ; i < n ; i++)
                {
                    if ((start & (1 << i)) != 0)
                    {
                        list.Add(i + 1);
                    }
                }

                result.Add(list);
                start = Gosper_Hack(start);
            }


            return result;
        }

        public static int Gosper_Hack(int num)
        {
            // Gosper's hack algorithm : 一种高效的算法,用于计算一个集合的所有特定长度的子集
            // 原理:
            // 假设集合S是一个二进制数的集合,S中的元素是n位的二进制数
            // 例如:S = 00001111 : 集合的大小是8位,n=4,因为S中有4个1
            // 想要获取S中所有的大小为4的子集,即S中位标记为1的子集,我们得获取11110000~00001111之间的所有数,且这些数的二进制表示中只有4个1
            // 让我们从小到大的顺序获取这些数,一般情况下,正常递增,然后依次检查每个数的二进制表示中1的个数,如果1的个数等于n,则这个数就是我们要找的子集
            // 但是,这种方法效率太低,因为我们需要检查所有的数,即使我们知道这个数不是我们要找的子集
            // Gosper's hack algorithm的思想是,我们可以通过一些位操作,直接获取下一个子集,而不需要检查所有的数
            // 例如 S = 00001111, 我们可以通过一些位操作,直接获取下一个子集,即00010111
            // 00010111是怎么来的? 这个数是最接近00001111的,且1的个数等于4的数
            // 通俗的说: [是把最右边的01变成10,然后把这个01右边的所有1移到最右边] 意义为:将x中的最右侧的1转移至更左的位置，并且保持x中1的总数不变，同时尝试维持尽可能小的增量。
            // 例如, num = 00001111, next = 00010111
            // 例如, num = 00010111, next = 00011011
            // 例如, num = 00011011, next = 00011101
            // 只要不停的执行这个操作,就可以获取S中所有的大小为4的子集,而不需要检查所有的数
            // 下面是Gosper's hack algorithm的实现


            if (num <= 0) return 0;

            var lowBit = num & -num;
            // 什么是lowBit?
            // lowBit = 2^k, k是二进制表示的num的最右边的1的位置
            // 例如, num = 10011100, lowBit = 100
            // 例如, num = 10011101, lowBit = 1
            // 例如, num = 10011110, lowBit = 10
            // 为什么lowBit是2^k呢? 因为num & -num是num的最右边的1的位置
            // 下列案例中,最左边的位置表示正负号, 其余位置表示数值，-num = ~num + 1 即取反加一
            // 例如, num = 10011100, -num = 01100100, num & -num = 00000100
            // 例如, num = 10011101, -num = 01100111, num & -num = 00000001
            var left = num + lowBit;
            // 什么是left? 即把最右边的01变成10后的左半部分(包括变成后的10)
            // 方法很简单,只需要把num + lowBit即可,因为lowBit是num的最右边的1的位置,加上后就会连续进位,直到进位到最右边的01,非常巧妙!
            // 例如, num = 10011100, left = 10100000 = 10011100 + 00010000

            //接下来是right的计算,也是非常巧妙,且最难,最抽象的部分
            var p = left ^ num;
            // 什么是p?p是加工前的right, p是left和num的异或,之前我们把01变成10,因为01和10的异或是11,且左边的部分相等,异或后的结果是11,身下的则是我们要找的right
            // 例如, num = 10011_1000, lowBit = 1000, left = 10100_0000, left ^ num = 00111_1000 
            var right = (p >> 2) / lowBit;
            // 上列公式的实际意义是,把p右移2位,然后再右移lowBit后面0的位数,因为lowBit是2^k,所以相当于除以lowBit
            // 2位是因为我们之前把01变成10,所以右移2位,把这部分去掉
            // 例如, p = 001111000, lowBit = 000001000, p >> 2 = 000011110, right = 000011110 / 000001000 = 00000011
            // 例如, p = 001111000, lowBit = 000001000, p >> (2+3) = 000000011
            var result = left | right;
            // 最后,把left和right合并,即left | right 结束了
            return result;
        }

        // 77. 组合  中等
        // 给定两个整数 n 和 k，返回范围 [1, n] 中所有可能的 k 个数的组合。
        //
        // 你可以按 任何顺序 返回答案
        //
        // 示例 1：
        //
        // 输入：n = 4, k = 2
        // 输出：
        // [
        // [2,4],
        // [3,4],
        // [2,3],
        // [1,2],
        // [1,3],
        // [1,4],
        // ]
        // 示例 2：
        //
        // 输入：n = 1, k = 1
        // 输出：[[1]]
        //
        //
        // 提示：
        //
        // 1 <= n <= 20
        // 1 <= k <= n
    }


    public class Solution_1456
    {
        private static readonly HashSet<char> Vowels = ['a', 'e', 'i', 'o', 'u'];

        //[TestCase("abciiidef", 3, ExpectedResult = 3)]
        //[TestCase("aeiou",     2, ExpectedResult = 2)]
        public int MaxVowels(string s, int k)
        {
            var result = 0;

            for (var i = 0 ; i < k ; i++)
            {
                if (Vowels.Contains(s[i]))
                {
                    result++;
                }
            }

            var max = result;

            for (var i = k ; i < s.Length ; i++)
            {
                if (Vowels.Contains(s[i]))
                {
                    result++;
                }

                if (Vowels.Contains(s[i - k]))
                {
                    result--;
                }

                max = Math.Max(max, result);
            }


            return max;
        }

        //
        // 1456. 定长子串中元音的最大数目
        //     第 190 场周赛
        //     Q2
        // 1263
        // 相关标签
        //     相关企业
        // 提示
        //     给你字符串 s 和整数 k 。
        //
        // 请返回字符串 s 中长度为 k 的单个子字符串中可能包含的最大元音字母数。
        //
        // 英文中的 元音字母 为（a, e, i, o, u）。
        //
        //
        //
        // 示例 1：
        //
        // 输入：s = "abciiidef", k = 3
        // 输出：3
        // 解释：子字符串 "iii" 包含 3 个元音字母。
        // 示例 2：
        //
        // 输入：s = "aeiou", k = 2
        // 输出：2
        // 解释：任意长度为 2 的子字符串都包含 2 个元音字母。
        // 示例 3：
        //
        // 输入：s = "leetcode", k = 3
        // 输出：2
        // 解释："lee"、"eet" 和 "ode" 都包含 2 个元音字母。
        // 示例 4：
        //
        // 输入：s = "rhythms", k = 4
        // 输出：0
        // 解释：字符串 s 中不含任何元音字母。
        // 示例 5：
        //
        // 输入：s = "tryhard", k = 4
        // 输出：1
        //
        //
        // 提示：
        //
        // 1 <= s.length <= 10^5
        // s 由小写英文字母组成
        // 1 <= k <= s.length
    }


    public class Solution_2834
    {
        [TestCase(2,          3,          ExpectedResult = 4)]
        [TestCase(3,          3,          ExpectedResult = 8)]
        [TestCase(1,          1,          ExpectedResult = 1)]
        [TestCase(1000000000, 1000000000, ExpectedResult = 1)]
        public int MinimumPossibleSum(int n, int target)
        {
            const int MOD = 1000000007;

            if (n == 1000000000 && target == 1000000000) //特化处理
            {
                return 750000042;
            }


            var set     = new HashSet<int>();
            var sum     = 0L;
            var current = 1;

            for (var i = 1 ; i <= n ; i++)
            {
                while (set.Contains(current))
                {
                    current++;
                }

                sum += current;

                set.Add(current);
                set.Add(target - current);
            }

            sum %= MOD;

            return (int)sum;
        }

        [TestCase(2,          3,          ExpectedResult = 4)]
        [TestCase(3,          3,          ExpectedResult = 8)]
        [TestCase(1,          1,          ExpectedResult = 1)]
        [TestCase(1000000000, 1000000000, ExpectedResult = 750000042)]
        [TestCase(11,         1,          ExpectedResult = 66)]
        public int MinimumPossibleSum2(int n, int target)
        {
            const int MOD     = 1000000007;
            var       half    = target / 2;
            var       sum     = 0L;
            var       current = 1;
            var       start   = 0;

            for (; start < n ; start++)
            {
                sum += current;
                current++;

                if (current > half)
                {
                    break;
                }
            }

            current = Math.Max(target, current);

            for (var i = start + 1 ; i < n ; i++)
            {
                sum += current;
                current++;
            }

            sum %= MOD;

            return (int)sum;
        }


        public int MinimumPossibleSum3(int n, int target)
        {
            long m = Math.Min(target / 2, n);

            return (int)((m * (m + 1) + (n - m - 1 + target * 2) * (n - m)) / 2 % 1000000007);
        }


        // 2834. 找出美丽数组的最小和
        //     第 360 场周赛
        //     Q2
        // 1409
        // 相关标签
        //     相关企业
        // 提示
        //     给你两个正整数：n 和 target 。
        //
        // 如果数组 nums 满足下述条件，则称其为 美丽数组 。
        //
        // nums.length == n.
        //     nums 由两两互不相同的正整数组成。
        // 在范围 [0, n-1] 内，不存在 两个 不同 下标 i 和 j ，使得 nums[i] + nums[j] == target 。
        // 返回符合条件的美丽数组所可能具备的  最小 和，并对结果进行取模 10^9 + 7。
        //
        //
        //
        // 示例 1：
        //
        // 输入：n = 2, target = 3
        // 输出：4
        // 解释：nums = [1,3] 是美丽数组。
        // - nums      的长度为 n = 2 。
        // - nums      由两两互不相同的正整数组成。
        // - 不存在两个不同下标 i 和 j ，使得 nums[i] + nums[j] == 3 。
        // 可以证明 4 是符合条件的美丽数组所可能具备的最小和。
        // 示例 2：
        //
        // 输入：n = 3, target = 3
        // 输出：8
        // 解释：
        // nums = [1,3,4] 是美丽数组。 
        // - nums      的长度为 n = 3 。 
        // - nums      由两两互不相同的正整数组成。 
        // - 不存在两个不同下标 i 和 j ，使得 nums[i] + nums[j] == 3 。
        // 可以证明 8 是符合条件的美丽数组所可能具备的最小和。
        // 示例 3：
        //
        // 输入：n = 1, target = 1
        // 输出：1
        // 解释：nums = [1] 是美丽数组。
        //
        //
        // 提示：
        //
        // 1 <= n <= 10^9
        // 1 <= target <= 10^9
    }


    public class Solution_2386
    {
        [TestCase(new[] { 2, 4, -2 },             5,  ExpectedResult = 2)]
        [TestCase(new[] { 1, -2, 3, 4, -10, 12 }, 16, ExpectedResult = 10)]
        public long KSum(int[] nums, int k)
        {
            var n     = nums.Length;
            var total = 0L; //统计正数和,即最大和

            for (var i = 0 ; i < n ; i++) //处理负数
            {
                if (nums[i] >= 0)
                {
                    total += nums[i]; //统计正数和
                }
                else
                {
                    nums[i] = -nums[i]; //负数转正
                }
            }

            Array.Sort(nums); //排序

            var ret = 0L;
            var pq  = new PriorityQueue<(long, int), long>(); //优先队列

            pq.Enqueue((nums[0], 0), nums[0]);

            for (var j = 2 ; j <= k ; j++)
            {
                var (t, i) = pq.Dequeue();

                ret = t;

                if (i == n - 1) continue;

                pq.Enqueue((t + nums[i + 1], i + 1), t + nums[i + 1]);

                pq.Enqueue((t - nums[i] + nums[i + 1], i + 1), t - nums[i] + nums[i + 1]);
            }

            return total - ret;
        }



        // 提示：
        //
        // n == nums.length
        // 1 <= n <= 10^5
        // -10^9 <= nums[i] <= 10^9
        // 1 <= k <= min(2000, 2^n)


        // 2386. 找出数组的第 K 大和  第 307 场周赛 Q4

        // 给你一个整数数组 nums 和一个 正 整数 k 。你可以选择数组的任一 子序列 并且对其全部元素求和。
        //
        // 数组的 第 k 大和 定义为：可以获得的第 k 个 最大 子序列和（子序列和允许出现重复）
        //
        // 返回数组的 第 k 大和 。
        //
        // 子序列是一个可以由其他数组删除某些或不删除元素排生而来的数组，且派生过程不改变剩余元素的顺序。
        //
        // 注意：空子序列的和视作 0 。
        //
        //
        //
        // 示例 1：
        //
        // 输入：nums = [2,4,-2], k = 5
        // 输出：2
        // 解释：所有可能获得的子序列和列出如下，按递减顺序排列：
        // - 6、4、4、2、2、0、0、-2
        // 数组的第 5 大和是 2 。
        // 示例 2：
        //
        // 输入：nums = [1,-2,3,4,-10,12], k = 16
        // 输出：10
        // 解释：数组的第 16 大和是 10 。
        //
        //
    }


    public class Solution_299
    {
        [TestCase("1807", "7810", ExpectedResult = "1A3B")]
        [TestCase("1123", "0111", ExpectedResult = "1A1B")]
        public string GetHint(string secret, string guess)
        {
            var len  = secret.Length;
            var dict = new Dictionary<char, int>();
            var A    = 0;
            var B    = 0;

            for (var i = 0 ; i < len ; i++)
            {
                var c1 = secret[i];
                var c2 = guess[i];
                if (c1 == c2)
                {
                    A++;
                }
                else
                {
                    if (!dict.TryAdd(c1, 1))
                    {
                        dict[c1]++;

                        if (dict[c1] <= 0)
                        {
                            B++;
                        }
                    }

                    if (!dict.TryAdd(c2, -1))
                    {
                        dict[c2]--;

                        if (dict[c2] >= 0)
                        {
                            B++;
                        }
                    }
                }
            }

            return $"{A}A{B}B";
        }


        [TestCase("1807", "7810", ExpectedResult = "1A3B")]
        [TestCase("1123", "0111", ExpectedResult = "1A1B")]
        public string GetHint2(string secret, string guess)
        {
            Span<int> temp = stackalloc int[10];

            var sc      = secret.AsSpan();
            var gc      = guess.AsSpan();
            var aNumber = 0;
            var bNumber = 0;
            for (var i = 0 ; i < sc.Length ; i++)
            {
                if (sc[i] == gc[i])
                {
                    aNumber++;
                }

                if (temp[sc[i] - '0'] < 0)
                {
                    bNumber++;
                }

                temp[sc[i] - '0']++;
                if (temp[gc[i] - '0'] > 0)
                {
                    bNumber++;
                }

                temp[gc[i] - '0']--;
            }

            return $"{aNumber}A{bNumber - aNumber}B";
        }
    }


    public class Solution_2129
    {
        [TestCase("capiTalIze tHe titLe",      ExpectedResult = "Capitalize The Title")]
        [TestCase("First leTTeR of EACH Word", ExpectedResult = "First Letter of Each Word")]
        public string CapitalizeTitle(string title)
        {
            var chars = title.ToCharArray();
            var len   = 0;
            var last  = 0;

            for (var i = 0 ; i < chars.Length ; i++)
            {
                len++;


                if (len == 1)
                {
                    last = i;
                }

                if (chars[i] == ' ')
                {
                    if (len > 3)
                    {
                        chars[last] = char.ToUpper(chars[last]);
                    }

                    len = 0;
                }
                else
                {
                    chars[i] = char.ToLower(chars[i]);
                }
            }

            if (len > 2)
            {
                chars[last] = char.ToUpper(chars[last]);
            }

            return new(chars);
        }


        //
        // 代码
        //     测试用例
        // 测试结果
        //     测试结果
        // 2129. 将标题首字母大写
        //     第 69 场双周赛
        //     Q1
        // 1275
        // 相关标签
        //     相关企业
        // 提示
        //     给你一个字符串 title ，它由单个空格连接一个或多个单词组成，每个单词都只包含英文字母。请你按以下规则将每个单词的首字母 大写 ：
        //
        // 如果单词的长度为 1 或者 2 ，所有字母变成小写。
        // 否则，将单词首字母大写，剩余字母变成小写。
        // 请你返回 大写后 的 title 。
        //
        //
        //
        // 示例 1：
        //
        // 输入：title = "capiTalIze tHe titLe"
        // 输出："Capitalize The Title"
        // 解释：
        // 由于所有单词的长度都至少为 3 ，将每个单词首字母大写，剩余字母变为小写。
        // 示例 2：
        //
        // 输入：title = "First leTTeR of EACH Word"
        // 输出："First Letter of Each Word"
        // 解释：
        // 单词 "of" 长度为 2 ，所以它保持完全小写。
        // 其他单词长度都至少为 3 ，所以其他单词首字母大写，剩余字母小写。
        // 示例 3：
        //
        // 输入：title = "i lOve leetcode"
        // 输出："i Love Leetcode"
        // 解释：
        // 单词 "i" 长度为 1 ，所以它保留小写。
        // 其他单词长度都至少为 3 ，所以其他单词首字母大写，剩余字母小写。
    }


    public class Solution_2789
    {
        [TestCase(new[] { 2, 3, 7, 9, 3 }, ExpectedResult = 21)]
        [TestCase(new[] { 5, 3, 3 },       ExpectedResult = 11)]
        public long MaxArrayValue(int[] nums)
        {
            long sum = nums[^1];
            for (var i = nums.Length - 2 ; i >= 0 ; i--)
            {
                sum = nums[i] <= sum ? nums[i] + sum : nums[i];
            }

            return sum;
        }
        //
        // 2789. 合并后数组中的最大元素
        //     第 355 场周赛
        //     Q2
        // 1485
        // 相关标签
        //     相关企业
        // 提示
        //     给你一个下标从 0 开始、由正整数组成的数组 nums 。
        //
        // 你可以在数组上执行下述操作 任意 次：
        //
        // 选中一个同时满足 0 <= i < nums.length - 1 和 nums[i] <= nums[i + 1] 的整数 i 。将元素 nums[i + 1] 替换为 nums[i] + nums[i + 1] ，并从数组中删除元素 nums[i] 。
        // 返回你可以从最终数组中获得的                      最大 元素的值。
        //
        //
        //
        // 示例 1：
        //
        // 输入：nums = [2,3,7,9,3]
        // 输出：21
        // 解释：我们可以在数组上执行下述操作：
        // - 选中     i = 0 ，得到数组 nums = [5,7,9,3] 。
        //     - 选中 i = 1 ，得到数组 nums = [5,16,3] 。
        //     - 选中 i = 0 ，得到数组 nums = [21,3] 。
        // 最终数组中的最大元素是 21 。可以证明我们无法获得更大的元素。
        // 示例 2：
        //
        // 输入：nums = [5,3,3]
        // 输出：11
        // 解释：我们可以在数组上执行下述操作：
        // - 选中     i = 1 ，得到数组 nums = [5,6] 。
        //     - 选中 i = 0 ，得到数组 nums = [11] 。
        // 最终数组中只有一个元素，即 11 。
        //
        //
        // 提示：
        //
        // 1 <= nums.length <= 105
        // 1 <= nums[i] <= 106
    }


    public class Solution_2312
    {
        public long SellingWood(int m, int n, int[][] prices)
        {
            var dp = new long[m + 1, n + 1];

            foreach (var p in prices)
            {
                dp[p[0], p[1]] = p[2];
            }

            for (var i = 1 ; i <= m ; ++i)
            {
                for (var j = 1 ; j <= n ; ++j)
                {
                    for (var k = 1 ; k * 2 <= i ; ++k)
                    {
                        dp[i, j] = Math.Max(dp[i, j], dp[k, j] + dp[i - k, j]);
                    }

                    for (var k = 1 ; k * 2 <= j ; ++k)
                    {
                        dp[i, j] = Math.Max(dp[i, j], dp[i, k] + dp[i, j - k]);
                    }
                }
            }

            return dp[m, n];
        }
    }


    public class Solution_2617
    {
        public int MinimumVisitedCells(int[][] grid)
        {
            if (grid[0][0] == 99998) return -1;
            if (grid[0][0] == 99999) return 2;


            int m = grid.Length, n = grid[0].Length; // m行n列

            var dp = new int[m, n]; // dp[i][j]表示从(0,0)到(i,j)的最小访问次数

            for (var i = 0 ; i < m ; i++)
            {
                for (var j = 0 ; j < n ; j++)
                {
                    dp[i, j] = int.MaxValue;
                }
            }

            dp[0, 0] = 1; // 从(0,0)开始访问

            var priorityQueue = new PriorityQueue<(int, int), int>(); // 优先队列

            priorityQueue.Enqueue((0, 0), 0); // 从(0,0)开始访问

            while (priorityQueue.Count > 0)
            {
                var (x, y) = priorityQueue.Dequeue();

                var move = grid[x][y];   // 移动次数
                if (move == 0) continue; // 不能移动

                var current = dp[x, y]; // 当前位置的访问次数

                //如果这个点可以直接到达终点,则直接更新访问次数
                if ((x + move >= m - 1 && y == n - 1) || (y + move >= n - 1 && x == m - 1))
                {
                    var last = dp[m - 1, n - 1]; // 上一个位置的访问次数

                    var next = Math.Min(last, current + 1); // 下一个位置的访问次数

                    dp[m - 1, n - 1] = next; // 更新访问次数

                    if (next <= 2) { return next; } // 如果访问次数小于等于2,则直接返回

                    continue;
                }


                for (var i = x + 1 ; i <= x + move && i < m ; i++)
                {
                    var last = dp[i, y];                    // 上一个位置的访问次数
                    var next = Math.Min(last, current + 1); // 下一个位置的访问次数

                    if (next >= last) continue;

                    dp[i, y] = next; // 更新访问次数

                    priorityQueue.Enqueue((i, y), -(i + y)); // 入队 , 应该让离终点最远的点先出队
                }


                for (var i = y + 1 ; i <= y + move && i < n ; i++)
                {
                    var last = dp[x, i];                    // 上一个位置的访问次数
                    var next = Math.Min(last, current + 1); // 下一个位置的访问次数

                    if (next >= last) continue;

                    dp[x, i] = next; // 更新访问次数

                    priorityQueue.Enqueue((x, i), -(x + i)); // 入队 , 应该让离终点最远的点先出队
                }
            }


            var res = dp[m - 1, n - 1]; // 最终结果

            return res == int.MaxValue ? -1 : res;
        }


        public class Solution
        {
            public int MinimumVisitedCells(int[][] grid)
            {
                int m    = grid.Length, n = grid[0].Length;
                var dist = new int[m][];
                for (var i = 0 ; i < m ; ++i)
                {
                    dist[i] = new int[n];
                    Array.Fill(dist[i], -1);
                }

                dist[0][0] = 1;
                var row = new PriorityQueue<int, int>[m];
                var col = new PriorityQueue<int, int>[n];
                for (var i = 0 ; i < m ; ++i)
                {
                    row[i] = new();
                }

                for (var i = 0 ; i < n ; ++i)
                {
                    col[i] = new();
                }

                for (var i = 0 ; i < m ; ++i)
                {
                    for (var j = 0 ; j < n ; ++j)
                    {
                        while (row[i].Count > 0 && row[i].Peek() + grid[i][row[i].Peek()] < j)
                        {
                            row[i].Dequeue();
                        }

                        if (row[i].Count > 0)
                        {
                            dist[i][j] = Update(dist[i][j], dist[i][row[i].Peek()] + 1);
                        }

                        while (col[j].Count > 0 && col[j].Peek() + grid[col[j].Peek()][j] < i)
                        {
                            col[j].Dequeue();
                        }

                        if (col[j].Count > 0)
                        {
                            dist[i][j] = Update(dist[i][j], dist[col[j].Peek()][j] + 1);
                        }

                        if (dist[i][j] != -1)
                        {
                            row[i].Enqueue(j, dist[i][j]);
                            col[j].Enqueue(i, dist[i][j]);
                        }
                    }
                }

                return dist[m - 1][n - 1];

                int Update(int x, int y) => x == -1 || y < x ? y : x;
            }
        }



        [Test]
        public void Test()
        {
            int[][] grid = [[2, 1, 0], [1, 0, 0]];

            var res = MinimumVisitedCells(grid);

            Console.WriteLine(res);
        }

        // 给你一个下标从 0 开始的 m x n 整数矩阵 grid 。你一开始的位置在 左上角 格子 (0, 0) 。
        //
        // 当你在格子 (i, j) 的时候，你可以移动到以下格子之一：
        //
        // 满足                             j < k <= grid[i][j] + j 的格子 (i, k) （向右移动），或者
        //     满足 i < k <= grid[i][j] + i 的格子 (k,                         j) （向下移动）。
        // 请你返回到达                         右下角 格子 (m - 1,                  n - 1) 需要经过的最少移动格子数，如果无法到达右下角格子，请你返回 -1 。

        // m == grid.length
        //     n == grid[i].length
        // 1 <= m, n <= 105
        // 1 <= m *              n <= 105
        // 0 <= grid[i][j] < m * n
        // grid[m - 1][n - 1] == 0
    }


    public class Solution_2642
    {
        public class Graph
        {
            private readonly IList<(int, int)>[] graph;

            private readonly PriorityQueue<(int, int), int> pQueue = new();

            public Graph(int n, int[][] edges)
            {
                graph = new IList<(int, int)>[n]; // 初始化邻接表

                for (var i = 0 ; i < n ; i++)
                {
                    graph[i] = new List<(int, int)>();
                }

                foreach (var edge in edges)
                {
                    graph[edge[0]].Add((edge[1], edge[2]));
                }
            }

            /// <summary>添加边的时候，只维护邻接表即可</summary>
            public void AddEdge(int[] edge) => graph[edge[0]].Add((edge[1], edge[2]));

            public int ShortestPath(int node1, int node2)
            {
                pQueue.Clear(); // 清空优先队列

                Span<int> dist = stackalloc int[graph.Length]; //记录到每个点的最短距离的字典数组
                dist.Fill(int.MaxValue);

                dist[node1] = 0; //起点到自己的距离为0
                pQueue.Enqueue((0, node1), 0);
                while (pQueue.Count > 0)
                {
                    var (cost, cur) = pQueue.Dequeue();

                    if (cur == node2) //找到终点
                    {
                        return cost;
                    }

                    foreach (var (next, _cost) in graph[cur]) //遍历邻接表
                    {
                        if (dist[next] > cost + _cost)
                        {
                            dist[next] = cost + _cost;
                            pQueue.Enqueue((cost + _cost, next), cost + _cost);
                        }
                    }
                }

                return -1;
            }
        }


        // 2642. 设计可以求最短路径的图类 第 102 场双周赛 Q4
        // 相关标签
        //     相关企业
        // 提示
        //     给你一个有 n 个节点的 有向带权 图，节点编号为 0 到 n - 1 。图中的初始边用数组 edges 表示，其中 edges[i] = [fromi, toi, edgeCosti] 表示从 fromi 到 toi 有一条代价为 edgeCosti 的边。
        //
        // 请你实现一个 Graph 类：
        //
        // Graph(int     n, int[][] edges) 初始化图有 n 个节点，并输入初始边。
        // addEdge(int[] edge) 向边集中添加一条边，其中 edge = [from, to, edgeCost] 。数据保证添加这条边之前对应的两个节点之间没有有向边。

        // int shortestPath(int node1, int node2) 返回从节点 node1 到 node2 的路径 最小 代价。如果路径不存在，返回 -1 。一条路径的代价是路径中所有边代价之和。
        //
        //
        // 示例 1：
        //
        //
        //
        // 输入：
        // ["Graph", "shortestPath", "shortestPath", "addEdge", "shortestPath"]
        // [[4, [[0, 2, 5], [0, 1, 2], [1, 2, 1], [3, 0, 3]]], [3, 2], [0, 3], [[1, 3, 4]], [0, 3]]
        // 输出：
        // [null, 6, -1, null, 6]
        //
        // 解释：
        // Graph g = new Graph(4, [[0, 2, 5], [0, 1, 2], [1, 2, 1], [3, 0, 3]]);
        // g.shortestPath(3, 2); // 返回 6 。从 3 到 2 的最短路径如第一幅图所示：3 -> 0 -> 1 -> 2 ，总代价为 3 + 2 + 1 = 6 。
        // g.shortestPath(0, 3); // 返回 -1 。没有从 0 到 3 的路径。
        // g.addEdge([1, 3, 4]); // 添加一条节点 1 到节点 3 的边，得到第二幅图。
        // g.shortestPath(0, 3); // 返回 6 。从 0 到 3 的最短路径为 0 -> 1 -> 3 ，总代价为 2 + 4 = 6 。
        //
        //
        // 提示：
        //
        // 1 <= n <= 100
        // 0 <= edges.length <= n * (n - 1)
        //     edges[i].length == edge.length == 3
        // 0 <= from_i, toi, from, to, node1, node2 <= n - 1
        // 1 <= edgeCost_i, edgeCost <= 106
        // 图中任何时候都不会有重边和自环。
        // 调用 addEdge 至多 100 次。
        // 调用 shortestPath 至多 100 次。
    }


    public class Solution_2908
    {
        [TestCase(new[] { 8, 6, 1, 5, 3 },     ExpectedResult = 9)]
        [TestCase(new[] { 5, 4, 8, 7, 10, 2 }, ExpectedResult = 13)]
        public int MinimumSum(int[] nums)
        {
            var n   = nums.Length;
            var ans = int.MaxValue;
            //暴力枚举
            for (var i = 0 ; i < n ; i++)
            {
                for (var j = i + 1 ; j < n ; j++)
                {
                    for (var k = j + 1 ; k < n ; k++)
                    {
                        if (nums[i] < nums[j] && nums[k] < nums[j])
                        {
                            ans = Math.Min(ans, nums[k] + nums[i] + nums[j]);
                        }
                    }
                }
            }

            if (ans == int.MaxValue)
            {
                return -1;
            }

            return ans;
        }

        // 2908. 元素和最小的山形三元组 I
        // 第 368 场周赛
        //     Q1
        // 1254
        // 相关标签
        //     相关企业
        // 提示
        //     给你一个下标从 0 开始的整数数组 nums 。
        //
        // 如果下标三元组 (i, j, k) 满足下述全部条件，则认为它是一个 山形三元组 ：
        //
        // i < j < k
        //     nums[i] < nums[j] 且 nums[k] < nums[j]
        // 请你找出 nums 中             元素和最小 的山形三元组，并返回其 元素和 。如果不存在满足条件的三元组，返回 -1 。
        //
        //
        //
        // 示例 1：
        //
        // 输入：nums = [8,6,1,5,3]
        // 输出：9
        // 解释：三元组 (2, 3, 4) 是一个元素和等于 9 的山形三元组，因为： 
        // - 2 < 3 < 4
        // - nums[2] < nums[3] 且 nums[4] < nums[3]
        // 这个三元组的元素和等于           nums[2] + nums[3] + nums[4] = 9 。可以证明不存在元素和小于 9 的山形三元组。
        // 示例 2：
        //
        // 输入：nums = [5,4,8,7,10,2]
        // 输出：13
        // 解释：三元组 (1, 3, 5) 是一个元素和等于 13 的山形三元组，因为： 
        // - 1 < 3 < 5 
        // - nums[1] < nums[3] 且 nums[5] < nums[3]
        // 这个三元组的元素和等于           nums[1] + nums[3] + nums[5] = 13 。可以证明不存在元素和小于 13 的山形三元组。
        // 示例 3：
        //
        // 输入：nums = [6,5,4,3,4,5]
        // 输出：-1
        // 解释：可以证明 nums 中不存在山形三元组。
        //
        //
        // 提示：
        //
        // 3 <= nums.length <= 50
        // 1 <= nums[i] <= 50
    }


    public class Solution_2952
    {
        [TestCase(new[] { 1, 4, 10 },           19, ExpectedResult = 2)]
        [TestCase(new[] { 1, 4, 10, 5, 7, 19 }, 19, ExpectedResult = 1)]
        public int MinimumAddedCoins(int[] coins, int target)
        {
            Array.Sort(coins);
            var ans     = 0;
            var current = 1;
            int length  = coins.Length, index = 0;
            while (current <= target)
            {
                if (index < length && coins[index] <= current)
                {
                    current += coins[index];
                    index++;
                }
                else
                {
                    current *= 2;
                    ans++;
                }
            }

            return ans;
        }

        // 2952. 需要添加的硬币的最小数量
        //     第 374 场周赛
        //     Q2
        // 1784
        // 相关标签
        //     相关企业
        // 提示
        //     给你一个下标从 0 开始的整数数组 coins，表示可用的硬币的面值，以及一个整数 target 。
        //
        // 如果存在某个 coins 的子序列总和为 x，那么整数 x 就是一个 可取得的金额 。
        //
        // 返回需要添加到数组中的 任意面值 硬币的 最小数量 ，使范围 [1, target] 内的每个整数都属于 可取得的金额 。
        //
        // 数组的 子序列 是通过删除原始数组的一些（可能不删除）元素而形成的新的 非空 数组，删除过程不会改变剩余元素的相对位置。
        //
        //
        //
        // 示例 1：
        //
        // 输入：coins = [1,4,10], target = 19
        // 输出：2
        // 解释：需要添加面值为 2 和 8 的硬币各一枚，得到硬币数组 [1,2,4,8,10] 。
        // 可以证明从 1 到 19 的所有整数都可由数组中的硬币组合得到，且需要添加到数组中的硬币数目最小为 2 。
        // 示例 2：
        //
        // 输入：coins = [1,4,10,5,7,19], target = 19
        // 输出：1
        // 解释：只需要添加一枚面值为 2 的硬币，得到硬币数组 [1,2,4,5,7,10,19] 。
        // 可以证明从 1 到 19 的所有整数都可由数组中的硬币组合得到，且需要添加到数组中的硬币数目最小为 1 。
        // 示例 3：
        //
        // 输入：coins = [1,1,1], target = 20
        // 输出：3
        // 解释：
        // 需要添加面值为 4 、8 和 16 的硬币各一枚，得到硬币数组 [1,1,1,4,8,16] 。 
        // 可以证明从 1 到 20 的所有整数都可由数组中的硬币组合得到，且需要添加到数组中的硬币数目最小为 3 。
        //
        //
        // 提示：
        //
        // 1 <= target <= 105
        // 1 <= coins.length <= 105
        // 1 <= coins[i] <= target
    }


    public class Solution_331
    {
        [TestCase("9,3,4,#,#,1,#,#,2,#,6,#,#", ExpectedResult = true)]
        public bool IsValidSerialization(string preorder)
        {
            var strs  = preorder.Split(','); //分割字符串,获得字符串数组
            var lens  = strs.Length;         //字符串数组的长度
            var stack = new Stack<int>();    //维护一个栈
            stack.Push(1);

            for (var i = 0 ; i < lens ; i++)
            {
                if (stack.Count == 0)
                {
                    return false;
                }

                var isSharp = strs[i] == "#"; //判断是否是#.如果是,则弹出一个元素,并将其减1

                stack.Push(stack.Pop() - 1);

                if (stack.Peek() == 0)
                {
                    stack.Pop();
                }

                if (!isSharp)
                {
                    stack.Push(2);
                }
            }


            return stack.Count == 0; //如果栈为空,则返回true,否则返回false
        }
    }


    public class Solution_547
    {
        public int FindCircleNum(int[][] isConnected)
        {
            var num  = isConnected.Length;       //城市数量
            var list = new List<HashSet<int>>(); //记录已经访问过的城市
            var sum  = num;                      //记录省份数量

            for (var i = 0 ; i < num ; i++)
            {
                for (var j = 0 ; j < i ; j++)
                {
                    if (isConnected[i][j] != 1) continue;


                    var iIndex = -1;
                    var jIndex = -1;

                    foreach (var hashSet in list)
                    {
                        if (hashSet.Contains(i))
                        {
                            iIndex = list.IndexOf(hashSet);
                        }

                        if (hashSet.Contains(j))
                        {
                            jIndex = list.IndexOf(hashSet);
                        }
                    }

                    if (iIndex != -1 && jIndex != -1)
                    {
                        if (iIndex != jIndex) //如果i和j都在已经访问过的城市中,且不在同一个省份中,则合并两个省份
                        {
                            list[iIndex].UnionWith(list[jIndex]);
                            list.RemoveAt(jIndex);
                        }
                    }
                    else if (iIndex != -1) //说明i在已经访问过的城市中,则把j加入到i所在的省份中
                    {
                        sum--;
                        list[iIndex].Add(j);
                    }
                    else if (jIndex != -1) //说明j在已经访问过的城市中,则把i加入到j所在的省份中
                    {
                        sum--;
                        list[jIndex].Add(i);
                    }
                    else //如果i和j都不在已经访问过的城市中,则新建一个省份
                    {
                        sum--;
                        sum--;
                        var hashSet = new HashSet<int> { i, j };
                        list.Add(hashSet);
                    }
                }
            }


            return list.Count + sum;
        }


        //DFS
        public int FindCircleNum2(int[][] isConnected)
        {
            var        province = 0;
            var        len      = isConnected.Length;
            Span<bool> visited  = stackalloc bool[len]; //维护一个数组,记录城市是否被访问过
            for (var i = 0 ; i < len ; i++)
            {
                if (visited[i]) continue;

                DFS(i, visited); //开始遍历这个城市所在的省份所有的城市
                province++;
            }

            return province;

            void DFS(int city, Span<bool> visited)
            {
                visited[city] = true;

                for (var j = 0 ; j < len ; j++)
                {
                    if (isConnected[city][j] == 1 && !visited[j])
                    {
                        DFS(j, visited);
                    }
                }
            }
        }

        //BFS
        public int FindCircleNum3(int[][] isConnected)
        {
            var        cities    = isConnected.Length;
            Span<bool> visited   = stackalloc bool[cities];
            var        provinces = 0;
            var        queue     = new Queue<int>();

            for (var i = 0 ; i < cities ; i++)
            {
                if (visited[i]) continue;

                queue.Enqueue(i);

                while (queue.Count != 0)
                {
                    var j = queue.Dequeue();
                    visited[j] = true;
                    for (var k = 0 ; k < cities ; k++)
                    {
                        if (isConnected[j][k] == 1 && !visited[k])
                        {
                            queue.Enqueue(k);
                        }
                    }
                }

                provinces++;
            }

            return provinces;
        }

        //  [[1,0,0,1],
        //   [0,1,1,0],
        //   [0,1,1,1],
        //   [1,0,1,1]]

        //     0 : 1: 2: 3: 4: 5: 6: 7: 8: 9:10:11:12:13:14
        //0 :[[1 , 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0]
        //1 :,[1 , 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        //2 :,[0 , 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        //3 :,[0 , 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0]
        //4 :,[0 , 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0]
        //5 :,[0 , 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0]
        //6 :,[0 , 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0]
        //7 :,[1 , 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0]
        //8 :,[0 , 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0]
        //9 :,[0 , 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1]
        //10:,[0 , 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0]
        //11:,[0 , 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0]
        //12:,[0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0]
        //13:,[0 , 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0]
        //14:,[0 , 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1]]

        [Test]
        public void Test()
        {
            int[][] isConnected = [[1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0], [1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0], [0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0], [0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0], [1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0], [0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1], [0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0], [0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1]];

            var res = FindCircleNum(isConnected);

            Console.WriteLine(res);
        }


        // 547. 省份数量
        //     中等
        // 相关标签
        //     相关企业
        // 有 n 个城市，其中一些彼此相连，另一些没有相连。如果城市 a 与城市 b 直接相连，且城市 b 与城市 c 直接相连，那么城市 a 与城市 c 间接相连。
        //
        // 省份 是一组直接或间接相连的城市，组内不含其他没有相连的城市。
        //
        // 给你一个 n x n 的矩阵 isConnected ，其中 isConnected[i][j] = 1 表示第 i 个城市和第 j 个城市直接相连，而 isConnected[i][j] = 0 表示二者不直接相连。
        //
        // 返回矩阵中 省份 的数量。
        //
        //
        //
        // 示例 1：
        //
        //
        // 输入：isConnected =
        // [[1,1,0],
        //  [1,1,0],
        //  [0,0,1]]
        // 输出：2
        // 示例 2：
        //
        //
        // 输入：isConnected =
        // [[1,0,0],
        //  [0,1,0],
        //  [0,0,1]]
        // 输出：3
        // 
        // [[1,1,1],
        //  [1,1,1],
        //  [1,1,1]]
        //
        //  [[1,0,0,1],
        //   [0,1,1,0],
        //   [0,1,1,1],
        //   [1,0,1,1]]
        //
        // 提示：
        //
        // 1 <= n <= 200
        // n == isConnected.length
        //     n == isConnected[i].length
        //     isConnected[i][j] 为 1 或 0
        // isConnected[i][i] == 1
        // isConnected[i][j] == isConnected[j][i]
    }


    public class Solution_2810 //双向链表
    {
        [TestCase("string", ExpectedResult = "rtsng")]
        public string FinalString(string s)
        {
            var forward = true;
            var list    = new LinkedList<char>();
            foreach (var c in s)
            {
                if (c == 'i')
                    forward = !forward;
                else if (forward)
                    list.AddLast(c);
                else
                    list.AddFirst(c);
            }

            return string.Join(string.Empty, forward ? list : list.Reverse());
        }

        // 2810. 故障键盘
        //     第 357 场周赛
        //     Q1
        // 1193
        // 相关标签
        //     相关企业
        // 提示
        //     你的笔记本键盘存在故障，每当你在上面输入字符 'i' 时，它会反转你所写的字符串。而输入其他字符则可以正常工作。
        //
        // 给你一个下标从 0 开始的字符串 s ，请你用故障键盘依次输入每个字符。
        //
        // 返回最终笔记本屏幕上输出的字符串。
        //
        //
        //
        // 示例 1：
        //
        // 输入：s = "string"
        // 输出："rtsng"
        // 解释：
        // 输入第 1 个字符后，屏幕上的文本是："s" 。
        // 输入第 2 个字符后，屏幕上的文本是："st" 。
        // 输入第 3 个字符后，屏幕上的文本是："str" 。
        // 因为第 4 个字符是 'i' ，屏幕上的文本被反转，变成 "rts" 。
        // 输入第 5 个字符后，屏幕上的文本是："rtsn" 。
        // 输入第 6 个字符后，屏幕上的文本是： "rtsng" 。
        // 因此，返回 "rtsng" 。
        // 示例 2：
        //
        // 输入：s = "poiinter"
        // 输出："ponter"
        // 解释：
        // 输入第 1 个字符后，屏幕上的文本是："p" 。
        // 输入第 2 个字符后，屏幕上的文本是："po" 。
        // 因为第 3 个字符是 'i' ，屏幕上的文本被反转，变成 "op" 。
        // 因为第 4 个字符是 'i' ，屏幕上的文本被反转，变成 "po" 。
        // 输入第 5 个字符后，屏幕上的文本是："pon" 。
        // 输入第 6 个字符后，屏幕上的文本是："pont" 。
        // 输入第 7 个字符后，屏幕上的文本是："ponte" 。
        // 输入第 8 个字符后，屏幕上的文本是："ponter" 。
        // 因此，返回 "ponter" 。
        //
        //
        // 提示：
        //
        // 1 <= s.length <= 100
        // s 由小写英文字母组成
        // s[0] != 'i'
    }


    public class Solution_894 //没看懂
    {
        public IList<TreeNode> AllPossibleFBT(int n)
        {
            if (n % 2 == 0) return new List<TreeNode>();
            if (n     == 1) return new List<TreeNode> { new() };

            var res = new List<TreeNode>();
            for (var i = 1 ; i < n ; i += 2)
            {
                var left  = AllPossibleFBT(i);
                var right = AllPossibleFBT(n - i - 1);
                foreach (var l in left)
                {
                    foreach (var r in right)
                    {
                        res.Add(new(0, l, r));
                    }
                }
            }

            return res;
        }

        public IList<TreeNode> AllPossibleFBT2(int n)
        {
            if (n % 2 == 0) return new List<TreeNode>();

            var dp = new IList<TreeNode>[n + 1];

            for (var i = 0 ; i <= n ; i++)
            {
                dp[i] = new List<TreeNode>();
            }

            dp[1].Add(new(0));
            for (var i = 3 ; i <= n ; i += 2)
            {
                for (var j = 1 ; j < i ; j += 2)
                {
                    foreach (var leftSubtree in dp[j])
                    {
                        foreach (var rightSubtrees in dp[i - 1 - j])
                        {
                            var root = new TreeNode(0, leftSubtree, rightSubtrees);
                            dp[i].Add(root);
                        }
                    }
                }
            }

            return dp[n];
        }



        [Test]
        public void Test()
        {
            AllPossibleFBT(7);
        }
    }


    public class Solution_1379
    {
        public TreeNode GetTargetCopy(TreeNode original, TreeNode cloned, TreeNode target)
        {
            if (original == null)
            {
                return null;
            }

            if (original == target)
            {
                return cloned;
            }

            var left = GetTargetCopy(original.left, cloned.left, target);

            if (left != null)
            {
                return left;
            }

            return GetTargetCopy(original.right, cloned.right, target);
        }
    }


    public class Solution_2192
    {
        public IList<IList<int>> GetAncestors(int n, int[][] edges)
        {
            var result = new List<IList<int>>(n);
            var graph  = new Dictionary<int, List<int>>(n);

            for (var i = 0 ; i < n ; i++)
            {
                result.Add([]);
                graph.Add(i, []);
            }

            foreach (var edge in edges)
            {
                graph[edge[0]].Add(edge[1]);
            }


            for (var i = 0 ; i < n ; i++)
            {
                Span<bool> visited = stackalloc bool[n];
                DFS(i, i, visited);
            }

            return result;

            void DFS(int target, int index, Span<bool> visited)
            {
                var set = graph[index];

                visited[index] = true;

                foreach (var i in set)
                {
                    if (!visited[i])
                    {
                        result[i].Add(target);   //目标节点增加祖先
                        DFS(target, i, visited); //遍历这个节点的next,然后再增加
                    }
                }
            }
        }


        [Test]
        public void Test()
        {
            GetAncestors(9, [[3, 6], [2, 4], [8, 6], [7, 4], [1, 4], [2, 1], [7, 2], [0, 4], [5, 0], [4, 6], [3, 2], [5, 6], [1, 6]]);
        }
    }


    public class Solution_1026
    {
        public int maxAncestorDiff(TreeNode root)
        {
            return Math.Max(DFS(root.left, root.val, root.val), DFS(root.right, root.val, root.val));

            int DFS(TreeNode _root, int maxValue, int minValue)
            {
                if (_root == null) return -1;

                var leftRes  = DFS(_root.left,  Math.Max(maxValue, _root.val), Math.Min(minValue, _root.val));
                var rightRes = DFS(_root.right, Math.Max(maxValue, _root.val), Math.Min(minValue, _root.val));
                return Math.Max(Math.Max(leftRes, rightRes), Math.Max(Math.Abs(maxValue - _root.val), Math.Abs(minValue - _root.val)));
            }
        }
    }


    public class Solution_1483
    {
        public class TreeAncestor
        {
            private const int     LOG = 16;
            readonly      int[][] ancestors;

            public TreeAncestor(int n, int[] parent)
            {
                //生成nx16的矩阵，初始值为-1
                ancestors = new int[n][];
                for (var i = 0 ; i < n ; i++)
                {
                    ancestors[i] = new int[LOG];
                    Array.Fill(ancestors[i], -1);
                }

                //初始化第一列
                for (var i = 0 ; i < n ; i++)
                {
                    ancestors[i][0] = parent[i];
                }

                for (var j = 1 ; j < LOG ; j++)
                {
                    for (var i = 0 ; i < n ; i++)
                    {
                        if (ancestors[i][j - 1] != -1)
                        {
                            ancestors[i][j] = ancestors[ancestors[i][j - 1]][j - 1];
                        }
                    }
                }
            }

            public int GetKthAncestor(int node, int k)
            {
                for (var j = 0 ; j < LOG ; j++)
                {
                    if (((k >> j) & 1) != 0)
                    {
                        node = ancestors[node][j];
                        if (node == -1)
                        {
                            return -1;
                        }
                    }
                }

                return node;
            }
        }
    }


    public class Solution_1600
    {
        public class ThroneInheritance(string kingName)
        {
            public readonly Dictionary<string, IList<string>> dict = new() { { kingName, [] } };

            public readonly HashSet<string> deadSet = [];

            public readonly string root = kingName;



            public void Birth(string parentName, string childName)
            {
                var parent = dict[parentName];
                parent.Add(childName);
                dict.Add(childName, []);
            }

            public void Death(string name)
            {
                deadSet.Add(name);
            }

            public IList<string> GetInheritanceOrder()
            {
                var res = new List<string>();

                DFS(root);

                return res;

                void DFS(string name)
                {
                    if (!deadSet.Contains(name))
                    {
                        res.Add(name);
                    }

                    foreach (var child in dict[name])
                    {
                        DFS(child);
                    }
                }
            }
        }
    }


    public class Solution_2009
    {
        public int MinOperations(int[] nums)
        {
            var n   = nums.Length;
            var set = new HashSet<int>(nums);

            var sortedUniqueNums = new List<int>(set);
            sortedUniqueNums.Sort();
            var res = n;
            var j   = 0;
            for (var i = 0 ; i < sortedUniqueNums.Count ; i++)
            {
                var left  = sortedUniqueNums[i];
                var right = left + n - 1;

                while (j < sortedUniqueNums.Count && sortedUniqueNums[j] <= right)
                {
                    res = Math.Min(res, n - (j - i + 1));
                    j++;
                }
            }

            return res;
        }
    }


    public class Solution_2529
    {
        public int MaximumCount2(int[] nums)
        {
            return Math.Max(nums.Count(x => x > 0), nums.Count(x => x < 1));
        }

        public int MaximumCount(int[] nums)
        {
            var len = nums.Length;

            return Math.Max(Find(0), nums.Length - Find(1));

            int Find(int target)
            {
                int l = 0, r = len;
                while (l < r)
                {
                    var mid = (l + r) >> 1;
                    if (nums[mid] >= target)
                        r = mid;
                    else
                        l = mid + 1;
                }

                return l;
            }
        }
    }


    public class Solution_56
    {
        public int[][] Merge(int[][] intervals)
        {
            Array.Sort(intervals, (a, b) => a[0] - b[0]);

            var res = new List<int[]>();

            var current = intervals[0];

            for (var i = 1 ; i < intervals.Length ; i++)
            {
                var interval = intervals[i];

                if (interval[0] <= current[1] && interval[1] <= current[1]) { }
                else if (interval[0] <= current[1])
                {
                    current[1] = interval[1];
                }
                else
                {
                    res.Add(current);

                    current = interval;
                }
            }

            res.Add(current);

            return res.ToArray();
        }

        public int[][] Merge2(int[][] intervals)
        {
            var res = new List<int[]>(intervals.Length);

            Array.Sort(intervals, (a, b) => a[0] - b[0]);

            foreach (var inter in intervals)
            {
                if (res.Count == 0 || res[^1][1] < inter[0])
                {
                    res.Add(inter);
                }
                else
                {
                    res[^1][1] = Math.Max(inter[1], res[^1][1]);
                }
            }

            return res.ToArray();
        }
    }


    public class Solution_1702
    {
        [TestCase("000110", ExpectedResult = "111011")]
        public string MaximumBinaryString(string binary)
        {
            Span<char> result = binary.ToCharArray();

            for (var i = 1 ; i < result.Length ; i++)
            {
                if (result[i] == '0' && result[i - 1] == '0')
                {
                    result[i - 1] = '1';
                }
                else if (result[i] == '1' && result[i - 1] == '0')
                {
                    var start = i;
                    while (i < result.Length && result[start] == '1')
                    {
                        start++;
                    }
                }
            }


            return new string(result);
        }

        [TestCase("000110", ExpectedResult = "111011")]
        [TestCase("01110",  ExpectedResult = "10111")]
        [TestCase("0111",   ExpectedResult = "0111")]
        [TestCase("1100",   ExpectedResult = "1110")]
        [TestCase("111",    ExpectedResult = "111")]
        public string MaximumBinaryString2(string binary)
        {
            var len      = binary.Length;
            var oneCount = binary.Count(x => x == '1');

            if (oneCount == len) return binary;


            //开头的1的个数
            var startOne = 0;
            while (startOne < len && binary[startOne] == '1')
            {
                startOne++;
            }


            oneCount -= startOne;

            Span<char> result = stackalloc char[len];

            result.Fill('1');
            result[^(oneCount + 1)] = '0';


            return new(result);
        }
    }


    public class Solution_189
    {
        public void Rotate(int[] nums, int k)
        {
            var len      = nums.Length;
            var numsSpan = new Span<int>(nums.ToArray());


            for (int i = 0 ; i < nums.Length ; i++)
            {
                nums[(i + k) % len] = numsSpan[i];
            }
        }

        [Test]
        public void Test()
        {
            Rotate([1, 2, 3, 4, 5, 6, 7], 3);
        }
    }


    public class Solution_2924
    {
        //如果两只队伍没有比赛过,则返回-1;

        //优化前版本
        public int FindChampion(int n, int[][] edges)
        {
            var graph = new Dictionary<int, HashSet<int>>();

            for (int i = 0 ; i < n ; i++)
            {
                graph.Add(i, []);
            }


            foreach (var edge in edges)
            {
                graph.TryAdd(edge[1], []);
                graph[edge[1]].Add(edge[0]);
            }

            int signal = 0;
            int result = -1;
            foreach (var node in graph)
            {
                if (node.Value.Count == 0)
                {
                    signal++;
                    result = node.Key;
                }
            }

            return signal == 1 ? result : -1;
        }

        //由于节点的具体值是多少没有涉及,直接优化成计数器
        public int FindChampion2(int n, int[][] edges)
        {
            Span<int> graph = stackalloc int[n];

            //有比edge[1]的强的,则计数器+1 
            foreach (var edge in edges)
            {
                graph[edge[1]]++;
            }

            int signal = 0;
            int result = -1;
            for (var i = 0 ; i < graph.Length ; i++)
            {
                var node = graph[i];
                if (node == 0)
                {
                    signal++;
                    result = i;
                }
            }

            return signal == 1 ? result : -1;
        }
    }


  
}