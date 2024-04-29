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