namespace LeetCodeCSharp;

/// Definition for singly-linked list.
public class ListNode(int val = 0, ListNode next = null)
{
    public int      val  = val;
    public ListNode next = next;
}


/// Definition for a binary tree node.
public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int      val   = val;
    public TreeNode left  = left;
    public TreeNode right = right;
}


/// Definition for a Node.
public class Node(int _val = default, IList<Node> _children = default)
{
    public int         val      = _val;
    public IList<Node> children = _children;
}

