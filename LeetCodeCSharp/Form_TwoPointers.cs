namespace LeetCodeCSharp;

/**************************************************  双指针和滑动窗口  ****************************************************/


/// <summary> 2024. 考试的最大困扰度 算术评级: 5 第 62 场双周赛Q3-1643 </summary>
public class Solution_2024
{
    [TestCase("TTFTTFTT", 1, ExpectedResult = 5)]
    [TestCase("TTFF",     2, ExpectedResult = 4)]
    [TestCase("TFFT",     1, ExpectedResult = 3)]
    public int MaxConsecutiveAnswers2(string answerKey, int k)
    {
        var len = answerKey.Length;
        var res = 0;

        for (int left = 0, right = 0, sum = 0 ; right < len ; right++)
        {
            sum += answerKey[right] != 'T' ? 1 : 0;
            while (sum > k)
            {
                sum -= answerKey[left++] != 'T' ? 1 : 0;
            }

            res = Math.Max(res, right - left + 1);
        }

        for (int left = 0, right = 0, sum = 0 ; right < len ; right++)
        {
            sum += answerKey[right] != 'F' ? 1 : 0;
            while (sum > k)
            {
                sum -= answerKey[left++] != 'F' ? 1 : 0;
            }

            res = Math.Max(res, right - left + 1);
        }


        return res;
    }

    //     一位老师正在出一场由 n 道判断题构成的考试，每道题的答案为 true （用 'T' 表示）或者 false （用 'F' 表示）。老师想增加学生对自己做出答案的不确定性，方法是 最大化 有 连续相同 结果的题数。（也就是连续出现 true 或者连续出现 false）。
    //
    // 给你一个字符串 answerKey ，其中 answerKey[i] 是第 i 个问题的正确结果。除此以外，还给你一个整数 k ，表示你能进行以下操作的最多次数：
    //
    // 每次操作中，将问题的正确答案改为 'T' 或者 'F' （也就是将 answerKey[i] 改为 'T' 或者 'F' ）。
    // 请你返回在不超过                          k 次操作的情况下，最大 连续 'T' 或者 'F' 的数目。
    //
    //
    //
    // 示例 1：
    //
    // 输入：answerKey = "TTFF", k = 2
    // 输出：4
    // 解释：我们可以将两个 'F' 都变为 'T' ，得到 answerKey = "TTTT" 。
    // 总共有四个连续的 'T' 。
    // 示例 2：
    //
    // 输入：answerKey = "TFFT", k = 1
    // 输出：3
    // 解释：我们可以将最前面的 'T' 换成 'F' ，得到 answerKey = "FFFT" 。
    // 或者，我们可以将第二个 'T' 换成 'F' ，得到  answerKey = "TFFF" 。
    // 两种情况下，都有三个连续的 'F' 。
    // 示例 3：
    //
    // 输入：answerKey = "TTFTTFTT", k = 1
    // 输出：5
    // 解释：我们可以将第一个 'F' 换成 'T' ，得到 answerKey = "TTTTTFTT" 。
    // 或者我们可以将第二个 'F' 换成 'T' ，得到  answerKey = "TTFTTTTT" 。
    // 两种情况下，都有五个连续的 'T' 。
    //
    //
    // 提示：
    //
    // n == answerKey.length
    // 1 <= n <= 5 * 104
    // answerKey[i] 要么是 'T' ，要么是 'F'
    // 1 <= k <= n
}


/// <summary> 2181. 合并零之间的节点 算术评级: 3 第 281 场周赛Q2-1333 </summary>
public class Solution_2181
{
    // [0,3,1,0,4,5,2,0]
    public ListNode MergeNodes(ListNode head)
    {
        var result  = head;
        var last    = head;
        var current = head.next;

        while (current != null)
        {
            if (current.val == 0)
            {
                if (current.next == null)
                {
                    last.next = null;
                    break;
                }

                last.next = current;
                last      = last.next;
            }
            else
            {
                last.val += current.val;
            }

            current = current.next;
        }


        return result;
    }

    // 给你一个链表的头节点 head ，该链表包含由 0 分隔开的一连串整数。链表的 开端 和 末尾 的节点都满足 Node.val == 0
    //
    // 对于每两个相邻的 0 ，请你将它们之间的所有节点合并成一个节点，其值是所有已合并节点的值之和。然后将所有 0 移除，修改后的链表不应该含有任何 0
    //
    // 返回修改后链表的头节点 head 。
    //
    // 示例 1：
    //
    //
    // 输入：head = [0,3,1,0,4,5,2,0]
    // 输出：[4,11]
    // 解释：
    // 上图表示输入的链表。修改后的链表包含：
    // - 标记为绿色的节点之和：3 + 1 = 4
    // - 标记为红色的节点之和：4 + 5 + 2 = 11
    // 示例 2：
    //
    //
    // 输入：head = [0,1,0,3,0,2,2,0]
    // 输出：[1,3,4]
    // 解释：
    // 上图表示输入的链表。修改后的链表包含：
    // - 标记为绿色的节点之和：1 = 1
    // - 标记为红色的节点之和：3 = 3
    // - 标记为黄色的节点之和：2 + 2 = 4
    //
    //
    // 提示：
    //
    // 列表中的节点数目在范围 [3, 2 * 105] 内
    // 0 <= Node.val <= 1000
    // 不存在连续两个 Node.val == 0 的节点
    // 链表的 开端 和 末尾 节点都满足 Node.val == 0
}