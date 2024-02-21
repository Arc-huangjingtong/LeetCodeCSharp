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
}