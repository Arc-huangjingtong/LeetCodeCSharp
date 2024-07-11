namespace LeetCodeCSharp;

// 双变量问题 : 两个变量之间有一个函数关系,可以通过只维护一个因变量来解决
// 例如 [1.两数之和] ai+aj = t , 可以枚举右边的 aj，转换成 单变量问题，也就是查找在 ai 左边是否有 ai = t−aj，这可以用哈希表维护。
// 这个技巧叫做 "枚举右，维护左"

// 双变量问题的特点:
// 1. 两个变量(或者通过换元,把多个最终转换成两个)之间有一个函数关系 [多变量换元: 454. 四数相加 II]
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