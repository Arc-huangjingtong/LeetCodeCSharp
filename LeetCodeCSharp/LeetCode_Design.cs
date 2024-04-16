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