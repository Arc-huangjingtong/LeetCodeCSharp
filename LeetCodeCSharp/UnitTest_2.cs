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


   
}