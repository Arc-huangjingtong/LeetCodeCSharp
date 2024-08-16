namespace LeetCodeCSharp;

/// <summary> 572. 另一棵树的子树 </summary>
public class Solution_572
{
    public bool IsSubtree(TreeNode root, TreeNode subRoot)
    {
        return DFS(root);

        bool DFS(TreeNode head)
        {
            if (head == null) return false;

            if (head.val == subRoot.val && IsSubtree(head, subRoot))
            {
                return true;
            }

            return DFS(head.left) || DFS(head.right);
        }


        bool IsSubtree(TreeNode head, TreeNode sub)
        {
            if (head == null && sub == null) return true;

            if (head?.val != sub?.val) return false;

            return IsSubtree(head?.left, sub?.left) || IsSubtree(head?.right, sub?.right);
        }
    }
}


