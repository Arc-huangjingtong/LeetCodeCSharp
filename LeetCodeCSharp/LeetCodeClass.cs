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
public class Node
{
    public int         val;
    public IList<Node> children;

    public Node() { }

    public Node(int _val)
    {
        val = _val;
    }

    public Node(int _val, IList<Node> _children)
    {
        val      = _val;
        children = _children;
    }
}