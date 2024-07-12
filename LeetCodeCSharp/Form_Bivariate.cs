namespace LeetCodeCSharp;

// 双变量问题 : 两个变量之间有一个函数关系,可以通过只维护一个因变量来解决
// 例如 [1.两数之和] ai+aj = t , 可以枚举右边的 aj，转换成 单变量问题，也就是查找在 ai 左边是否有 ai = t−aj，这可以用哈希表维护。
// 这个技巧叫做 "枚举右，维护左"

// 双变量问题的特点:
// 1. 两个变量(或者通过换元,把多个最终转换成两个)之间有一个函数关系 [多变量换元: 454. 四数相加 II]
//    1.1 多个变量时,变量之间的关系必须是单调关系,或者是有一个通用的贪心原则在 ,例如 [2874. 有序三元组中的最大值 II] , 两个变量之间的关系是单调的
// 2. 通过枚举其中一个变量,转换成单变量问题
// 3. 通过哈希表维护另一个变量,来解决问题
// 4. 如果变量元素的索引,有大小关系,则可以一次遍历 [1814. 统计一个数组中好对子的数目]
// 5. 如果变量元素的索引,没有大小关系,并且可以重复,则需要先遍历一遍,构造哈希表
// 6. 目的是计数,具体环境下,可能会有具体处理,但是单纯的计数,需要考虑取余操作
//    (a mod c + b mod c) mod c = (a + b) mod c :即 子项的模运算和,的模运算 等于 总和的模运算


/// <summary> 1814. 统计一个数组中好对子的数目 </summary>
public class Solution_1814
{
    // 这就是变量之间的关系:
    // 根据公式  : nums[i] + rev(nums[j]) == nums[j] + rev(nums[i])
    // ======> : nums[i] - rev(nums[i]) == nums[j] - rev(nums[j])
    // 找到关系后,还要注意一点 : 
    public int CountNicePairs(int[] nums)
    {
        const int mod = 1000000007;
        var       dic = new Dictionary<int, int>();
        var       res = 0;

        foreach (var num in nums)
        {
            var sign = num - Reverse(num);

            if (dic.TryAdd(sign, 1)) continue;

            res = (res + dic[sign]) % mod; // (a mod c + b mod c) mod c = (a + b) mod c

            dic[sign]++;
        }

        res %= mod;


        return res;

        int Reverse(int num)
        {
            var i = 0;
            while (num > 0)
            {
                i   =  i * 10 + num % 10;
                num /= 10;
            }

            return i;
        }
    }

    // 给你一个数组 nums ,定义 rev(x) 的值为将整数 x 各个数字位反转得到的结果。比方说 rev(123) = 321 ， rev(120) = 21
    // 我们称满足下面条件的下标对 (i, j) 是 好的 ：
    // 0 <= i < j < nums.length
    // nums[i] + rev(nums[j]) == nums[j] + rev(nums[i])
    // 请你返回好下标对的数目。由于结果可能会很大，请将结果对 10^9 + 7 取余 后返回
    // 1 <= nums.length <= 10^5
    // 0 <= nums[i]     <= 10^9
}


/// <summary> 1014. 最佳观光组合 </summary>
public class Solution_1014
{
    // 1. 题干可知   : (i < j)求最大的 values[i] + values[j] + i - j
    // 可能会有疑问: 即函数关系在哪,如何转换成单变量问题?
    // 2. 可以的推导 :
    // 2.1 : part1:(values[i] + i) + part2:(values[j] - j)  
    // 因为只需要维护part1的最大值,即可,所以part1在算法中是固定的 , 在part1固定,且j固定的时候: 可得关系: Max = part1Max + values[j] - j
    // 2.2 : 维护最大的part1:(values[i] + i) 枚举 values[j] - j 即可

    // 难点主要是找到隐藏的关系, 本质上是动态规划的思想,很难找到规律
    // MAX[0-N] = MAX[0-N-1] + values[N] - N
    [TestCase(new[] { 8, 1, 5, 2, 6 }, ExpectedResult = 11)]
    public int MaxScoreSightseeingPair(int[] values)
    {
        var max = values[0];
        var res = 0;

        for (var i = 1 ; i < values.Length ; i++)
        {
            res = Math.Max(res, max + values[i] - i);
            max = Math.Max(max, values[i]       + i);
        }

        return res;
    }

    // 给你一个正整数数组 values，其中 values[i] 表示第 i 个观光景点的评分，并且两个景点 i 和 j 之间的 距离 为 j - i。
    //
    // 一对景点（i < j）组成的观光组合的得分为 values[i] + values[j] + i - j ，也就是景点的评分之和 减去 它们两者之间的距离。
    //
    // 返回一对观光景点能取得的最高分。
    //
    //
    //
    // 示例 1：
    //
    // 输入：values = [8,1,5,2,6]
    // 输出：11
    // 解释：i = 0, j = 2, values[i] + values[j] + i - j = 8 + 5 + 0 - 2 = 11
    // 示例 2：
    //
    // 输入：values = [1,2]
    // 输出：2
    //
    //
    // 提示：
    //
    // 2 <= values.length <= 5 * 10^4
    // 1 <= values[i] <= 1000
}


/// <summary> 2342. 数位和相等数对的最大和 </summary>
public class Solution_2342
{
    //这一题的隐藏的关系是: res =max( (维护的max)dic[bitSum] + bitSum:num )
    // 即维护变量:字典中指定数位和的最大值,然后遍历剩余的值,计算成绩,维护成绩的最大值即可
    // 模板题,感觉中等题的难度,基本上模板的东西还是得绕不少弯子的
    [TestCase(new[] { 18, 43, 36, 13, 7 }, ExpectedResult = 54)]
    [TestCase(new[] { 10, 12, 19, 14 },    ExpectedResult = -1)]
    public int MaximumSum(int[] nums)
    {
        var res = -1;
        var dic = new Dictionary<int, int>();

        foreach (var num in nums)
        {
            var bitSum = BitSum(num);

            if (!dic.TryAdd(bitSum, num))
            {
                res         = Math.Max(res,         dic[bitSum] + num);
                dic[bitSum] = Math.Max(dic[bitSum], num);
            }
        }


        return res;

        int BitSum(int num)
        {
            var sum = 0;
            while (num > 0)
            {
                sum += num % 10;
                num /= 10;
            }

            return sum;
        }
    }


    // 给你一个下标从 0 开始的数组 nums ，数组中的元素都是 正 整数。请你选出两个下标 i 和 j（i != j），且 nums[i] 的数位和 与  nums[j] 的数位和相等。
    //
    // 请你找出所有满足条件的下标 i 和 j ，找出并返回 nums[i] + nums[j] 可以得到的 最大值 。
    //
    // 示例 1：
    //
    // 输入：nums = [18,43,36,13,7]
    // 输出：54
    // 解释：满足条件的数对 (i, j) 为：
    // - (0, 2) ，两个数字的数位和都是 9 ，相加得到 18 + 36 = 54 
    // - (1, 4) ，两个数字的数位和都是 7 ，相加得到 43 + 7  = 50 
    // 所以可以获得的最大和是 54 。
    // 示例 2：
    //
    // 输入：nums = [10,12,19,14]
    // 输出：-1
    // 解释：不存在满足条件的数对，返回 -1 。
    //
    //
    // 1 <= nums.length <= 10^5
    // 1 <= nums[i]     <= 10^9
}


/// <summary> 1679. K 和数对的最大数目 </summary>
public class Solution_1679
{
    [TestCase(new[] { 1, 2, 3, 4 },    5, ExpectedResult = 2)]
    [TestCase(new[] { 3, 1, 3, 4, 3 }, 6, ExpectedResult = 1)]
    //
    [TestCase(new[] { 2, 5, 4, 4, 1, 3, 4, 4, 1, 4, 4, 1, 2, 1, 2, 2, 3, 2, 4, 2 }, 3, ExpectedResult = 4)]
    public int MaxOperations(int[] nums, int k)
    {
        var dic = new Dictionary<int, int>();
        var res = 0;

        foreach (var num in nums)
        {
            var target = k - num;

            if (dic.TryGetValue(target, out var count) && count > 0)
            {
                res++;
                dic[target]--;
                if (dic[target] == 0)
                {
                    dic.Remove(target);
                }
            }
            else
            {
                if (!dic.TryAdd(num, 1))
                {
                    dic[num]++;
                }
            }
        }

        return res;
    }

    [TestCase(new[] { 1, 2, 3, 4 },                                                 5, ExpectedResult = 2)]
    [TestCase(new[] { 3, 1, 3, 4, 3 },                                              6, ExpectedResult = 1)]
    [TestCase(new[] { 2, 5, 4, 4, 1, 3, 4, 4, 1, 4, 4, 1, 2, 1, 2, 2, 3, 2, 4, 2 }, 3, ExpectedResult = 4)]
    // 优化后的双指针版本, 但是这个版本的时间复杂度是O(nlogn),空间复杂度是O(1)
    public int MaxOperations2(int[] nums, int k)
    {
        Array.Sort(nums);
        var l   = 0;
        var r   = nums.Length - 1;
        var sum = 0;
        while (l < r)
        {
            if (nums[l] + nums[r] > k)
            {
                r--;
            }
            else if (nums[l] + nums[r] < k)
            {
                l++;
            }
            else
            {
                l++;
                r--;

                sum++;
            }
        }

        return sum;
    }



    // 1679. K 和数对的最大数目
    // 中等
    //     相关标签
    // 相关企业
    //     提示
    // 给你一个整数数组 nums 和一个整数 k 。
    //
    // 每一步操作中，你需要从数组中选出和为 k 的两个整数，并将它们移出数组。
    //
    // 返回你可以对数组执行的最大操作数。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [1,2,3,4], k = 5
    // 输出：2
    // 解释：开始时 nums = [1,2,3,4]：
    //     - 移出 1 和 4 ，之后 nums = [2,3]
    //                         - 移出 2 和 3 ，之后 nums = []
    // 不再有和为 5 的数对，因此最多执行 2 次操作。
    // 示例 2：
    //
    // 输入：nums = [3,1,3,4,3], k = 6
    // 输出：1
    // 解释：开始时 nums = [3,1,3,4,3]：
    //     - 移出前两个 3 ，之后nums = [1,4,3]
    // 不再有和为 6 的数对，因此最多执行 1 次操作。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 10^5
    // 1 <= nums[i] <= 10^9
    // 1 <= k <= 10^9
}


/// <summary> 3185. 构成整天的下标对数目 II </summary>
public class Solution_3185
{
    // 没什么好说的,主要是在操作元素上可以利用取余+数组的形式,节省很多性能
    // 其他的就是模板题
    [TestCase(new[] { 12, 12, 30, 24, 24 }, ExpectedResult = 2)]
    [TestCase(new[] { 72, 48, 24, 3 },      ExpectedResult = 3)]
    public long CountCompleteDayPairs(int[] hours)
    {
        var       res = 0L;
        Span<int> dic = stackalloc int[25];


        foreach (var hour in hours)
        {
            res += dic[(24 - hour % 24) % 24];

            dic[hour % 24]++;
        }

        return res;
    }


    // 给你一个整数数组 hours，表示以 小时 为单位的时间，返回一个整数，表示满足 i < j 且 hours[i] + hours[j] 构成 整天 的下标对 i, j 的数目。
    //
    // 整天 定义为时间持续时间是 24 小时的 整数倍 。
    //
    // 例如，1 天是 24 小时，2 天是 48 小时，3 天是 72 小时，以此类推。
    //
    //
    //
    // 示例 1：
    //
    // 输入： hours = [12,12,30,24,24]
    //
    // 输出： 2
    //
    // 解释：
    //
    // 构成整天的下标对分别是 (0, 1) 和 (3, 4)。
    //
    // 示例 2：
    //
    // 输入： hours = [72,48,24,3]
    //
    // 输出： 3
    //
    // 解释：
    //
    // 构成整天的下标对分别是 (0, 1)、(0, 2) 和 (1, 2)。
    //
    //
    //
    // 提示：
    //
    // 1 <= hours.length <= 5 * 10^5
    // 1 <= hours[i] <= 10^9
}


/// <summary> 2874. 有序三元组中的最大值 II </summary>
public class Solution_2874
{
    // 等价1014的变体, 但是这里是三元组,所以需要枚举两个变量,转换成单变量问题
    // (part1:nums[i] - part2:nums[j]) * part3: [ nums[k] ] 的最大值 ,且 i < j < k
    // 1. 维护最大的(nums[i] - nums[j]) 枚举 nums[k] 即可
    // 2. part1 中 维护最大值, 枚举 part2, part3即可
    // 3. 因为是有序的,所以可以一次遍历,但是要注意顺序
    [TestCase(new[] { 12, 6, 1, 2, 7 },      ExpectedResult = 77)]
    [TestCase(new[] { 1, 10, 3, 4, 19 },     ExpectedResult = 133)]
    [TestCase(new[] { 1, 2, 3 },             ExpectedResult = 0)]
    [TestCase(new[] { 1000000, 1, 1000000 }, ExpectedResult = 999999000000)]
    public long MaximumTripletValue(int[] nums)
    {
        var  res      = Math.Max(0,       (nums[0] - nums[1]) * (long)nums[2]);
        long maxPart1 = Math.Max(nums[0], nums[1]);
        var  maxPart2 = maxPart1 - nums[1];

        for (var i = 2 ; i < nums.Length - 1 ; i++)
        {
            maxPart2 = Math.Max(maxPart2, maxPart1 - nums[i]);
            maxPart1 = Math.Max(maxPart1, nums[i]);
            res      = Math.Max(res,      maxPart2 * nums[i + 1]);
        }

        return res;
    }

    // 给你一个下标从 0 开始的整数数组 nums
    // 请你从所有满足 i < j < k 的下标三元组 (i, j, k) 中，找出并返回下标三元组的最大值。如果所有满足条件的三元组的值都是负数，则返回 0 
    // 下标三元组 (i, j, k) 的值等于 (nums[i] - nums[j]) * nums[k] 
    // 3 <= nums.length <= 10^5
    // 1 <= nums[i]     <= 10^6
    //
    // 示例 1：
    //
    // 输入：nums = [12,6,1,2,7]
    // 输出：77
    // 解释：下标三元组 (0, 2, 4) 的值是 (nums[0] - nums[2]) * nums[4] = 77 。
    // 可以证明不存在值大于 77 的有序下标三元组。
    // 示例 2：
    //
    // 输入：nums = [1,10,3,4,19]
    // 输出：133
    // 解释：下标三元组 (1, 2, 4) 的值是 (nums[1] - nums[2]) * nums[4] = 133 。
    // 可以证明不存在值大于 133 的有序下标三元组。 
    // 示例 3：
    //
    // 输入：nums = [1,2,3]
    // 输出：0
    // 解释：唯一的下标三元组 (0, 1, 2) 的值是一个负数，(nums[0] - nums[1]) * nums[2] = -3 。因此，答案是 0 。
    //
    //
    // 提示：
    //
}


/// <summary> 2971. 找到最大周长的多边形 </summary>
public class Solution_2971
{
    // 由题干挖掘: 依旧是找到满足公式的n个元素的最大值
    // 公式为 : a1 + a2 + a3 + ... + ak-1 > ak
    //    即 :　sum(a) - a_max > a_max
    //         sum(a) > 2 * a_max 
    // 1. 由于是周长,所以需要排序(为什么呢? 因为他需要保证新的边比之前所有的边都要大,排序的话,可以保证之前累加的和中的边,没有最大的
    // 2. 所以这题维护的'左' 是累加的和, '右' 是当前的边 ,并且判断是否满足条件
    // 3. 不得不感叹真的是千变万化呀
    [TestCase(new[] { 5, 5, 5 },               ExpectedResult = 15)]
    [TestCase(new[] { 1, 12, 1, 2, 5, 50, 3 }, ExpectedResult = 12)]
    [TestCase(new[] { 5, 5, 50 },              ExpectedResult = -1)]
    public long LargestPerimeter(int[] nums)
    {
        Array.Sort(nums);

        var sum = 0L;
        var max = -1L;

        foreach (var num in nums)
        {
            sum += num;
            if (sum > 2 * num)
            {
                max = Math.Max(max, sum);
            }
        }

        return max;
    }

    // 给你一个长度为 n 的 正 整数数组 nums
    // 多边形 指的是一个至少有 3 条边的封闭二维图形。多边形的 最长边 一定 小于 所有其他边长度之和
    // 如果你有 k （k >= 3）个 正 数 a1，a2，a3, ...，ak 满足 a1 <= a2 <= a3 <= ... <= ak 且 a1 + a2 + a3 + ... + ak-1 > ak ，那么 一定 存在一个 k 条边的多边形，每条边的长度分别为 a1 ，a2 ，a3 ， ...，ak
    // 一个多边形的 周长 指的是它所有边之和
    // 请你返回从 nums 中可以构造的 多边形 的 最大周长 。如果不能构造出任何多边形，请你返回 -1 

    // 示例 1：
    // 输入：nums = [5,5,5]
    // 输出：15
    // 解释：nums 中唯一可以构造的多边形为三角形，每条边的长度分别为 5 ，5 和 5 ，周长为 5 + 5 + 5 = 15 。
    // 示例 2：
    // 输入：nums = [1,12,1,2,5,50,3] => [1,1,2,3,5,12,50]
    // 输出：12
    // 解释：最大周长多边形为五边形，每条边的长度分别为 1 ，1 ，2 ，3 和 5 ，周长为 1 + 1 + 2 + 3 + 5 = 12 。
    // 我们无法构造一个包含变长为 12 或者 50 的多边形，因为其他边之和没法大于两者中的任何一个。
    // 所以最大周长为 12 。
    // 示例 3：
    // 输入：nums = [5,5,50]
    // 输出：-1
    // 解释：无法构造任何多边形，因为多边形至少要有 3 条边且 50 > 5 + 5 。
    //
    //
    // 提示：
    //
    // 3 <= n <= 10^5
    // 1 <= nums[i] <= 10^9
}


/// <summary> 454. 四数相加 II </summary>
public class Solution_454
{
    // 454. 四数相加 II
    // 这题直接给出了四个数组,但是实际上依然可以使用双变量问题的思路来解决,是双变量的扩展
    // 由题目可知 i + j + k + l = 0, 可以转换成 i + j = -k - l
    // 可以将四个数组分成两组,分别计算两组的和,然后再计算两组的和的组合
    public int FourSumCount(int[] nums1, int[] nums2, int[] nums3, int[] nums4)
    {
        var result = 0;
        var dicSum = new Dictionary<int, int>();

        // 计算两组的和
        // 1. 枚举 : i + j 的所有组合
        foreach (var num1 in nums1)
        {
            foreach (var num2 in nums2)
            {
                var sum = num1 + num2;

                if (!dicSum.TryAdd(sum, 1))
                {
                    dicSum[sum]++;
                }
            }
        }

        // 2. 枚举 : k + l 的所有组合
        foreach (var num1 in nums3)
        {
            foreach (var num2 in nums4)
            {
                var sum = num1 + num2;

                // 3. 查找 : 更具推导公式 : i + j = -k - l ,是否已经存在 -sum 的组合 ,然后直接计数
                if (dicSum.TryGetValue(-sum, out var count))
                {
                    result += count;
                }
            }
        }


        return result;
    }

    //给你四个整数数组 nums1、nums2、nums3 和 nums4 ，数组长度都是 n ，请你计算有多少个元组 (i, j, k, l) 能满足：
    // 0 <= i, j, k, l < n
    // nums1[i] + nums2[j] + nums3[k] + nums4[l] == 0
    // 1 <= n <= 200
    // -2^28 <= nums1[i], nums2[i], nums3[i], nums4[i] <= 2^28
}