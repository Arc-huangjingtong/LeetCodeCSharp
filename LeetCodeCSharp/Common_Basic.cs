namespace LeetCodeCSharp
{

    public class ManySort
    {
        /// <summary> BubbleSort </summary>
        public static void BubbleSort(int[] arr)
        {
            var length = arr.Length;

            for (var i = 0 ; i < length - 1 ; i++)
            {
                for (var j = 0 ; j < length - i - 1 ; j++)
                {
                    if (arr[j] > arr[j + 1]) // 如果前一个比后一个大
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]); // 交换位置 
                    }
                }
            }
        }

        /// <summary> SelectionSort </summary>
        public static void SelectionSort(int[] arr)
        {
            var length = arr.Length;

            for (var i = 0 ; i < length - 1 ; i++)
            {
                var minIndex = i; // 假设当前位置是最小值的位置

                for (var j = i + 1 ; j < length ; j++)
                {
                    if (arr[j] < arr[minIndex]) // 如果当前位置的值比最小值还小
                    {
                        minIndex = j; // 更新最小值的位置
                    }
                }

                if (minIndex != i)
                {
                    (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
                }
            }
        }

        /// <summary> InsertionSort </summary>
        public static void InsertionSort(int[] arr)
        {
            var length = arr.Length;
            for (var i = 1 ; i < length ; ++i)
            {
                var key = arr[i];
                var j   = i - 1;

                while (j >= 0 && arr[j] > key) // 将arr[i]移动到其在前面子数组中正确的位置
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = key;
            }

            // 插入排序算法的工作原理基于对已排序和未排序的数据段进行操作。它逐步将未排序的元素插入到已排序的段中，从而达到整个数据集合的排序。具体来说，可以通过以下几个步骤来解释插入排序算法的工作原理：
            // 开始时假设：在开始时，我们假设序列的第一个元素自成一个已排序的段。即在初始状态下，序列的第一个元素既是最小的已排序段，也是唯一的元素。剩余的部分被视为未排序的段。
            // 选取元素：从未排序的段中选取第一个元素作为“关键元素”（key）。一开始，这个关键元素是序列中的第二个元素。
            // 比较和插入：将关键元素与已排序段中的元素从后向前逐一进行比较。如果关键元素小于已排序段中的某个元素，则将该元素向后移动一位，为关键元素腾出空间。这个过程一直进行，直到找到关键元素应该插入的位置。
            // 插入关键元素：将关键元素插入到已排序段中正确的位置。
            // 重复过程：重复步骤2到步骤4，直到所有的未排序元素都移动到了已排序段中。
            // 插入排序是通过不断地扩大已排序段，同时缩小未排序段，直到未排序段没有元素，从而完成整个排序过程。在每次迭代中，关键元素都是逐个从未排序段移动到已排序段的正确位置。
            // 举个例子：
            // 假设有一个序列 [5, 3, 4, 1]，我们要使用插入排序对其进行排序。
            // 初始状态：已排序段[5]，未排序段[3, 4, 1]
            // 第一轮：选取关键元素3，与已排序段的5比较，3小于5，因此5向后移动一位，3插入到它的位置。结果是[3, 5], 未排序段[4, 1]。
            // 第二轮：选取关键元素4，与已排序段的5比较，由于4小于5，5向后移动，然后4与3比较，4大于3，因此4插入到3和5之间。结果是[3, 4, 5]，未排序段[1]。
            // 第三轮：选取关键元素1，它需要与已排序段中的所有元素比较，最终1被插入到最前面。最终结果是[1, 3, 4, 5]。
            // 通过这个过程，插入排序算法逐渐构建了一个完全排序的序列。插入排序特别适合于数据集合较小，或者数据已经部分排序的情况
        }

        /// <summary> MergeSort </summary>
        public static void MergeSort(int[] arr, int left, int right) // 主排序方法
        {
            if (left < right)
            {
                // 找到中间的索引
                var middle = (left + right) / 2;

                // 对左半部分递归排序
                MergeSort(arr, left, middle);
                // 对右半部分递归排序
                MergeSort(arr, middle + 1, right);

                // 合并两个排序好的部分
                Merge(arr, left, middle, right);
            }

            // 归并排序是一种高效稳定的排序算法，尤其在以下几种情况下表现出较其他排序算法更高的效率：
            // 大数据集：对于大规模数据集，归并排序的性能表现通常优于诸如冒泡排序或插入排序的简单排序算法。这是因为归并排序的时间复杂度为O(n log n)，而简单排序算法往往是O(n^2)。当数据量大的时候，这种差距会变得非常明显。
            // 链表排序：归并排序在链表上的排序也非常高效。这是因为它不依赖于随机访问特性，与在数组中使用时相比，链表上的归并排序不需要额外的空间来创建数组副本，只需要重新链接节点即可。因此，在链表排序上，归并排序比如快速排序这类需要随机访问的排序算法性能要好。
            // 外部排序：当数据集太大以至于无法完全装入内存时，通常会使用外部排序。归并排序特别适合于外部排序算法，因为它能有效地将多个已排序的小文件合并成一个有序的大文件。这个过程中的分割和合并操作对于磁盘I/O来说是非常高效的。
            // 稳定性要求：在排序的过程中，如果两个元素相等，归并排序能保持它们原有的先后顺序，这意味着归并排序是稳定的。当需要保持数据中的原始顺序（例如，按姓名字母顺序排序同时保持相同姓名按照年龄排序）时，稳定性就变得很重要，归并排序在这方面比如快速排序等非稳定排序算法更有优势。
            // 并行处理：归并排序比较适合并行计算。它的分治原理允许多个处理器同时工作在不同的数据集上，然后再将结果归并起来。这种特性使得归并排序在多处理器系统中能够高效地处理大量数据。
            // 尽管归并排序在这些方面有较好的表现，但它也存在一些缺点，例如在排序小数据集时可能不如插入排序等算法高效，且因为需要额外的存储空间来存放临时数组而在空间复杂度上不如一些原地排序算法。因此，选择合适的排序算法需要根据具体的应用场景和数据特性进行。
        }

        /// <summary> MergeSort </summary>
        public static void Merge(int[] arr, int left, int middle, int right) // 合并两个子数组的方法
        {
            var n1 = middle - left + 1;
            var n2 = right         - middle;

            // 创建临时数组
            var leftArray  = new int[n1];
            var rightArray = new int[n2];
            int i, j;

            // 拷贝数据到临时数组
            for (i = 0 ; i < n1 ; ++i)
            {
                leftArray[i] = arr[left + i];
            }

            for (j = 0 ; j < n2 ; ++j)
            {
                rightArray[j] = arr[middle + 1 + j];
            }

            // 合并临时数组
            i = 0;
            j = 0;
            var k = left;
            while (i < n1 && j < n2)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    arr[k] = leftArray[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArray[j];
                    j++;
                }

                k++;
            }

            // 拷贝剩余的元素
            while (i < n1)
            {
                arr[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                arr[k] = rightArray[j];
                j++;
                k++;
            }
        }
    }


    public class Permute
    {
        /// <summary> All Permute </summary>
        public IList<IList<int>> AllPermute(int[] nums)
        {
            var result = new List<IList<int>>();
            DFS(0);


            return result;

            void DFS(int depth) // 经典回溯排列
                // https://leetcode.cn/problems/permutations/solutions/218275/quan-pai-lie-by-leetcode-solution-2/
            {
                if (depth == nums.Length)
                {
                    result.Add(nums.ToList());
                    return;
                }

                for (var i = depth ; i < nums.Length ; i++)
                {
                    (nums[depth], nums[i]) = (nums[i], nums[depth]);
                    DFS(depth + 1);
                    (nums[depth], nums[i]) = (nums[i], nums[depth]);
                }
            }
        }
    }

}