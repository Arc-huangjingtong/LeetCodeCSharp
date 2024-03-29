﻿namespace LeetCodeCSharp;

//********************************************************************************************************************//


public partial class UnitTest
{
    /// Gosper's hack algorithm
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
}