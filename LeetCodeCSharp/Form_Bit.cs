namespace LeetCodeCSharp;

public class Solution_3133
{
    [TestCase(3, 4, ExpectedResult = 6)]
    [TestCase(2, 7, ExpectedResult = 15)]
    public long MinEnd(int n, int x)
    {
        long res = x;
        for (var i = 1 ; i < n ; i++)
        {
            res = (uint)x | (res + 1);
        }

        return res;
    }

    // 给你两个整数 n 和 x 。
    // 你需要构造一个长度为 n 的 正整数 数组 nums ,且严格递增
    // 并且数组 nums 中所有元素的按位 AND 运算结果为 x 。
    //
    // 返回 nums[^1] 可能的最小值

    // 输入：n = 3, x = 4
    // 输出：6
    // 解释：
    // 数组 nums 可以是 [4,5,6] ，最后一个元素为 6 
    // 4: 100
    // 5: 101
    // 6: 110

    // 输入：n = 2, x = 7
    // 输出：15
    // 解释：数组 nums 可以是 [7,15] ，最后一个元素为 15 。
    //  7 : 0111
    // 15 : 1111


    public long MinEnd2(int n, int x)
    {
        n--; // 先把 n 减一，这样下面讨论的 n 就是原来的 n-1
        long ans = x;
        int  i   = 0, j = 0;
        while ((n >> j) > 0)
        {
            // x 的第 i 个比特值是 0，即「空位」
            if ((ans >> i & 1) == 0)
            {
                // 空位填入 n 的第 j 个比特值
                ans |= (long)((n >> j) & 1) << i;
                j++;
            }

            i++;
        }

        return ans;
    }
}