namespace LeetCodeCSharp;

//////////////////////////////////////////////////////// 贪婪问题 ////////////////////////////////////////////////////////
// 1. 这类问题很难抽象出一个统一的步骤,但是在思想上,都是能找到一个局部最优解,"这个操作是百利而无一害的"
// 2. 和动态规划的区别是,贪心问题不需要回溯,因为每一步都是最优解,不需要考虑之前的状态


/// 生成特殊数字的最少操作
[MemoryDiagnoser]
public class Solution_2844
{
    [TestCase("2245047", ExpectedResult = 2)]
    public static int MinimumOperations_ArcMethod(string num)
    {
        // 1. 自己在写的时候,总体上都是正确的,但是逻辑有点混乱(但是速度更快),在此记录,建议直接看灵神的做法
        int index1 = -1, index2 = -1;

        for (var i = num.Length - 1 ; i >= 0 ; i--)
        {
            if (num[i] == '0')
            {
                index1 = i;

                i--;


                for (; i >= 0 ; i--)
                {
                    if (num[i] is '5' or '0')
                    {
                        index2 = i;
                        break;
                    }
                }

                break;
            }
        }

        int index3 = -1, index4 = -1;

        for (var i = num.Length - 1 ; i >= 0 ; i--)
        {
            if (num[i] == '5')
            {
                index3 = i;
                i--;
                for (; i >= 0 ; i--)
                {
                    if (num[i] is '2' or '7')
                    {
                        index4 = i;
                        break;
                    }
                }

                break;
            }
        }

        var success1 = index1 != -1 && index2 != -1;
        var success2 = index3 != -1 && index4 != -1;

        if (!success1 && !success2)
        {
            if (index1 != -1)
            {
                return num.Length - 1;
            }

            return num.Length;
        }

        var result1 = success1 ? num.Length - index2 - 2 : int.MaxValue;
        var result2 = success2 ? num.Length - index4 - 2 : int.MaxValue;


        return Math.Min(result1, result2);
    }


    [TestCase("2245047", ExpectedResult = 2)]
    public static int MinimumOperations_LingShenMethod(string num)
    {
        var len = num.Length;

        bool found0 = false, found5 = false; //用一个flag标记是否找到了的状态,然后在循环中依次判断


        for (var i = len - 1 ; i >= 0 ; i--)
        {
            var c = num[i];
            if (found0 && (c == '0' || c == '5') ||
                found5 && (c == '2' || c == '7'))
            {
                return len - i - 2;
            }


            found0 |= c == '0';
            found5 |= c == '5';
        }

        return found0 ? len - 1 : len;
    }

    // 1. 不含前导0 
    // 2. 能被75整除的最小数 末尾特征: 00 50 25 75
    // 3. 如果都找不到,则全部删除,或者删到只剩一个 0 
    // 先找 0 - 5,0  再找 5 - 2,7


    // 给你一个下标从 0 开始的字符串 num ，表示一个非负整数。
    //
    // 在一次操作中，您可以选择 num 的任意一位数字并将其删除。请注意，如果你删除 num 中的所有数字，则 num 变为 0。
    //
    // 返回最少需要多少次操作可以使 num 变成特殊数字。
    //
    // 如果整数 x 能被 25 整除，则该整数 x 被认为是特殊数字。


    [Benchmark]
    public void Test_Arc() => MinimumOperations_ArcMethod("2245047");

    [Benchmark]
    public void Test_LingShen() => MinimumOperations_ArcMethod("2245047");
}


/// <summary> 3111. 覆盖所有点的最少矩形数目 </summary>
public class Solution_3111
{
    public int MinRectanglesToCoverPoints(int[][] points, int w)
    {
        // 选中所有横坐标,排序
        var sorted = points.Select(p => p[0]).OrderBy(x => x);


        var max = -1;
        var res = 0;

        foreach (var x in sorted)
        {
            if (x <= max) continue;

            // 贪心点 :　直接选最大宽度
            // 贪心点 :　如果不够，则直接从下一个点开始
            max = x + w;

            res++;
        }


        return res;
    }



    // 题目是说,用等宽(宽度小于w)的条状矩形,覆盖二维空间下 points中的所有点,最少需要多少个矩形
}