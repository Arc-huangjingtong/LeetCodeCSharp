namespace LeetCodeCSharp;

public partial class UnitTest
{
    /// <summary>更具数组生成树</summary>
    /// <param name="array">可空的,层序遍历的数组</param>
    /// <returns>基于TreeNode生成的树结构</returns>
    public static TreeNode CreateTree(int?[] array)
    {
        if (array.Length == 0 || array[0] == null)
        {
            return null!;
        }

        var root  = new TreeNode(array[0]!.Value);
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        for (var i = 1 ; i < array.Length ; i++)
        {
            if (queue.Count == 0)
            {
                break;
            }

            var node = queue.Dequeue();
            if (array[i] != null)
            {
                node.left = new TreeNode(array[i]!.Value);
                queue.Enqueue(node.left);
            }

            i++;
            if (i < array.Length && array[i] != null)
            {
                node.right = new TreeNode(array[i]!.Value);
                queue.Enqueue(node.right);
            }
        }

        return root;
    }


    /// <summary>更具数组字符串生成树</summary>
    /// <param name="arrayString">层序遍历的数组字符串</param>
    /// <returns>基于TreeNode生成的树结构</returns>
    public static TreeNode CreateTree(string arrayString)
    {
        arrayString = arrayString.Trim('[', ']');
        var array = arrayString.Split(',');
        var nodes = new int?[array.Length];
        for (var i = 0 ; i < array.Length ; i++)
        {
            if (array[i] == "null")
            {
                nodes[i] = null;
            }
            else
            {
                nodes[i] = int.Parse(array[i]);
            }
        }

        return CreateTree(nodes);
    }


    /// <summary>更具树生成数组</summary>
    /// <param name="root">树的根节点</param>
    /// <returns>层序遍历的数组</returns>
    public static int?[] CreateArray(TreeNode root)
    {
        if (root == null)
        {
            return Array.Empty<int?>();
        }

        var list  = new List<int?>();
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == null)
            {
                list.Add(null);
            }
            else
            {
                list.Add(node.val);
                queue.Enqueue(node.left);
                queue.Enqueue(node.right);
            }
        }

        return list.ToArray();
    }


    /// <summary>更具树生成数组字符串</summary>
    /// <param name="root">树的根节点</param>
    /// <returns>层序遍历的数组字符串</returns>
    public static string CreateArrayString(TreeNode root)
    {
        var array = CreateArray(root);
        // 移除末尾的null

        var count = array.Length;
        for (var i = array.Length - 1 ; i >= 0 ; i--)
        {
            if (array[i] == null)
            {
                count--;
            }
            else
            {
                break;
            }
        }

        Array.Resize(ref array, count);

        return $"[{string.Join(',', array.Select(x => x == null ? "null" : x.ToString()))}]";
    }
}