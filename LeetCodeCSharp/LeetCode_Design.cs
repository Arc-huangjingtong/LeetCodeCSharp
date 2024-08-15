namespace LeetCodeCSharp;
///////////////////////////////////////////////////////设计相关题型///////////////////////////////////////////////////////


/// <summary> 设计哈希集合 </summary>
public class Solution_705
{
    public class MyHashSet
    {
        private readonly bool[] _hashSet = new bool[1000001];


        public void Add(int key) => _hashSet[key] = true;

        public void Remove(int key) => _hashSet[key] = false;

        public bool Contains(int key) => _hashSet[key];
    }


    public class MyHashSet2
    {
        public readonly IList<int>[] set = new IList<int>[BASE];

        private const int BASE = 769;

        public MyHashSet2()
        {
            for (var i = 0 ; i < BASE ; ++i)
            {
                set[i] = [];
            }
        }



        // 类似于桶排序,将key分流到不同的list中
        public void Add(int key)
        {
            if (Contains(key)) return;

            set[Hash(key)].Add(key);
        }

        public void Remove(int key) => set[Hash(key)].Remove(key);

        // 最终利用分流后的list循环遍历判断是否存在
        public bool Contains(int key) => set[Hash(key)].Contains(key);

        // 通过取模运算,将key映射到哈希表中的索引
        private static int Hash(int key) => key % BASE;
    }
}


/// <summary> 设计哈希映射 </summary>
public class Solution_706
{
    //0 <= key, value <= 10^6
    public class MyHashMap2
    {
        private readonly int[] _hashMap = new int[1000001];

        public MyHashMap2() => Array.Fill(_hashMap, -1);


        public void Put(int    key, int value) => _hashMap[key] = value;
        public int  Get(int    key) => _hashMap[key];
        public void Remove(int key) => _hashMap[key] = -1;
    }


    public class MyHashMap
    {
        public readonly Node[] bucket;
        public const    int    hashCode = 128;

        public MyHashMap()
        {
            bucket = new Node[128];
            for (var i = 0 ; i < 128 ; i++)
            {
                bucket[i] = new Node(-1, -1);
            }
        }

        public void Put(int key, int value)
        {
            var index = key % hashCode;
            var p     = bucket[index];
            while (p.next != null)
            {
                if (p.key == key)
                {
                    p.value = value;
                    return;
                }

                p = p.next;
            }

            p.key   = key;
            p.value = value;
            p.next  = new Node(-1, -1);
        }

        public int Get(int key)
        {
            var index = key % hashCode;
            var p     = bucket[index];
            while (p.next != null)
            {
                if (p.key == key)
                {
                    return p.value;
                }

                p = p.next;
            }

            return -1;
        }

        public void Remove(int key)
        {
            var index = key % hashCode;
            var p     = bucket[index];
            while (p.next != null)
            {
                if (p.key == key)
                {
                    bucket[index] = p.next;
                    return;
                }

                if (p.next.key == key)
                {
                    p.next = p.next.next;
                    return;
                }

                p = p.next;
            }

            return;
        }


        public class Node(int k, int v)
        {
            public int  key   = k;
            public int  value = v;
            public Node next;
        }
    }
}


/// <summary> 快照数组 </summary>
public class Solution_1146
{
    public class SnapshotArray
    {
        private          int                snap_cnt = 0;
        private readonly List<(int, int)>[] data     = []; //每一位都是一个链表

        public SnapshotArray(int length)
        {
            for (var i = 0 ; i < length ; i++)
            {
                data[i] = [];
            }
        }

        ///<summary> 将指定索引index处的元素设置为val </summary>
        public void Set(int index, int val) => data[index].Add((snap_cnt, val));

        public int Snap() => snap_cnt++;

        public int Get(int index, int snap_id)
        {
            var x = BinarySearch(index, snap_id);
            return x == 0 ? 0 : data[index][x - 1].Item2;
        }

        //二分法查找 snap_id
        private int BinarySearch(int index, int snap_id)
        {
            int low = 0, high = data[index].Count;
            while (low < high)
            {
                var mid  = low + (high - low) / 2;
                var pair = data[index][mid];
                if (pair.Item1 > snap_id + 1 || (pair.Item1 == snap_id + 1 && pair.Item2 >= 0))
                {
                    high = mid;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return low;
        }
    }


    [Test]
    public void Test()
    {
        //["SnapshotArray","set","snap","snap","snap","get","snap","snap","get"]
        //[[1],[0,15],[],[],[],[0,2],[],[],[0,0]]
        var obj = new SnapshotArray(1);
        obj.Set(0, 15);
        obj.Snap();
        obj.Snap();
        obj.Snap();
        obj.Get(0, 2); //15
        obj.Snap();
        obj.Snap();
        obj.Get(0, 0);
    }
}


/// <summary> 设计魔法字典 </summary>
public class Solution_676
{
    public class MagicDictionary
    {
        // 如何判断只有一个字母不同?
        // 1. 长度相同
        // 2. 只有一个字母不同

        public string[] _dictionary;

        public void BuildDict(string[] dictionary)
        {
            _dictionary = dictionary;
        }

        public bool Search(string searchWord)
        {
            foreach (var word in _dictionary)
            {
                if (word.Length != searchWord.Length) continue;

                var diff = 0;
                for (var i = 0 ; i < word.Length ; i++)
                {
                    if (word[i] != searchWord[i])
                    {
                        diff++;
                        if (diff > 1) break;
                    }
                }

                if (diff == 1) return true;
            }

            return false;
        }
    }


    /**
     * Your MagicDictionary object will be instantiated and called as such:
     * MagicDictionary obj = new MagicDictionary();
     * obj.BuildDict(dictionary);
     * bool param_2 = obj.Search(searchWord);
     */

    // 设计一个使用单词列表进行初始化的数据结构，单词列表中的单词 互不相同 。 如果给出一个单词，请判定能否只将这个单词中一个字母换成另一个字母，使得所形成的新单词存在于你构建的字典中。
    //
    // 实现 MagicDictionary 类：
    //
    // MagicDictionary() 初始化对象
    // void buildDict(String[] dictionary) 使用字符串数组 dictionary 设定该数据结构，dictionary 中的字符串互不相同
    // bool search(String      searchWord) 给定一个字符串 searchWord ，判定能否只将字符串中        一个 字母换成另一个字母，使得所形成的新字符串能够与字典中的任一字符串匹配。如果可以，返回 true ；否则，返回 false 。


    // 示例：
    //
    // 输入
    // ["MagicDictionary", "buildDict", "search", "search", "search", "search"]
    // [[], [["hello", "leetcode"]], ["hello"], ["hhllo"], ["hell"], ["leetcoded"]]
    // 输出
    // [null, null, false, true, false, false]
    //
    // 解释
    //     MagicDictionary magicDictionary = new MagicDictionary();
    // magicDictionary.buildDict(["hello", "leetcode"]);
    // magicDictionary.search("hello");     // 返回 False
    // magicDictionary.search("hhllo");     // 将第二个 'h' 替换为 'e' 可以匹配 "hello" ，所以返回 True
    // magicDictionary.search("hell");      // 返回 False
    // magicDictionary.search("leetcoded"); // 返回 False
    //
    //
    // 提示：
    //
    // 1 <= dictionary.length <= 100
    // 1 <= dictionary[i].length <= 100
    // dictionary[i] 仅由小写英文字母组成
    //     dictionary 中的所有字符串 互不相同
    // 1 <= searchWord.length <= 100
    // searchWord 仅由小写英文字母组成
    // buildDict  仅在 search 之前调用一次
    // 最多调用 100 次 search
}