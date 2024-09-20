namespace LeetCodeCSharp;

/// <summary> 1441. 用栈操作构建数组 算术评级: 3 第 188 场周赛Q1-1180 </summary>
public class Solution_1441
{
    string Push = "Push";
    string Pop  = "Pop";

    public IList<string> BuildArray(int[] target, int n)
    {
        var res = new List<string>();
        var cur = 1;

        for (int i = 0 ; i < target.Length ; i++)
        {
            while (cur < target[i])
            {
                res.Add(Push);
                res.Add(Pop);
                cur++;
            }

            res.Add(Push);
            cur++;
        }


        return res;
    }


    // 给你一个数组 target 和一个整数 n。每次迭代，需要从  list = { 1 , 2 , 3 ..., n } 中依次读取一个数字。
    //
    // 请使用下述操作来构建目标数组 target ：
    //
    // "Push"：从 list 中读取一个新元素， 并将其推入数组中。
    // "Pop"：删除数组中的最后一个元素。
    // 如果目标数组构建完成，就停止读取更多元素。
    // 题目数据保证目标数组严格递增，并且只包含 1 到 n 之间的数字。
    //
    // 请返回构建目标数组所用的操作序列。如果存在多个可行方案，返回任一即可。
    //
    //
    //
    // 示例 1：
    //
    // 输入：target = [1,3], n = 3
    // 输出：["Push","Push","Pop","Push"]
    // 解释： 
    // 读取 1 并自动推入数组 -> [1]
    // 读取 2 并自动推入数组，然后删除它 -> [1]
    // 读取 3 并自动推入数组 -> [1,3]
    // 示例 2：
    //
    // 输入：target = [1,2,3], n = 3
    // 输出：["Push","Push","Push"]
    // 示例 3：
    //
    // 输入：target = [1,2], n = 4
    // 输出：["Push","Push"]
    // 解释：只需要读取前 2 个数字就可以停止。
    //
    //
    // 提示：
    //
    // 1 <= target.length <= 100
    // 1 <= n <= 100
    // 1 <= target[i] <= n
    //     target 严格递增
}


/// <summary> 1472. 设计浏览器历史记录 算术评级: 6 第 192 场周赛Q3-1454 </summary>
public class Solution_1472
{
    public class BrowserHistory(string homepage)
    {
        private readonly Stack<string> _backStack    = [];
        private readonly Stack<string> _forwardStack = [];

        public void Visit(string url)
        {
            _forwardStack.Clear();
            _backStack.Push(url);
        }

        public string Back(int steps)
        {
            while (steps > 0 && _backStack.Count >= 1)
            {
                _forwardStack.Push(_backStack.Pop());
                steps--;
            }

            return _backStack.Count == 0 ? homepage : _backStack.Peek();
        }

        public string Forward(int steps)
        {
            while (steps > 0 && _forwardStack.Count >= 1)
            {
                _backStack.Push(_forwardStack.Pop());
                steps--;
            }

            return _backStack.Count == 0 ? homepage : _backStack.Peek();
        }
    }


    /**
     * Your BrowserHistory object will be instantiated and called as such:
     * BrowserHistory obj = new BrowserHistory(homepage);
     * obj.Visit(url);
     * string param_2 = obj.Back(steps);
     * string param_3 = obj.Forward(steps);
     */



    // 你有一个只支持单个标签页的 浏览器 ，最开始你浏览的网页是 homepage ，你可以访问其他的网站 url ，也可以在浏览历史中后退 steps 步或前进 steps 步。
    //
    // 请你实现 BrowserHistory 类：
    //
    // BrowserHistory(string homepage) ，用 homepage 初始化浏览器类。
    // void visit(string url) 从当前页跳转访问 url 对应的页面  。执行此操作会把浏览历史前进的记录全部删除。
    // string back(int steps) 在浏览历史中后退 steps 步。如果你只能在浏览历史中后退至多 x 步且 steps > x ，那么你只后退 x 步。请返回后退 至多 steps 步以后的 url 。
    // string forward(int steps) 在浏览历史中前进 steps 步。如果你只能在浏览历史中前进至多 x 步且 steps > x ，那么你只前进 x 步。请返回前进 至多 steps步以后的 url 。
    //  
    //
    // 示例：
    //
    // 输入：
    // ["BrowserHistory","visit","visit","visit","back","back","forward","visit","forward","back","back"]
    // [["leetcode.com"],["google.com"],["facebook.com"],["youtube.com"],[1],[1],[1],["linkedin.com"],[2],[2],[7]]
    // 输出：
    // [null,null,null,null,"facebook.com","google.com","facebook.com",null,"linkedin.com","google.com","leetcode.com"]
    //
    // 解释：
    // BrowserHistory browserHistory = new BrowserHistory("leetcode.com");
    // browserHistory.visit("google.com");       // 你原本在浏览 "leetcode.com" 。访问 "google.com"
    // browserHistory.visit("facebook.com");     // 你原本在浏览 "google.com" 。访问 "facebook.com"
    // browserHistory.visit("youtube.com");      // 你原本在浏览 "facebook.com" 。访问 "youtube.com"
    // browserHistory.back(1);                   // 你原本在浏览 "youtube.com" ，后退到 "facebook.com" 并返回 "facebook.com"
    // browserHistory.back(1);                   // 你原本在浏览 "facebook.com" ，后退到 "google.com" 并返回 "google.com"
    // browserHistory.forward(1);                // 你原本在浏览 "google.com" ，前进到 "facebook.com" 并返回 "facebook.com"
    // browserHistory.visit("linkedin.com");     // 你原本在浏览 "facebook.com" 。 访问 "linkedin.com"
    // browserHistory.forward(2);                // 你原本在浏览 "linkedin.com" ，你无法前进任何步数。
    // browserHistory.back(2);                   // 你原本在浏览 "linkedin.com" ，后退两步依次先到 "facebook.com" ，然后到 "google.com" ，并返回 "google.com"
    // browserHistory.back(7);                   // 你原本在浏览 "google.com"， 你只能后退一步到 "leetcode.com" ，并返回 "leetcode.com"
    //  
    //
    // 提示：
    //
    // 1 <= homepage.length <= 20
    // 1 <= url.length <= 20
    // 1 <= steps <= 100
    // homepage 和 url 都只包含 '.' 或者小写英文字母。
    // 最多调用 5000 次 visit， back 和 forward 函数。
}


/// <summary> 71. 简化路径 算术评级: 6 中等 </summary>
public class Solution_71
{
    [TestCase("/home/",                           ExpectedResult = "/home")]
    [TestCase("/home//foo/",                      ExpectedResult = "/home/foo")]
    [TestCase("/home/user/Documents/../Pictures", ExpectedResult = "/home/user/Pictures")]
    [TestCase("/../",                             ExpectedResult = "/")]
    [TestCase("/.../a/../b/c/../d/./",            ExpectedResult = "/.../b/d")]
    public string SimplifyPath(string path)
    {
        var words = path.Split('/');
        var stack = new Stack<string>();
        stack.Push("/");

        foreach (var word in words)
        {
            if (word == "..")
            {
                if (stack.Count > 1)
                {
                    stack.Pop();
                    stack.Pop();
                }
            }
            else if (word != "." && word != "")
            {
                stack.Push(word);
                stack.Push("/");
            }
        }

        if (stack.Count > 1 && stack.Peek() == "/")
        {
            stack.Pop();
        }


        return string.Join("", stack.Reverse());
    }

    // 
    // 给你一个字符串 path ，表示指向某一文件或目录的 Unix 风格 绝对路径 （以 '/' 开头），请你将其转化为 更加简洁的规范路径。
    //
    // 在 Unix 风格的文件系统中规则如下：
    //
    // 一个点 '.' 表示当前目录本身。
    // 此外，两个点 '..' 表示将目录切换到上一级（指向父目录）。
    // 任意多个连续的斜杠（即，'//' 或 '///'）都被视为单个斜杠 '/'。
    // 任何其他格式的点（例如，'...' 或 '....'）均被视为有效的文件/目录名称。
    // 返回的 简化路径 必须遵循下述格式：
    //
    // 始终以斜杠 '/' 开头。
    // 两个目录名之间必须只有一个斜杠 '/' 。
    // 最后一个目录名（如果存在）不能 以 '/' 结尾。
    // 此外，路径仅包含从根目录到目标文件或目录的路径上的目录（即，不含 '.' 或 '..'）。
    // 返回简化后得到的 规范路径 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：path = "/home/"
    //
    // 输出："/home"
    //
    // 解释：
    //
    // 应删除尾随斜杠。
    //
    // 示例 2：
    //
    // 输入：path = "/home//foo/"
    //
    // 输出："/home/foo"
    //
    // 解释：
    //
    // 多个连续的斜杠被单个斜杠替换。
    //
    // 示例 3：
    //
    // 输入：path = "/home/user/Documents/../Pictures"
    //
    // 输出："/home/user/Pictures"
    //
    // 解释：
    //
    // 两个点 ".." 表示上一级目录（父目录）。
    //
    // 示例 4：
    //
    // 输入：path = "/../"
    //
    // 输出："/"
    //
    // 解释：
    //
    // 不可能从根目录上升一级目录。
    //
    // 示例 5：
    //
    // 输入：path = "/.../a/../b/c/../d/./"
    //
    // 输出："/.../b/d"
    //
    // 解释：
    //
    // "..." 在这个问题中是一个合法的目录名。
    //
    //
    //
    // 提示：
    //
    // 1 <= path.length <= 3000
    // path 由英文字母，数字，'.'，'/' 或 '_' 组成。
    // path 是一个有效的 Unix 风格绝对路径。
}