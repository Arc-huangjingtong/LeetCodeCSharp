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


/// <summary> 690. 员工的重要性 </summary>
public class Solution_690
{
    // Definition for Employee.
    public class Employee
    {
        public int        id;
        public int        importance;
        public IList<int> subordinates;
    }


    // DFS 标准模板
    public int GetImportance(IList<Employee> employees, int id)
    {
        var dict = new Dictionary<int, Employee>();

        foreach (var employee in employees)
        {
            dict[employee.id] = employee;
        }

        return DFS(id);

        int DFS(int id)
        {
            var employee = dict[id];
            var sum      = employee.importance;

            foreach (var subordinate in employee.subordinates)
            {
                sum += DFS(subordinate);
            }

            return sum;
        }
    }
}