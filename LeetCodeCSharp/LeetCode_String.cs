namespace LeetCodeCSharp
{

    public class Solution_2288
    {
        //TODO:待优化
        //[TestCase("there are $1 $2 and 5$ candies in the shop", 50,  ExpectedResult = "there are $0.50 $1.00 and 5$ candies in the shop")]
        //[TestCase("1 2 $3 4 $5 $6 7 8$ $9 $10$",                100, ExpectedResult = "1 2 $0.00 4 $0.00 $0.00 7 8$ $0.00 $10$")]
        //[TestCase("$2$3 $10 $100 $1 200 $33 33$ $$ $99 $99999 $9999999999", 0,  ExpectedResult = "$2$3 $10.00 $100.00 $1.00 200 $33.00 33$ $$ $99.00 $99999.00 $9999999999.00")]
        [TestCase("1$2", 50, ExpectedResult = "1$2")]
        public string DiscountPrices(string sentence, int discount)
        {
            var sb = new StringBuilder();

            for (var i = 0 ; i < sentence.Length ; i++)
            {
                if (sentence[i] == '$')
                {
                    var j        = i + 1;
                    var isNumber = true;
                    while (j < sentence.Length && sentence[j] != ' ')
                    {
                        if (!char.IsDigit(sentence[j]))
                        {
                            isNumber = false;
                        }

                        j++;
                    }

                    if (0 == j - i - 1)
                    {
                        sb.Append('$');
                        continue;
                    }

                    var substr = sentence.Substring(i + 1, j - i - 1);

                    if (!isNumber || (i >= 1 && sentence[i - 1] != ' '))
                    {
                        sb.Append('$');
                        sb.Append(substr);
                        i = j - 1;
                        continue;
                    }


                    var price           = long.Parse(substr);
                    var discountedPrice = price * (100 - discount) / 100.0;
                    sb.Append($"${discountedPrice:F2}");
                    i = j - 1;
                }
                else
                {
                    sb.Append(sentence[i]);
                }
            }

            return sb.ToString();
        }

        [TestCase("there are $1 $2 and 5$ candies in the shop",             50,  ExpectedResult = "there are $0.50 $1.00 and 5$ candies in the shop")]
        [TestCase("1 2 $3 4 $5 $6 7 8$ $9 $10$",                            100, ExpectedResult = "1 2 $0.00 4 $0.00 $0.00 7 8$ $0.00 $10$")]
        [TestCase("$2$3 $10 $100 $1 200 $33 33$ $$ $99 $99999 $9999999999", 0,   ExpectedResult = "$2$3 $10.00 $100.00 $1.00 200 $33.00 33$ $$ $99.00 $99999.00 $9999999999.00")]
        [TestCase("1$2",                                                    50,  ExpectedResult = "1$2")]
        public string DiscountPrices2(string sentence, int discount)
        {
            var sb = new StringBuilder();

            for (var i = 0 ; i < sentence.Length ; i++)
            {
                if (sentence[i] != ' ')
                {
                    var j        = i + 1;
                    var isNumber = true;
                    while (j < sentence.Length && sentence[j] != ' ')
                    {
                        if (!char.IsDigit(sentence[j]))
                        {
                            isNumber = false;
                        }

                        j++;
                    }

                    // 非金钱的情况
                    if (sentence[i] != '$' || !isNumber || (j - i - 1 == 0))
                    {
                        var sub = sentence.Substring(i, j - i);
                        sb.Append(sub);
                        i = j - 1;
                        continue;
                    }

                    var substr = sentence.Substring(i + 1, j - i - 1);

                    var price           = long.Parse(substr);
                    var discountedPrice = price * (100 - discount) / 100.0;
                    sb.Append($"${discountedPrice:F2}");
                    i = j - 1;
                }
                else
                {
                    sb.Append(sentence[i]);
                }
            }

            return sb.ToString();
        }
    }



    public class Solution_2663
    {
        public readonly char[] alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];

        public string SmallestBeautifulString(string s, int k)
        {
            for (var i = s.Length - 1 ; i >= 0 ; i--)
            {
                var blockedChars = new HashSet<char>();
                for (var j = 1 ; j < 3 ; j++)
                {
                    if (i - j >= 0)
                    {
                        blockedChars.Add(s[i - j]);
                    }
                }

                for (var j = 1 ; j < 4 ; j++)
                {
                    if (s[i] - 'a' + j + 1 <= k && !blockedChars.Contains((char)(s[i] + j)))
                    {
                        return Generate(s, i, j);
                    }
                }
            }

            return "";
        }

        public static string Generate(string s, int idx, int offset)
        {
            var res = s.ToCharArray();
            res[idx] = (char)(res[idx] + offset);
            for (var i = idx + 1 ; i < s.Length ; i++)
            {
                var blockedChars = new HashSet<char>();
                for (var j = 1 ; j < 3 ; j++)
                {
                    if (i - j >= 0)
                    {
                        blockedChars.Add(res[i - j]);
                    }
                }

                for (var j = 0 ; j < 3 ; j++)
                {
                    if (!blockedChars.Contains((char)('a' + j)))
                    {
                        res[i] = (char)('a' + j);
                        break;
                    }
                }
            }

            return new string(res);
        }



        // 提示:
        // 1.不含[AA][ABA] |:如果字符串不包含任何长度为 2 和 3 的回文子字符串，则该字符串根本不包含任何回文子字符串。
        // 2.[BAA+1 != BAB or BAB +1 != BAA ]从右到左迭代，如果可以在索引 i 处增加字符，而不创建任何长度为 2 和 3 的回文子字符串，则增加它。
        // 3.在索引 i 处增加字符后，将索引 i 之后的每个字符设置为字符 a。有了这个，我们将确保我们创建了一个比 s 更大的字典字符串，该字符串在索引 i 之前不包含任何回文，并且在字典上是最小的
        // 4.[案例: axyz + 1 a]最后，我们只剩下一个案例来修复回文子字符串，它位于索引 i 之后。这可以通过第二个提示中提到的类似方法来完成



        // 2416-字典序最小的美丽字符串-算术评级[9] 第 343 场周赛 Q4
        // 
        // [美丽字符串]:
        // 1.它由英语小写字母表的[前 k 个字母]组成
        // 2.它不包含任何长度 [大于等于2的回文子字符串]
        //
        // [字典序]:
        // 1.[长度相同]的两个字符串 a 和 b
        // 2.如果字符串 a 在与字符串 b 不同的第一个位置上的字符字典序更大,则字符串 a 的字典序大于字符串 b 。
        // "abcd" > "abcc"
        // 
        // 1.美丽字符串 s (s.length == n, 1 <= n  <= 10^5)
        // 2.正整数 k (4 <= k <= 26)
        // 请你找出并返回一个长度为 n 的美丽字符串,该字符串还满足：在字典序大于 s 的所有美丽字符串中字典序最小。如果不存在这样的字符串,则返回一个空字符串。
        //
        //
        // 示例 1：
        //
        // 输入：s = "abcz", k = 26
        // 输出："abda"
        // 解释：字符串 "abda" 既是美丽字符串,又满足字典序大于 "abcz" 。
        // 可以证明不存在字符串同时满足字典序大于 "abcz"、美丽字符串、字典序小于 "abda" 这三个条件。
        //
        // 示例 2：
        //
        // 输入：s = "dc", k = 4 
        // 输出：""
        // 解释：可以证明,不存在既是美丽字符串,又字典序大于 "dc" 的字符串。
    }


    // 100345. 使所有元素都可以被 3 整除的最少操作数
    // 简单
    // 给你一个整数数组 nums 。一次操作中，你可以将 nums 中的 任意 一个元素增加或者减少 1 。
    //
    // 请你返回将 nums 中所有元素都可以被 3 整除的 最少 操作次数。


    public class Solution_100345
    {
        public int MinimumOperations(int[] nums)
        {
            var res = 0;

            foreach (var num in nums)
            {
                if (num % 3 == 0)
                {
                    continue;
                }

                res++;
            }

            return res;
        }
    }


    // 给你一个二进制数组 nums 。
    //
    // 你可以对数组执行以下操作 任意 次（也可以 0 次）：
    //
    // 选择数组中 任意连续 3 个元素，并将它们 全部反转 。
    // 反转    一个元素指的是将它的值从 0 变 1 ，或者从 1 变 0 。
    //
    // 请你返回将 nums 中所有元素变为 1 的 最少 操作次数。如果无法全部变成 1 ，返回 -1 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [0,1,1,1,0,0]
    //
    // 输出：3
    //
    // 解释：
    // 我们可以执行以下操作：
    //
    // 选择下标为 0 ，1 和 2 的元素并反转，得到 nums = [1,0,0,1,0,0] 。
    // 选择下标为 1 ，2 和 3 的元素并反转，得到 nums = [1,1,1,0,0,0] 。
    // 选择下标为 3 ，4 和 5 的元素并反转，得到 nums = [1,1,1,1,1,1] 。
    // 示例 2：
    //
    // 输入：nums = [0,1,1,1]
    //
    // 输出：-1
    //
    // 解释：
    // 无法将所有元素都变为 1 。
    //
    //
    //
    // 提示：
    //
    // 3 <= nums.length <= 10^5
    // 0 <= nums[i]     <= 1
    public class Solution_111
    {
        [TestCase(new int[] { 0, 1, 1, 1, 0, 0 }, ExpectedResult = 3)]
        [TestCase(new int[] { 0, 1, 1, 1 },       ExpectedResult = -1)]
        public int MinOperations(int[] nums)
        {
            var result = 0;
            var length = nums.Length;
            for (var i = 0 ; i < length ; i++)
            {
                if (nums[i] == 1) continue;

                if (i + 2 >= length) return -1;


                result++;
                nums[i]     = 1;
                nums[i + 1] = nums[i + 1] == 1 ? 0 : 1;
                nums[i + 2] = nums[i + 2] == 1 ? 0 : 1;
            }

            return result;
        }
    }


    // 100346. 使二进制数组全部等于 1 的最少操作次数 II
    // 中等
    // 给你一个二进制数组 nums 。
    //
    // 你可以对数组执行以下操作 任意 次（也可以 0 次）：
    //
    // 选择数组中 任意 一个下标 i ，并将从下标 i 开始一直到数组末尾 所有 元素 反转 。
    // 反转 一个元素指的是将它的值从 0 变 1 ，或者从 1 变 0 。
    //
    // 请你返回将 nums 中所有元素变为 1 的 最少 操作次数
    // 示例 1：
    //
    // 输入：nums = [0,1,1,0,1]
    //
    // 输出：4
    //
    // 解释：
    // 我们可以执行以下操作：
    //
    // 选择下标 i = 1 执行操作，得到 nums = [0,0,0,1,0]
    // 选择下标 i = 0 执行操作，得到 nums = [1,1,1,0,1]
    // 选择下标 i = 4 执行操作，得到 nums = [1,1,1,0,0]
    // 选择下标 i = 3 执行操作，得到 nums = [1,1,1,1,1]
    // 示例 2：
    //
    // 输入：nums = [1,0,0,0]
    //
    // 输出：1
    //
    // 解释：
    // 我们可以执行以下操作：
    //
    // 选择下标 i = 1 执行操作，得到 nums = [1,1,1,1] 。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 105
    // 0 <= nums[i]     <= 1
    public class Solution_100346
    {
        [TestCase(new int[] { 0, 1, 1, 0, 1 }, ExpectedResult = 4)]
        [TestCase(new int[] { 1, 0, 0, 0 },    ExpectedResult = 1)]
        public int MinOperations(int[] nums)
        {
            var operations = 0;
            for (var i = 1 ; i < nums.Length ; i++)
            {
                if (nums[i] != nums[i - 1])
                {
                    operations++;
                }
            }

            if (nums[0] == 0) operations++;


            return operations;
        }
    }


    // 100333. 统计逆序对的数目
    // 困难
    // 给你一个整数 n 和一个二维数组 requirements ，其中 requirements[i] = [endi, cnti] 表示这个要求中的末尾下标和 逆序对 的数目。
    //
    // 整数数组 nums 中一个下标对 (i, j) 如果满足以下条件，那么它们被称为一个 逆序对 ：
    //
    // i < j 且 nums[i] > nums[j]
    // 请你返回 [0, 1, 2, ..., n - 1] 的 
    //     排列
    // perm 的数目，满足对 所有 的 requirements[i] 都有 perm[0..endi] 恰好有 cnti 个逆序对。
    //
    // 由于答案可能会很大，将它对 109 + 7 取余 后返回。
    //
    //
    //
    // 示例 1：
    //
    // 输入：n = 3, requirements = [[2,2],[0,0]]
    //
    // 输出：2
    //
    // 解释：
    //
    // 两个排列为：
    //
    // [2, 0, 1]
    // 前缀 [2, 0, 1] 的逆序对为 (0, 1) 和 (0, 2) 。
    // 前缀 [2] 的逆序对数目为 0 个。
    // [1, 2, 0]
    // 前缀 [1, 2, 0] 的逆序对为 (0, 2) 和 (1, 2) 。
    // 前缀 [1] 的逆序对数目为 0 个。
    // 示例 2：
    //
    // 输入：n = 3, requirements = [[2,2],[1,1],[0,0]]
    //
    // 输出：1
    //
    // 解释：
    //
    // 唯一满足要求的排列是 [2, 0, 1] ：
    //
    // 前缀 [2, 0, 1] 的逆序对为 (0, 1) 和 (0, 2) 。
    // 前缀 [2, 0] 的逆序对为 (0,    1) 。
    // 前缀 [2] 的逆序对数目为 0 。
    // 示例 3：
    //
    // 输入：n = 2, requirements = [[0,0],[1,0]]
    //
    // 输出：1
    //
    // 解释：
    //
    // 唯一满足要求的排列为 [0, 1] ：
    //
    // 前缀 [0] 的逆序对数目为 0 。
    // 前缀 [0, 1] 的逆序对为 (0, 1) 。
    //
    //
    //
    // 提示：
    //
    // 2 <= n                   <= 300
    // 1 <= requirements.length <= n
    // requirements[i] = [endi, cnti]
    // 0                   <= endi <= n - 1
    // 0                   <= cnti <= 400
    // 输入保证至少有一个 i 满足 endi == n - 1 。
    // 输入保证所有的   endi 互不相同。


    public class Solution_100333
    {
        public int NumberOfPermutations(int n, int[][] requirements)
        {
            var result = 0;
            var output = Enumerable.Range(0, n).ToList();
            backtrack(0);

            return result;

            void backtrack(int first)
            {
                if (first == n) // 所有数都填完了
                {
                    var flag = true;
                    foreach (var requirement in requirements)
                    {
                        var endi = requirement[0] + 1;
                        var cnti = requirement[1];

                        var temp = output[..endi].ToArray();

                        var k = MergeSortAndCount(temp, 0, temp.Length - 1);

                        if (k != cnti)
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        result++;
                    }
                }

                for (var i = first ; i < n ; i++)
                {
                    // 动态维护数组
                    (output[first], output[i]) = (output[i], output[first]);
                    // 继续递归填下一个数
                    backtrack(first + 1);
                    // 撤销操作
                    (output[first], output[i]) = (output[i], output[first]);
                }
            }
        }


        private static int MergeSortAndCount(int[] arr, int l, int r)
        {
            if (l >= r) return 0;

            var count = 0;
            var m     = (l + r) / 2;
            count += MergeSortAndCount(arr, l,     m);
            count += MergeSortAndCount(arr, m + 1, r);
            count += Merge(arr, l, m, r);
            return count;
        }

        private static int Merge(int[] arr, int l, int m, int r)
        {
            // 创建临时数组保存合并后的结果

            var leftArray  = arr[l..(m + 1)];
            var rightArray = arr[(m    + 1)..r];


            int i = 0, j = 0, k = l, swaps = 0;

            while (i < leftArray.Length && j < rightArray.Length)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    // 当左侧元素小于等于右侧元素，将左侧元素复制到结果数组中
                    arr[k++] = leftArray[i++];
                }
                else
                {
                    // 当左侧元素大于右侧元素，逆序对的数量增加
                    // 增加的数量是左侧数组中剩余的元素数量
                    arr[k++] =  rightArray[j++];
                    swaps    += (m + 1) - (l + i);
                }
            }

            // 复制剩余元素
            while (i < leftArray.Length) arr[k++]  = leftArray[i++];
            while (j < rightArray.Length) arr[k++] = rightArray[j++];

            return swaps;
        }



        public int NumberOfPermutations2(int n, int[][] requirements)
        {
            var result = int.MaxValue;

            foreach (var requirement in requirements)
            {
                var endi = requirement[0] + 1;
                var cnti = requirement[1];

                var _r = KInversePairs(endi, cnti);

                result = Math.Min(result, _r);
            }

            return result;
        }


        public int KInversePairs(int n, int k)
        {
            const int MOD = 1000000007;

            var f = new int[2, k + 1];
            f[0, 0] = 1;

            for (var i = 1 ; i <= n ; ++i)
            {
                for (var j = 0 ; j <= k ; ++j)
                {
                    int cur = i & 1, prev = cur ^ 1;

                    f[cur, j] = (j - 1 >= 0 ? f[cur, j - 1] : 0) - (j - i >= 0 ? f[prev, j - i] : 0) + f[prev, j];

                    if (f[cur, j] >= MOD)
                    {
                        f[cur, j] -= MOD;
                    }
                    else if (f[cur, j] < 0)
                    {
                        f[cur, j] += MOD;
                    }
                }
            }

            return f[n & 1, k];
        }


        [TestCase(ExpectedResult = 1)]
        public int Test()
        {
            return NumberOfPermutations(3, [[2, 2], [1, 1], [0, 0]]);
        }
    }

}