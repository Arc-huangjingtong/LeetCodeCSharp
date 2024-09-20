namespace LeetCodeCSharp;
///////////////////////////////////////////////////////数学相关题型///////////////////////////////////////////////////////


///<summary> 1518. 换水问题 </summary>
public class Solution_1518
{
    [TestCase(9,  3, ExpectedResult = 13)]
    [TestCase(15, 4, ExpectedResult = 19)]
    public int NumWaterBottles(int numBottles, int numExchange)
    {
        if (numBottles >= numExchange)
        {
            var extra = (numBottles - numExchange) / (numExchange - 1) + 1;
            return extra + numBottles;
        }

        return numBottles;
    }

    //换水问题核心思路: 一次交换,n个空瓶子可以换1瓶水,实际上,因为一瓶水本身就是一个空瓶子,所以n-1个空瓶子可以换1瓶水
    //但是,如果空瓶子的数量小于n,那么就无法换水了,所以这个时候,我们就可以直接返回空瓶子的数量
    //所以只需要看倒数第二次换水的时候,剩下的空瓶子数量是否大于等于n,如果大于等于n,那么就可以继续换水,否则就直接返回
    //公式: b−n(e−1)≥e
}


public class Solution_2929
{
    public long DistributeCandies(int n, int limit)
    {
        return Cal(n + 2) - 3 * Cal(n - limit + 1) + 3 * Cal(n - (limit + 1) * 2 + 2) - Cal(n - 3 * (limit + 1) + 2);

        long Cal(int x) => x < 0 ? 0 : (long)x * (x - 1) / 2;
        //核心思路: 1. 从n个糖果中选出limit个糖果,可以有C(n, limit)种选法
    }

    [Test]
    public void Test()
    {
        var dict = new Dictionary<int, int>();

        dict.Add(1, 2);
        dict[1] = 2;
        dict[2] = 3;
    }
}


/// <summary> 2332. 坐上公交的最晚时间 算术评级: 6 第 82 场双周赛Q2-1841 </summary>
public class Solution_2332
{
    [TestCase(new[] { 10, 20, 30 }, new[] { 4, 11, 13, 19, 21, 25, 26 }, 2, ExpectedResult = 20)]
    public int LatestTimeCatchTheBus(int[] buses, int[] passengers, int capacity)
    {
        Array.Sort(buses);
        Array.Sort(passengers);
        int pos   = 0;
        int space = 0;

        foreach (int arrive in buses)
        {
            space = capacity;
            while (space > 0 && pos < passengers.Length && passengers[pos] <= arrive)
            {
                space--;
                pos++;
            }
        }

        pos--;
        int lastCatchTime = space > 0 ? buses[^1] : passengers[pos];
        // 核心思维点:如果最后一辆车没坐满,那么最晚时间就是最后一辆车的出发时间,否则就是最后一个乘客的到达时间
        while (pos >= 0 && passengers[pos] == lastCatchTime)
        {
            pos--;
            lastCatchTime--;
        }

        return lastCatchTime;
    }



    // 给你一个下标从 0 开始长度为 n 的整数数组 buses ，
    // 其中 buses[i] 表示第 i 辆公交车的出发时间。同时给你一个下标从 0 开始长度为 m 的整数数组 passengers ，其中 passengers[j] 表示第 j 位乘客的到达时间。所有公交车出发的时间互不相同，所有乘客到达的时间也互不相同。
    //
    // 给你一个整数 capacity ，表示每辆公交车 最多 能容纳的乘客数目。
    //
    // 每位乘客都会搭乘下一辆有座位的公交车。如果你在 y 时刻到达，公交在 x 时刻出发，满足 y <= x  且公交没有满，那么你可以搭乘这一辆公交。最早 到达的乘客优先上车。
    //
    // 返回你可以搭乘公交车的最晚到达公交站时间。你 不能 跟别的乘客同时刻到达。
    //
    // 注意：数组 buses 和 passengers 不一定是有序的。
    //
    //
    //
    // 示例 1：
    //
    // 输入：buses = [10,20], passengers = [2,17,18,19], capacity = 2
    // 输出：16
    // 解释：
    // 第 1 辆公交车载着第 1 位乘客。
    // 第 2 辆公交车载着你和第 2 位乘客。
    // 注意你不能跟其他乘客同一时间到达，所以你必须在第二位乘客之前到达。
    // 示例 2：
    //
    // 输入：buses = [20,30,10], passengers = [19,13,26,4,25,11,21], capacity = 2
    // 输出：20
    // 解释：
    // 第 1 辆公交车载着第 4 位乘客。
    // 第 2 辆公交车载着第 6 位和第 2 位乘客。
    // 第 3 辆公交车载着第 1 位乘客和你。
    //
    //
    // 提示：
    //
    // n == buses.length
    // m == passengers.length
    // 1 <= n, m, capacity <= 10^5
    // 2 <= buses[i], passengers[i] <= 10^9
    // buses      中的元素 互不相同 。
    // passengers 中的元素 互不相同 。
}


/// <summary> 946. 验证栈序列 算术评级: 5 第 112 场周赛Q2-1462 </summary>
public class Solution_946
{
    [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 4, 5, 3, 2, 1 }, ExpectedResult = true)]
    [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 4, 3, 5, 1, 2 }, ExpectedResult = false)]
    [TestCase(new[] { 2, 1, 0 },       new[] { 1, 2, 0 },       ExpectedResult = true)]
    public bool ValidateStackSequences(int[] pushed, int[] popped)
    {
        var stack     = new Stack<int>();
        var pushIndex = 0;
        var popIndex  = 0;

        while (true)
        {
            if (pushIndex < pushed.Length)
            {
                stack.Push(pushed[pushIndex++]);
            }

            while (stack.Count > 0 && stack.Peek() == popped[popIndex])
            {
                stack.Pop();
                popIndex++;
            }

            if (pushIndex >= pushed.Length)
            {
                break;
            }
        }

        return stack.Count == 0;
    }


    // [2,1,0]  
    // [1,2,0]

    // 给定 pushed 和 popped 两个序列，每个序列中的 值都不重复，
    // 只有当它们可能是在最初空栈上进行的推入 push 和弹出 pop 操作序列的结果时，返回 true；否则，返回 false 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：pushed = [1,2,3,4,5], popped = [4,5,3,2,1]
    // 输出：true
    // 解释：我们可以按以下顺序执行：
    // push(1), push(2), push(3), push(4), pop() -> 4,
    // push(5), pop() -> 5, pop() -> 3, pop() -> 2, pop() -> 1
    // 示例 2：
    //
    // 输入：pushed = [1,2,3,4,5], popped = [4,3,5,1,2] 
    // 输出：false 
    // 解释：1 不能在 2 之前弹出。
    //
    //
    // 提示：
    //
    // 1 <= pushed.length <= 1000
    // 0 <= pushed[i] <= 1000
    // pushed 的所有元素 互不相同
    //     popped.length == pushed.length
    //     popped 是 pushed 的一个排列
}


