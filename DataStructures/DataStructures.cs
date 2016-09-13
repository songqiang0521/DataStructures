using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace ADS_Library
{
    #region LinkedList

    public class ADS_LinkedListNode<T>
    {
        /// <summary>
        /// Constructs a new node with the specified value.
        /// </summary>
        public ADS_LinkedListNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// The node value
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        /// The next node in the linked list (null if last node)
        /// </summary>
        public ADS_LinkedListNode<T> Next { get; internal set; }

        /// <summary>
        /// The previous node in the linked list (null if first node)
        /// </summary>
        public ADS_LinkedListNode<T> Previous { get; internal set; }
    }

    public class ADS_LinkedList<T> : System.Collections.Generic.ICollection<T>
    {
        private ADS_LinkedListNode<T> _head;
        private ADS_LinkedListNode<T> _tail;

        public ADS_LinkedListNode<T> Head
        {
            get
            {
                return _head;
            }
        }

        public ADS_LinkedListNode<T> Tail
        {
            get
            {
                return _tail;
            }
        }

        public void AddFirst(T value)
        {
            ADS_LinkedListNode<T> node = new ADS_LinkedListNode<T>(value);

            // Save off the head node so we don't lose it
            ADS_LinkedListNode<T> temp = _head;

            // Point head to the new node
            _head = node;

            // Insert the rest of the list behind head
            _head.Next = temp;

            if (Count == 0)
            {
                // if the list was empty then head  and tail should
                // both point to the new node.
                _tail = _head;
            }
            else
            {
                // Before: head -------> 5 <-> 7 -> null
                // After:  head  -> 3 <-> 5 <-> 7 -> null
                temp.Previous = _head;
            }
            Count++;
        }

        public void AddLast(T value)
        {
            ADS_LinkedListNode<T> node = new ADS_LinkedListNode<T>(value);

            if (Count == 0)
            {
                _head = node;
            }
            else
            {
                _tail.Next = node;

                // Before: Head -> 3 <-> 5 -> null
                // After:  Head -> 3 <-> 5 <-> 7 -> null
                // 7.Previous = 5
                node.Previous = _tail;
            }
            _tail = node;
            Count++;
        }

        public void Add(T value)
        {
            AddLast(value);
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            ADS_LinkedListNode<T> current = _head;
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ADS_LinkedListNode<T> current = _head;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }

        public int Count
        {
            get;
            private set;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void RemoveFirst()
        {
            if (Count != 0)
            {
                // Before: Head -> 3 <-> 5
                // After:  Head -------> 5

                // Head -> 3 -> null
                // Head ------> null
                _head = _head.Next;

                Count--;

                if (Count == 0)
                {
                    _tail = null;
                }
                else
                {
                    // 5.Previous was 3, now null
                    _head.Previous = null;
                }
            }
        }

        public void RemoveLast()
        {
            if (Count != 0)
            {
                if (Count == 1)
                {
                    _head = null;
                    _tail = null;
                }
                else
                {
                    // Before: Head --> 3 --> 5 --> 7
                    //         Tail = 7
                    // After:  Head --> 3 --> 5 --> null
                    //         Tail = 5
                    // Null out 5's Next pointerproperty
                    _tail.Previous.Next = null;
                    _tail = _tail.Previous;
                }
                Count--;
            }
        }

        public bool Remove(T item)
        {
            ADS_LinkedListNode<T> previous = null;
            ADS_LinkedListNode<T> current = _head;

            // 1: Empty list - do nothing
            // 2: Single node: (previous is null)
            // 3: Many nodes
            //    a: node to remove is the first node
            //    b: node to remove is the middle or last

            while (current != null)
            {
                // Head -> 3 -> 5 -> 7 -> null
                // Head -> 3 ------> 7 -> null
                if (current.Value.Equals(item))
                {
                    // it's a node in the middle or end
                    if (previous != null)
                    {
                        // Case 3b
                        previous.Next = current.Next;

                        // it was the end - so update Tail
                        if (current.Next == null)
                        {
                            _tail = previous;
                        }
                        else
                        {
                            // Before: Head -> 3 <-> 5 <-> 7 -> null
                            // After:  Head -> 3 <-------> 7 -> null

                            // previous = 3
                            // current = 5
                            // current.Next = 7
                            // So... 7.Previous = 3
                            current.Next.Previous = previous;
                        }
                        Count--;
                    }
                    else
                    {
                        // Case 2 or 3a
                        RemoveFirst();
                    }
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            return false;
        }


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            ADS_LinkedListNode<T> current = _head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }

    #endregion

    #region ArrayList<T>

    public class ArrayList<T> : System.Collections.Generic.IList<T>
    {
        T[] _items;

        private void GrowArray()
        {
            int newLength = _items.Length == 0 ? 16 : _items.Length << 1;

            T[] newArray = new T[newLength];

            _items.CopyTo(newArray, 0);

            _items = newArray;
        }

        public ArrayList()
            : this(0)
        {
        }

        public ArrayList(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("length");
            }
            _items = new T[length];
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (_items.Length == this.Count)
            {
                this.GrowArray();
            }

            // shift all the items following index one to the right
            Array.Copy(_items, index, _items, index + 1, Count - index);

            _items[index] = item;
            Count++;
        }


        public void RemoveAt(int index)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            int shiftStart = index + 1;
            if (shiftStart < Count)
            {
                // shift all the items following index one to the left
                Array.Copy(_items, shiftStart, _items, index, Count - shiftStart);
            }
            Count--;
        }

        public T this[int index]
        {
            get
            {
                if (index < Count)
                {
                    return _items[index];
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < Count)
                {
                    _items[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public void Add(T item)
        {
            if (_items.Length == Count)
            {
                GrowArray();
            }
            _items[Count++] = item;
        }

        public void Clear()
        {
            _items = new T[0];
            Count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) > -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, Count);
        }

        public int Count
        {
            get;
            private set;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _items[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    #endregion

    #region Stack
    public class Stack<T>
    {
        ADS_LinkedList<T> _items = new ADS_LinkedList<T>();

        public void Push(T value)
        {
            _items.AddLast(value);
        }

        public T Pop()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty");
            }

            T result = _items.Tail.Value;

            _items.RemoveLast();

            return result;
        }

        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty");
            }

            return _items.Tail.Value;
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }
    }

    #endregion

    #region Queue
    public class Queue<T>
    {
        ADS_LinkedList<T> _items = new ADS_LinkedList<T>();

        public void Enqueue(T value)
        {
            _items.AddFirst(value);
        }

        public T Dequeue()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            T last = _items.Tail.Value;

            _items.RemoveLast();

            return last;
        }

        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            return _items.Tail.Value;
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }
    }

    #endregion

    #region Deque LinkedList based
    //public class Deque<T>
    //{
    //    ADS_LinkedList<T> _items = new ADS_LinkedList<T>();

    //    public void EnqueueFirst(T value)
    //    {
    //        _items.AddFirst(value);
    //    }

    //    public void EnqueueLast(T value)
    //    {
    //        _items.AddLast(value);
    //    }

    //    public T DequeueFirst()
    //    {
    //        if (_items.Count == 0)
    //        {
    //            throw new InvalidOperationException("DequeueFirst called when deque is empty");
    //        }

    //        T temp = _items.Head.Value;
    //        _items.RemoveFirst();
    //        return temp;
    //    }

    //    public T DequeueLast()
    //    {
    //        if (_items.Count == 0)
    //        {
    //            throw new InvalidOperationException("DequeueLast called when deque is empty");
    //        }

    //        T temp = _items.Tail.Value;
    //        _items.RemoveLast();
    //        return temp;
    //    }

    //    public T PeekFirst()
    //    {
    //        if (_items.Count == 0)
    //        {
    //            throw new InvalidOperationException("Dequeue PeekFirst called when deque is empty");
    //        }
    //        return _items.Head.Value;
    //    }

    //    public T PeekLast()
    //    {
    //        if (_items.Count == 0)
    //        {
    //            throw new InvalidOperationException("Dequeue PeekLast called when deque is empty");
    //        }
    //        return _items.Tail.Value;
    //    }

    //    public int Count
    //    {
    //        get
    //        {
    //            return _items.Count;
    //        }
    //    }
    //}

    #endregion

    #region Deque Array based

    public class Deque<T>
    {
        T[] _items = new T[0];

        // the number of items in the queue
        int _size = 0;

        // the index of the first (oldest) item in the queue
        int _head = 0;

        // the index of the last (newest) item in the queue
        int _tail = -1;

        private void allocateNewArray(int startingIndex)
        {
            int newLength = (_size == 0) ? 4 : _size * 2;

            T[] newArray = new T[newLength];

            if (_size > 0)
            {
                int targetIndex = startingIndex;

                // copy contents...
                // if the array has no wrapping, just copy the valid range
                // else copy from head to end of the array and then from 0 to the tail

                // if tail is less than head we've wrapped
                if (_tail < _head)
                {
                    // copy the _items[head].._items[end] -> newArray[0]..newArray[N]
                    for (int index = _head; index < _items.Length; index++)
                    {
                        newArray[targetIndex] = _items[index];
                        targetIndex++;
                    }

                    // copy _items[0].._items[tail] -> newArray[N+1]..
                    for (int index = 0; index <= _tail; index++)
                    {
                        newArray[targetIndex] = _items[index];
                        targetIndex++;
                    }
                }
                else
                {
                    // copy the _items[head].._items[tail] -> newArray[0]..newArray[N]
                    for (int index = _head; index <= _tail; index++)
                    {
                        newArray[targetIndex] = _items[index];
                        targetIndex++;
                    }
                }
                _head = startingIndex;
                _tail = targetIndex - 1; // compensate for the extra bump
            }
            else
            {
                // nothing in the array
                _head = 0;
                _tail = -1;
            }
            _items = newArray;
        }

        public void EnqueueFirst(T item)
        {
            // if the array needs to grow
            if (_items.Length == _size)
            {
                allocateNewArray(1);
            }

            // since we know the array isn't full and _head is greater than 0
            // we know the slot in front of head is open
            if (_head > 0)
            {
                _head--;
            }
            else
            {
                // otherwise we need to wrap around to the end of the array
                _head = _items.Length - 1;
            }
            _items[_head] = item;
            _size++;
        }

        public void EnqueueLast(T item)
        {
            // if the array needs to grow
            if (_items.Length == _size)
            {
                allocateNewArray(0);
            }

            // now we have a properly sized array and can focus on wrapping issues.
            // if _tail is at the end of the array we need to wrap around
            if (_tail == _items.Length - 1)
            {
                _tail = 0;
            }
            else
            {
                _tail++;
            }
            _items[_tail] = item;
            _size++;
        }

        public T DequeueFirst()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("The deque is empty");
            }

            T value = _items[_head];

            if (_head == _items.Length - 1)
            {
                // if the head is at the last index in the array - wrap around.
                _head = 0;
            }
            else
            {
                // move to the next slot
                _head++;
            }
            _size--;
            return value;
        }

        public T DequeueLast()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("The deque is empty");
            }

            T value = _items[_tail];

            if (_tail == 0)
            {
                // if the tai is at the first index in the array - wrap around.
                _tail = _items.Length - 1;
            }
            else
            {
                // move to the previous slot
                _tail--;
            }
            _size--;
            return value;
        }

        public T PeekFirst()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("The deque is empty");
            }
            return _items[_head];
        }

        public T PeekLast()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("The deque is empty");
            }
            return _items[_tail];
        }

        public int Count
        {
            get
            {
                return _size;
            }
        }
    }

    #endregion

    #region Binary Tree

    class BinaryTreeNode<TNode> : IComparable<TNode> where TNode : IComparable<TNode>
    {
        public BinaryTreeNode(TNode value)
        {
            Value = value;
        }

        public BinaryTreeNode<TNode> Left { get; set; }
        public BinaryTreeNode<TNode> Right { get; set; }
        public TNode Value { get; private set; }

        /// <summary>
        /// Compares the current node to the provided value
        /// </summary>
        /// <param name="other">The node value to compare to</param>
        /// <returns>1 if the instance value is greater than 
        /// the provided value, -1 if less or 0 if equal.</returns>
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }
    }

    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private BinaryTreeNode<T> _head;
        private int _count;

        public void Add(T value)
        {
            // Case 1: The tree is empty - allocate the head
            if (_head == null)
            {
                _head = new BinaryTreeNode<T>(value);
                _count++;
            }
            // Case 2: The tree is not empty so recursively 
            // find the right location to insert
            else
            {
                AddTo(_head, value);
            }

        }

        // Recursive add algorithm
        private void AddTo(BinaryTreeNode<T> node, T value)
        {
            // Case 1: Value is less than the current node value
            if (value.CompareTo(node.Value) < 0)
            {
                // if there is no left child make this the new left
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(value);
                    _count++;
                }
                else
                {
                    // else add it to the left node
                    AddTo(node.Left, value);
                }
            }
            // Case 2: Value is equal to or greater than the current value
            else
            {
                // If there is no right, add it to the right
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(value);
                    _count++;
                }
                else
                {
                    // else add it to the right node
                    AddTo(node.Right, value);
                }
            }
        }

        public bool Contains(T value)
        {
            // defer to the node search helper function.
            BinaryTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }

        /// <summary>
        /// Finds and returns the first node containing the specified value.  If the value
        /// is not found, returns null.  Also returns the parent of the found node (or null)
        /// which is used in Remove.
        /// </summary>
        private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
        {
            // Now, try to find data in the tree
            BinaryTreeNode<T> current = _head;
            parent = null;

            // while we don't have a match
            while (current != null)
            {
                int result = current.CompareTo(value);

                if (result > 0)
                {
                    // if the value is less than current, go left.
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // if the value is more than current, go right.
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    // we have a match!
                    break;
                }
            }
            return current;
        }

        public bool Remove(T value)
        {
            BinaryTreeNode<T> current, parent;

            // Find the node to remove
            current = FindWithParent(value, out parent);

            if (current == null)
            {
                return false;
            }
            _count--;

            // Case 1: If current has no right child, then current's left replaces current
            //1、当前节点无右子节点,当前节点的左子节点替换当前节点
            if (current.Right == null)
            {
                if (parent == null)
                {
                    _head = current.Left;
                }
                else
                {
                    //判断要删除的current是parent的左子节点还是右子节点
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // if parent value is greater than current value
                        // make the current left child a left child of parent
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // if parent value is less than current value
                        // make the current left child a right child of parent
                        parent.Right = current.Left;
                    }
                }
            }
            // Case 2: If current's right child has no left child, then current's right child
            //         replaces current
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    _head = current.Right;
                }
                else
                {
                    //判断要删除的current是parent的左子节点还是右子节点
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // if parent value is greater than current value
                        // make the current right child a left child of parent
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // if parent value is less than current value
                        // make the current right child a right child of parent
                        parent.Right = current.Right;
                    }
                }
            }
            // Case 3: If current's right child has a left child, replace current with current's
            //         right child's left-most child
            //3、当前节点有右子结点，而右子节点无右子节点，稍微复杂些
            else
            {
                // find the right's left-most child
                BinaryTreeNode<T> leftmost = current.Right.Left;
                BinaryTreeNode<T> leftmostParent = current.Right;

                //一直找，直到找到最左的节点，而这个节点正好比current节点的左子节点大
                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // the parent's left subtree becomes the leftmost's right subtree
                leftmostParent.Left = leftmost.Right;

                // assign leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                {
                    _head = leftmost;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // if parent value is greater than current value
                        // make leftmost the parent's left child
                        parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // if parent value is less than current value
                        // make leftmost the parent's right child
                        parent.Right = leftmost;
                    }
                }
            }
            return true;
        }

        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, _head);
        }

        private void PreOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                action(node.Value);
                PreOrderTraversal(action, node.Left);
                PreOrderTraversal(action, node.Right);
            }
        }

        public void PostOrderTraversal(Action<T> action)
        {
            PostOrderTraversal(action, _head);
        }

        private void PostOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {

            //if (root.Left!=null)
            //{
            //    PostOrderTranversal(action, root.Left);
            //}

            //先进去再说，再判断是不是null，该不该遍历。
            if (node != null)
            {
                PostOrderTraversal(action, node.Left);
                PostOrderTraversal(action, node.Right);
                action(node.Value);
            }
        }

        public void InOrderTraversal(Action<T> action)
        {
            InOrderTraversal(action, _head);
        }

        private void InOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                InOrderTraversal(action, node.Left);
                action(node.Value);
                InOrderTraversal(action, node.Right);
            }
        }

        public IEnumerator<T> InOrderTraversal()
        {
            // This is a non-recursive algorithm using a stack to demonstrate removing
            // recursion.
            if (_head != null)
            {
                // store the nodes we've skipped in this stack (avoids recursion)
                Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();

                BinaryTreeNode<T> current = _head;

                // when removing recursion we need to keep track of whether or not
                // we should be going to the left node or the right nodes next.
                bool goLeftNext = true;

                // start by pushing Head onto the stack
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // If we're heading left...
                    if (goLeftNext)
                    {
                        // push everything but the left-most node to the stack
                        // we'll yield the left-most after this block
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // in-order is left->yield->right
                    yield return current.Value;

                    // if we can go right then do so
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // once we've gone right once, we need to start
                        // going left again.
                        goLeftNext = true;
                    }
                    else
                    {
                        // if we can't go right then we need to pop off the parent node
                        // so we can process it and then go to it's right node
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }
    }

    #endregion

    #region Set

    public class Set<T> : IEnumerable<T> where T : IComparable<T>
    {
        private readonly List<T> _items = new List<T>();

        public Set()
        {
        }

        public Set(IEnumerable<T> items)
        {
            AddRange(items);
        }

        public void Add(T item)
        {
            if (Contains(item))
            {
                throw new InvalidOperationException("Item already exists in Set");
            }
            _items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public Set<T> Union(Set<T> other)
        {
            Set<T> result = new Set<T>(_items);

            foreach (T item in other._items)
            {
                if (!Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public Set<T> Intersection(Set<T> other)
        {
            Set<T> result = new Set<T>();

            foreach (T item in _items)
            {
                if (other._items.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public Set<T> Difference(Set<T> other)
        {
            Set<T> result = new Set<T>(_items);

            foreach (T item in other._items)
            {
                result.Remove(item);
            }
            return result;
        }

        public Set<T> SymmetricDifference(Set<T> other)
        {
            Set<T> union = Union(other);
            Set<T> intersection = Intersection(other);

            return union.Difference(intersection);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    #endregion

    #region Sorting

    public class SortingSupport<T> where T : IComparable<T>
    {
        void Swap(T[] items, int left, int right)
        {
            if (left != right)
            {
                T temp = items[left];
                items[left] = items[right];
                items[right] = temp;
            }
        }

        #region Bubble Sort

        public void BubbleSort(T[] items)
        {
            bool swapped;

            do
            {
                swapped = false;
                for (int i = 1; i < items.Length; i++)
                {
                    if (items[i - 1].CompareTo(items[i]) > 0)
                    {
                        Swap(items, i - 1, i);
                        swapped = true;
                    }
                }
                //TIME 2013年6月13日20:20:55
                //当循环完一遍后,发现没有交换元素.
                //说明数据已经排好序了
            } while (swapped != false);
        }
        #endregion

        #region Insertion Sort

        public void InsertionSort(T[] items)
        {
            int sortedRangeEndIndex = 1;

            while (sortedRangeEndIndex < items.Length)
            {
                if (items[sortedRangeEndIndex].CompareTo(items[sortedRangeEndIndex - 1]) < 0)
                {
                    int insertIndex = FindInsertionIndex(items, items[sortedRangeEndIndex]);
                    Insert(items, insertIndex, sortedRangeEndIndex);
                }

                sortedRangeEndIndex++;
            }
        }

        private int FindInsertionIndex(T[] items, T valueToInsert)
        {
            for (int index = 0; index < items.Length; index++)
            {
                if (items[index].CompareTo(valueToInsert) > 0)
                {
                    return index;
                }
            }

            throw new InvalidOperationException("The insertion index was not found");
        }

        private void Insert(T[] itemArray, int indexInsertingAt, int indexInsertingFrom)
        {
            // itemArray =       0 1 2 4 5 6 3 7
            // insertingAt =     3
            // insertingFrom =   6
            // actions
            //  1: store index at in temp     temp = 4
            //  2: set index at to index from  -> 0 1 2 3 5 6 3 7   temp = 4
            //  3: walking backwards from index from to index at + 1
            //     shift values from left to right once
            //     0 1 2 3 5 6 6 7   temp = 4
            //     0 1 2 3 5 5 6 7   temp = 4
            //  4: write temp value to index at + 1
            //     0 1 2 3 4 5 6 7   temp = 4

            // Step 1
            T temp = itemArray[indexInsertingAt];

            // Step 2

            itemArray[indexInsertingAt] = itemArray[indexInsertingFrom];

            // Step 3
            for (int current = indexInsertingFrom; current > indexInsertingAt; current--)
            {
                itemArray[current] = itemArray[current - 1];
            }

            // Step 4
            itemArray[indexInsertingAt + 1] = temp;
        }

        #endregion

        #region Selection Sort

        public void SelectionSort(T[] items)
        {
            int sortedRangeEnd = 0;

            while (sortedRangeEnd < items.Length)
            {
                int nextIndex = FindIndexOfSmallestFromIndex(items, sortedRangeEnd);
                Swap(items, sortedRangeEnd, nextIndex);

                sortedRangeEnd++;
            }
        }

        private int FindIndexOfSmallestFromIndex(T[] items, int sortedRangeEnd)
        {
            T currentSmallest = items[sortedRangeEnd];
            int currentSmallestIndex = sortedRangeEnd;

            for (int i = sortedRangeEnd + 1; i < items.Length; i++)
            {
                if (currentSmallest.CompareTo(items[i]) > 0)
                {
                    currentSmallest = items[i];
                    currentSmallestIndex = i;
                }
            }

            return currentSmallestIndex;
        }
        #endregion

        #region Merge Sort

        public void MergeSort(T[] items)
        {
            if (items.Length <= 1)
            {
                return;
            }

            int leftSize = items.Length / 2;
            int rightSize = items.Length - leftSize;

            T[] left = new T[leftSize];
            T[] right = new T[rightSize];

            Array.Copy(items, 0, left, 0, leftSize);
            Array.Copy(items, leftSize, right, 0, rightSize);

            MergeSort(left);
            MergeSort(right);
            Merge(items, left, right);
        }

        private void Merge(T[] items, T[] left, T[] right)
        {
            int leftIndex = 0;
            int rightIndex = 0;
            int targetIndex = 0;

            int remaining = left.Length + right.Length;

            while (remaining > 0)
            {
                if (leftIndex >= left.Length)
                {
                    items[targetIndex] = right[rightIndex++];
                }
                else if (rightIndex >= right.Length)
                {
                    items[targetIndex] = left[leftIndex++];
                }
                else if (left[leftIndex].CompareTo(right[rightIndex]) < 0)
                {
                    items[targetIndex] = left[leftIndex++];
                }
                else
                {
                    items[targetIndex] = right[rightIndex++];
                }
                targetIndex++;
                remaining--;
            }
        }

        #endregion

        #region Quick Sort

        Random _pivotRng = new Random();

        public void QuickSort(T[] items)
        {
            quicksort(items, 0, items.Length - 1);
        }

        private void quicksort(T[] items, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = _pivotRng.Next(left, right);
                int newPivot = partition(items, left, right, pivotIndex);

                quicksort(items, left, newPivot - 1);
                quicksort(items, newPivot + 1, right);
            }
        }

        private int partition(T[] items, int left, int right, int pivotIndex)
        {
            T pivotValue = items[pivotIndex];

            Swap(items, pivotIndex, right);

            int storeIndex = left;

            for (int i = left; i < right; i++)
            {
                if (items[i].CompareTo(pivotValue) < 0)
                {
                    Swap(items, i, storeIndex);
                    storeIndex += 1;
                }
            }

            Swap(items, storeIndex, right);
            return storeIndex;
        }

        #endregion
    }

    #endregion

    #region Skip List

    internal class SkipListNode<T>
    {
        /// <summary>
        /// Creates a new node with the specified value
        /// at the indicated link height.
        /// </summary>
        public SkipListNode(T value, int height)
        {
            Value = value;
            Next = new SkipListNode<T>[height];
        }

        /// <summary>
        /// The array of links. The number of items
        /// is the height of the links.
        /// </summary>
        public SkipListNode<T>[] Next
        {
            get;
            private set;
        }

        /// <summary>
        /// The contained value.
        /// </summary>
        public T Value
        {
            get;
            private set;
        }
    }

    public class SkipList<T> : ICollection<T>
    where T : IComparable<T>
    {
        // used to determine the random height of the node links
        private readonly Random _rand = new Random();

        // the non-data node which starts the list
        private SkipListNode<T> _head;

        // there is always one level of depth (the base list)
        private int _levels = 1;

        // the number of items currently in the list
        private int _count = 0;

        public SkipList()
        {
        }

        public void Add(T item)
        {
            int level = PickRandomLevel();
            SkipListNode<T> newNode = new SkipListNode<T>(item, level + 1);
            SkipListNode<T> current = _head;

            // walk down each level in the list (make big jumps)
            for (int i = _levels - 1; i >= 0; i--)
            {
                while (current.Next[i] != null)
                {
                    if (current.Next[i].Value.CompareTo(item) > 0)
                    {
                        // 应该降级了
                        // TIME 2013年6月14日14:53:22
                        break;
                    }
                    current = current.Next[i];
                }
                if (i <= level)
                {
                    // adding "c" to the list: a -> b -> d -> e
                    // current is node b and current.Next[i] is d.

                    // 1. Link the new node (c) to the existing node (d)
                    // c.Next = d
                    newNode.Next[i] = current.Next[i];

                    // Insert c into the list after b
                    // b.Next = c
                    current.Next[i] = newNode;
                }
            }
            _count++;
        }

        private int PickRandomLevel()
        {
            int rand = _rand.Next();
            int level = 0;
            // we're using the bit mask of a random integer to determine if the max 
            // level should bump up one or not. 
            // Say the 8 LSB of the int are 00101100. In that case when the 
            // LSB is compared against 1 it tests to 0 and the while loop is never 
            // entered so the level stays the same. That should happen 1/2 of the time.
            // Later if the _levels field is set to 3 and the rand value is 01101111,
            // this time the while loop will run 4 times and on the last iteration will
            // run 4 times creating a node with a skip list height of 4. This should 
            // only happen 1/16 of the time.
            while ((rand & 1) == 1)
            {
                if (level == _levels)
                {
                    _levels++;
                    break;
                }
                rand >>= 1;
                level++;
            }
            return level;
        }

        public bool Contains(T item)
        {
            SkipListNode<T> cur = _head;

            // walk down each level in the list (make big jumps)
            for (int i = _levels - 1; i >= 0; i--)
            {
                while (cur.Next[i] != null)
                {
                    int cmp = cur.Next[i].Value.CompareTo(item);
                    if (cmp > 0)
                    {
                        // the value is too large so go down one level
                        // and take smaller steps
                        break;
                    }
                    if (cmp == 0)
                    {
                        // found it!
                        return true;
                    }

                    // cur节点只可能向后推进
                    // TIME 2013年6月14日15:01:28
                    cur = cur.Next[i];
                }
            }
            return false;
        }

        public bool Remove(T item)
        {
            SkipListNode<T> cur = _head;
            bool removed = false;
            // walk down each level in the list (make big jumps)
            for (int level = _levels - 1; level >= 0; level--)
            {
                // while we're not at the end of the list
                while (cur.Next[level] != null)
                {
                    // if we found our node
                    if (cur.Next[level].Value.CompareTo(item) == 0)
                    {
                        // remove the node
                        cur.Next[level] = cur.Next[level].Next[level];
                        removed = true;

                        // and go down to the next level (where
                        // we will find our node again if we're
                        // not at the bottom level)
                        break;
                    }
                    // if we went too far then go down a level
                    if (cur.Next[level].Value.CompareTo(item) > 0)
                    {
                        break;
                    }
                    cur = cur.Next[level];
                }
            }
            if (removed)
            {
                _count--;
            }
            return removed;
        }

        public void Clear()
        {
            _head = new SkipListNode<T>(default(T), 32 + 1);
            _count = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            int offset = 0;
            foreach (T item in this)
            {
                array[arrayIndex + offset++] = item;
            }
        }

        public int Count
        {
            get { return _count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            //
            // 对第一层索引是[0]进行遍历就可以了
            // TIME 2013年6月14日14:58:31
            //
            SkipListNode<T> cur = _head.Next[0];
            while (cur != null)
            {
                yield return cur.Value;
                cur = cur.Next[0];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    #endregion

    #region Hashing Functions

    internal class HashingStuff
    {
        // Hashing function first reported by Dan Bernstein 
        // http://www.cse.yorku.ca/~oz/hash.html 
        private static int Djb2(string input)
        {
            int hash = 5381;
            foreach (int c in input.ToCharArray())
            {
                unchecked
                {
                    /* hash * 33 + c */
                    hash = ((hash << 5) + hash) + c;
                }
            }
            return hash;
        }

        // Treats each four characters as an integer so
        // "aaaabbbb" hashes differently than "bbbbaaaa"
        private static int FoldingHash(string input)
        {
            int hashValue = 0;
            int startIndex = 0;
            int currentFourBytes;
            do
            {
                currentFourBytes = GetNextBytes(startIndex, input);
                unchecked
                {
                    hashValue += currentFourBytes;
                }
                startIndex += 4;
            } while (currentFourBytes != 0);
            return hashValue;
        }

        // Gets the next four bytes of the string converted to an
        // integer - If there are not enough characters, 0 is used.
        private static int GetNextBytes(int startIndex, string str)
        {
            int currentFourBytes = 0;
            currentFourBytes += GetByte(str, startIndex);
            currentFourBytes += GetByte(str, startIndex + 1) << 8;
            currentFourBytes += GetByte(str, startIndex + 2) << 16;
            currentFourBytes += GetByte(str, startIndex + 3) << 24;
            return currentFourBytes;
        }

        private static int GetByte(string str, int index)
        {
            if (index < str.Length)
            {
                return (int)str[index];
            }
            return 0;
        }
    }
    #endregion

    #region HashTable 
    //2013年4月20日11:29:11
    //2013年6月14日15:51:04
    internal class HashTableNodePair<TKey, TValue>
    {
        public TKey Key { get; internal set; }
        public TValue Value { get; internal set; }

        public HashTableNodePair() { }
        public HashTableNodePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    //这其实就是一个链表
    internal class HashTableArrayNode<TKey, TValue>
    {
        // This list contains the actual data in the hash table. It chains together
        // data collisions.
        ADS_LinkedList<HashTableNodePair<TKey, TValue>> _items;

        /// <summary>
        /// Adds the key/value pair to the node. If the key already exists in the
        /// list an ArgumentException will be thrown
        /// </summary>
        /// <param name="key">The key of the item being added</param>
        /// <param name="value">The value of the item being added</param>
        public void Add(TKey key, TValue value)
        {
            // lazy init the linked list
            if (_items == null)
            {
                _items = new ADS_LinkedList<HashTableNodePair<TKey, TValue>>();
            }
            else
            {
                // Multiple items might collide and exist in this list - but each
                // key should only be in the list once.
                //如果不同的键有相同的hash，说明发生了碰撞
                //但是这个位置里面的键还是不能相同，因为这不符合业务逻辑
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        throw new ArgumentException("The collection already contains the key");
                    }
                }
            }
            // if we made it this far - add the item
            _items.AddFirst(new HashTableNodePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Updates the value of the existing key/value pair in the list.
        /// If the key does not exist in the list an ArgumentException
        /// will be thrown.
        /// </summary>
        /// <param name="key">The key of the item being updated</param>
        /// <param name="value">The updated value</param>
        public void Update(TKey key, TValue value)
        {
            bool updated = false;
            if (_items != null)
            {
                // check each item in the list for the specified key 
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        // update the value
                        pair.Value = value;
                        updated = true;
                        break;
                    }
                }
            }
            if (!updated)
            {
                throw new ArgumentException("The collection does not contain the key");
            }
        }

        /// <summary>
        /// Finds and returns the value for the specified key.
        /// </summary>
        /// <param name="key">The key whose value is sought</param>
        /// <param name="value">The value associated with the specified key</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            bool found = false;
            if (_items != null)
            {
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// Removes the item from the list whose key matches
        /// the specified key.
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>True if the item was removed, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            bool removed = false;
            if (_items != null)
            {
                ADS_LinkedListNode<HashTableNodePair<TKey, TValue>> current = _items.Head;
                while (current != null)
                {
                    if (current.Value.Key.Equals(key))
                    {
                        //TODO
                        //_items.Remove(current);
                        removed = true;
                        break;
                    }
                    current = current.Next;
                }
            }
            return removed;
        }

        /// <summary>
        /// Removes all the items from the list
        /// </summary>
        public void Clear()
        {
            if (_items != null)
            {
                _items.Clear();
            }
        }

        /// <summary>
        /// Returns an enumerator for all of the values in the list
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for all of the keys in the list
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node.Key;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for all the key/value pairs in the list
        /// </summary>
        public IEnumerable<HashTableNodePair<TKey, TValue>> Items
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node;
                    }
                }
            }
        }
    }

    class HashTableArray<TKey, TValue>
    {
        HashTableArrayNode<TKey, TValue>[] _array;

        /// <summary>
        /// Constructs a new hash table array with the specified capacity
        /// </summary>
        /// <param name="capacity">The capacity of the array</param>
        public HashTableArray(int capacity)
        {
            _array = new HashTableArrayNode<TKey, TValue>[capacity];
        }

        /// <summary>
        /// Adds the key/value pair to the node. If the key already exists in the
        /// node array an ArgumentException will be thrown
        /// </summary>
        /// <param name="key">The key of the item being added</param>
        /// <param name="value">The value of the item being added</param>
        public void Add(TKey key, TValue value)
        {
            int index = GetIndex(key);
            HashTableArrayNode<TKey, TValue> nodes = _array[index];
            if (nodes == null)
            {
                nodes = new HashTableArrayNode<TKey, TValue>();
                _array[index] = nodes;
            }
            nodes.Add(key, value);
        }

        /// <summary>
        /// Updates the value of the existing key/value pair in the node array.
        /// If the key does not exist in the array an ArgumentException
        /// will be thrown.
        /// </summary>
        /// <param name="key">The key of the item being updated</param>
        /// <param name="value">The updated value</param>
        public void Update(TKey key, TValue value)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[GetIndex(key)];
            if (nodes == null)
            {
                throw new ArgumentException("The key does not exist in the hash table", "key");
            }
            nodes.Update(key, value);
        }

        /// <summary>
        /// Removes the item from the node array whose key matches
        /// the specified key.
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>True if the item was removed, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[GetIndex(key)];
            if (nodes != null)
            {
                return nodes.Remove(key);
            }
            return false;
        }

        /// <summary>
        /// Finds and returns the value for the specified key.
        /// </summary>
        /// <param name="key">The key whose value is sought</param>
        /// <param name="value">The value associated with the specified key</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[GetIndex(key)];
            if (nodes != null)
            {
                return nodes.TryGetValue(key, out value);
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// The capacity of the hash table array
        /// </summary>
        public int Capacity
        {
            get
            {
                return _array.Length;
            }
        }

        /// <summary>
        /// Removes every item from the hash table array
        /// </summary>
        public void Clear()
        {
            foreach (HashTableArrayNode<TKey, TValue> node in _array.Where(node => node != null))
            {
                node.Clear();
            }
        }

        /// <summary>
        /// Returns an enumerator for all of the values in the node array
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in
                        _array.Where(node => node != null))
                {
                    foreach (TValue value in node.Values)
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for all of the keys in the node array
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in
                        _array.Where(node => node != null))
                {
                    foreach (TKey key in node.Keys)
                    {
                        yield return key;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for all of the Items in the node array
        /// </summary>
        public IEnumerable<HashTableNodePair<TKey, TValue>> Items
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in
                        _array.Where(node => node != null))
                {
                    foreach (HashTableNodePair<TKey, TValue> pair in node.Items)
                    {
                        yield return pair;
                    }
                }
            }
        }

        // Maps a key to the array index based on hash code
        private int GetIndex(TKey key)
        {
            return Math.Abs(key.GetHashCode() % Capacity);
        }
    }

    public class HashTable<TKey, TValue>
    {
        // If the array exceeds this fill percentage it will grow
        const double _fillFactor = 0.75;

        // The maximum number of items to store before growing.
        // This is just a cached value of the fill factor calculation
        int _maxItemsAtCurrentSize;

        // the number of items in the hash table
        int _count;

        // The array where the items are stored.
        HashTableArray<TKey, TValue> _array;

        /// <summary>
        /// Constructs a hash table with the default capacity
        /// </summary>
        public HashTable()
            : this(1000)
        {
        }

        /// <summary>
        /// Constructs a hash table with the specified capacity
        /// </summary>
        public HashTable(int initialCapacity)
        {
            if (initialCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("initialCapacity");
            }
            _array = new HashTableArray<TKey, TValue>(initialCapacity);
            // when the count exceeds this value, the next Add will cause the
            // array to grow
            _maxItemsAtCurrentSize = (int)(initialCapacity * _fillFactor) + 1;
        }

        /// <summary>
        /// Adds the key/value pair to the hash table. If the key already exists in the
        /// hash table an ArgumentException will be thrown
        /// </summary>
        /// <param name="key">The key of the item being added</param>
        /// <param name="value">The value of the item being added</param>
        public void Add(TKey key, TValue value)
        {
            // if we are at capacity, the array needs to grow
            if (_count >= _maxItemsAtCurrentSize)
            {
                // allocate a larger array
                HashTableArray<TKey, TValue> largerArray = new HashTableArray<TKey, TValue>(_array.Capacity * 2);
                // and re-add each item to the new array
                //不能直接复制，因为要重新Hash
                //TIME 2013年6月13日20:18:48
                foreach (HashTableNodePair<TKey, TValue> node in _array.Items)
                {
                    largerArray.Add(node.Key, node.Value);
                }
                // the larger array is now the hash table storage
                _array = largerArray;
                // update the new max items cached value
                _maxItemsAtCurrentSize = (int)(_array.Capacity * _fillFactor) + 1;
            }
            _array.Add(key, value);
            _count++;
        }

        /// <summary>
        /// Removes the item from the hash table whose key matches
        /// the specified key.
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>True if the item was removed, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            bool removed = _array.Remove(key);
            if (removed)
            {
                _count--;
            }
            return removed;
        }

        /// <summary>
        /// Gets and sets the value with the specified key. ArgumentException is
        /// thrown if the key does not already exist in the hash table.
        /// </summary>
        /// <param name="key">The key of the value to retrieve</param>
        /// <returns>The value associated with the specified key</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!_array.TryGetValue(key, out value))
                {
                    throw new ArgumentException("key");
                }
                return value;
            }
            set
            {
                _array.Update(key, value);
            }
        }

        /// <summary>
        /// Finds and returns the value for the specified key.
        /// </summary>
        /// <param name="key">The key whose value is sought</param>
        /// <param name="value">The value associated with the specified key</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _array.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns a Boolean indicating if the hash table contains the specified key.
        /// </summary>
        /// <param name="key">The key whose existence is being tested</param>
        /// <returns>True if the value exists in the hash table, false otherwise.</returns>
        public bool ContainsKey(TKey key)
        {
            TValue value;
            return _array.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns a Boolean indicating if the hash table contains the specified value.
        /// </summary>
        /// <param name="value">The value whose existence is being tested</param>
        /// <returns>True if the value exists in the hash table, false otherwise.</returns>
        public bool ContainsValue(TValue value)
        {
            foreach (TValue foundValue in _array.Values)
            {
                if (value.Equals(foundValue))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator for all of the keys in the hash table
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (TKey key in _array.Keys)
                {
                    yield return key;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for all of the values in the hash table
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (TValue value in _array.Values)
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Removes all items from the hash table
        /// </summary>
        public void Clear()
        {
            _array.Clear();
            _count = 0;
        }

        /// <summary>
        /// The number of items currently in the hash table
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
        }
    }

    #endregion

    #region Heap
    public class Heap<T> where T : IComparable<T>
    {
        T[] _items;
        int _count;
        const int DEFAULT_LENGTH = 100;

        public Heap()
            : this(DEFAULT_LENGTH)
        {
        }

        public Heap(int length)
        {
            _items = new T[length];
            _count = 0;
        }

        public void Add(T value)
        {
            if (_count >= _items.Length)
            {
                GrowBackingArray();
            }
            _items[_count] = value;
            int index = _count;

            //
            // TIME 2013年6月14日15:58:35
            // 比较的次数很少。因为用到了树结构。
            //
            while (index > 0 && _items[index].CompareTo(_items[Parent(index)]) > 0)
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
            _count++;
        }

        private void GrowBackingArray()
        {
            T[] newItems = new T[_items.Length * 2];
            for (int i = 0; i < _items.Length; i++)
            {
                newItems[i] = _items[i];
            }
            _items = newItems;
        }

        public T Peek()
        {
            if (Count > 0)
            {
                return _items[0];
            }
            throw new InvalidOperationException();
        }

        public T RemoveMax()
        {
            if (Count <= 0)
            {
                throw new InvalidOperationException();
            }
            T max = _items[0];
            _items[0] = _items[_count - 1];
            _count--;
            int index = 0;
            while (index < _count)
            {
                // get the left and right child indexes
                int left = (2 * index) + 1;
                int right = (2 * index) + 2;
                // make sure we are still within the heap
                if (left >= _count)
                {
                    break;
                }
                // To avoid having to swap twice, we swap with the largest value.
                // E.g.,
                //      5
                //    6   8
                // 
                // if we swapped with 6 first we'd have
                //
                //      6
                //   5    8
                //
                // And we'd require another swap to get the desired tree
                //
                //     8
                //  6    5
                //
                // So we find the largest child and just do the right thing off the bat
                int maxChildIndex = IndexOfMaxChild(left, right);
                if (_items[index].CompareTo(_items[maxChildIndex]) > 0)
                {
                    // the current item is larger than its children (heap property is satisfied)
                    break;
                }
                Swap(index, maxChildIndex);
                index = maxChildIndex;
            }
            return max;
        }

        private int IndexOfMaxChild(int left, int right)
        {
            // Find the index of the child with the largest value
            int maxChildIndex = -1;
            if (right >= _count)
            {
                // No right child
                maxChildIndex = left;
            }
            else
            {
                if (_items[left].CompareTo(_items[right]) > 0)
                {
                    maxChildIndex = left;
                }
                else
                {
                    maxChildIndex = right;
                }
            }
            return maxChildIndex;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void Clear()
        {
            _count = 0;
            _items = new T[DEFAULT_LENGTH];
        }

        private int Parent(int index)
        {
            return (index - 1) / 2;
        }

        private void Swap(int left, int right)
        {
            T temp = _items[left];
            _items[left] = _items[right];
            _items[right] = temp;
        }
    }

    public class PriorityQueue<T> where T : IComparable<T>
    {
        Heap<T> _heap = new Heap<T>();

        public void Enqueue(T value)
        {
            _heap.Add(value);
        }

        public T Dequeue()
        {
            return _heap.RemoveMax();
        }

        public void Clear()
        {
            _heap.Clear();
        }

        public int Count
        {
            get
            {
                return _heap.Count;
            }
        }
    }

    class Data : IComparable<Data>
    {
        readonly DateTime _creationTime;

        public Data(string message, int priority)
        {
            _creationTime = DateTime.UtcNow;
            Message = message;
            Priority = priority;
        }

        public string Message { get; private set; }

        public int Priority { get; private set; }

        public TimeSpan Age
        {
            get
            {
                return DateTime.UtcNow.Subtract(_creationTime);
            }
        }

        public int CompareTo(Data other)
        {
            int pri = Priority.CompareTo(other.Priority);
            if (pri == 0)
            {
                pri = Age.CompareTo(other.Age);
            }
            return pri;
        }

        public override string ToString()
        {
            return string.Format("[{0} : {1}] {2}",
                Priority,
                Age.Milliseconds,
                Message);
        }

        static void PriorityQueueSample()
        {
            PriorityQueue<Data> queue = new PriorityQueue<Data>();
            queue = new PriorityQueue<Data>();
            Random rng = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int priority = rng.Next() % 3;
                queue.Enqueue(new Data(string.Format("This is message: {0}", i), priority));
                Thread.Sleep(priority);
            }

            while (queue.Count > 0)
            {
                Console.WriteLine(queue.Dequeue().ToString());
            }
        }
    }
    #endregion

    #region AVL Tree

    public class AVLTreeNode<TNode> : IComparable<TNode> where TNode : IComparable
    {
        AVLTree<TNode> _tree;
        AVLTreeNode<TNode> _left;
        AVLTreeNode<TNode> _right;

        public AVLTreeNode(TNode value, AVLTreeNode<TNode> parent, AVLTree<TNode> tree)
        {
            Value = value;
            Parent = parent;
            _tree = tree;
        }

        public AVLTreeNode<TNode> Left
        {
            get
            {
                return _left;
            }
            internal set
            {
                _left = value;
                if (_left != null)
                {
                    _left.Parent = this;
                }
            }
        }

        public AVLTreeNode<TNode> Right
        {
            get
            {
                return _right;
            }
            internal set
            {
                _right = value;
                if (_right != null)
                {
                    _right.Parent = this;
                }
            }
        }

        public AVLTreeNode<TNode> Parent { get; internal set; }

        public TNode Value { get; private set; }

        /// <summary>
        /// Compares the current node to the provided value
        /// </summary>
        /// <param name="other">The node value to compare to</param>
        /// <returns>1 if the instance value is greater than the provided value, -1 if less or 0 if equal.</returns>
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }

        internal void Balance()
        {
            if (State == TreeState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {
                    LeftRightRotation();
                }
                else
                {
                    LeftRotation();
                }
            }
            else if (State == TreeState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {
                    RightLeftRotation();
                }
                else
                {
                    RightRotation();
                }
            }
        }

        private void LeftRotation()
        {
            //     a
            //      \
            //       b
            //        \
            //         c
            //
            // becomes
            //       b
            //      / \
            //     a   c

            AVLTreeNode<TNode> newRoot = Right;

            // replace the current root with the new root
            ReplaceRoot(newRoot);

            // take ownership of right's left child as right (now parent)
            Right = newRoot.Left;

            // the new root takes this as it's left
            newRoot.Left = this;
        }

        private void RightRotation()
        {
            //     c (this)
            //    /
            //   b
            //  /
            // a
            //
            // becomes
            //       b
            //      / \
            //     a   c

            AVLTreeNode<TNode> newRoot = Left;

            // replace the current root with the new root
            ReplaceRoot(newRoot);

            // take ownership of left's right child as left (now parent)
            Left = newRoot.Right;

            // the new root takes this as it's right
            newRoot.Right = this;
        }

        private void LeftRightRotation()
        {
            Right.RightRotation();
            LeftRotation();
        }

        private void RightLeftRotation()
        {
            Left.LeftRotation();
            RightRotation();
        }

        private void ReplaceRoot(AVLTreeNode<TNode> newRoot)
        {
            if (this.Parent != null)
            {
                if (this.Parent.Left == this)
                {
                    this.Parent.Left = newRoot;
                }
                else if (this.Parent.Right == this)
                {
                    this.Parent.Right = newRoot;
                }
            }
            else
            {
                _tree.Head = newRoot;
            }

            newRoot.Parent = this.Parent;
            this.Parent = newRoot;
        }


        private int MaxChildHeight(AVLTreeNode<TNode> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right));
            }

            return 0;
        }

        private int LeftHeight
        {
            get
            {
                return MaxChildHeight(Left);
            }
        }

        private int RightHeight
        {
            get
            {
                return MaxChildHeight(Right);
            }
        }

        private TreeState State
        {
            get
            {
                if (LeftHeight - RightHeight > 1)
                {
                    return TreeState.LeftHeavy;
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return TreeState.RightHeavy;
                }

                return TreeState.Balanced;
            }
        }

        private int BalanceFactor
        {
            get
            {
                return RightHeight - LeftHeight;
            }
        }

        enum TreeState
        {
            Balanced,
            LeftHeavy,
            RightHeavy,
        }

    }


    public class AVLTree<T> : IEnumerable<T> where T : IComparable
    {
        public AVLTreeNode<T> Head { get; internal set; }

        public void Add(T value)
        {
            // Case 1: The tree is empty - allocate the head
            if (Head == null)
            {
                Head = new AVLTreeNode<T>(value, null, this);
            }
            // Case 2: The tree is not empty so find the right location to insert
            else
            {
                AddTo(Head, value);
            }

            Count++;
        }

        // Recursive add algorithm
        private void AddTo(AVLTreeNode<T> node, T value)
        {
            // Case 1: Value is less than the current node value
            if (value.CompareTo(node.Value) < 0)
            {
                // if there is no left child make this the new left
                if (node.Left == null)
                {
                    node.Left = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    // else add it to the left node
                    AddTo(node.Left, value);
                }
            }
            // Case 2: Value is equal to or greater than the current value
            else
            {
                // If there is no right, add it to the right
                if (node.Right == null)
                {
                    node.Right = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    // else add it to the right node
                    AddTo(node.Right, value);
                }
            }

            node.Balance();
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        /// <summary>
        /// Finds and returns the first node containing the specified value. If the value
        /// is not found, returns null. Also returns the parent of the found node (or null)
        /// which is used in Remove.
        /// </summary>
        /// <param name="value">The value to search for</param>
        /// <param name="parent">The parent of the found node (or null)</param>
        /// <returns>The found node (or null)</returns>
        private AVLTreeNode<T> Find(T value)
        {
            // Now, try to find data in the tree
            AVLTreeNode<T> current = Head;

            // while we don't have a match
            while (current != null)
            {
                int result = current.CompareTo(value);

                if (result > 0)
                {
                    // if the value is less than current, go left.
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // if the value is more than current, go right.
                    current = current.Right;
                }
                else
                {
                    // we have a match!
                    break;
                }
            }
            return current;
        }

        /// <summary>
        /// Removes the first occurrence of the specified value from the tree.
        /// </summary>
        /// <param name="value">The value to remove</param>
        /// <returns>True if the value was removed, false otherwise</returns>
        public bool Remove(T value)
        {
            AVLTreeNode<T> current;
            current = Find(value);

            if (current == null)
            {
                return false;
            }

            AVLTreeNode<T> treeToBalance = current.Parent;

            Count--;

            // Case 1: If current has no right child, then current's left replaces current
            if (current.Right == null)
            {
                if (current.Parent == null)
                {
                    Head = current.Left;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // if parent value is greater than current value
                        // make the current left child a left child of parent
                        current.Parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // if parent value is less than current value
                        // make the current left child a right child of parent
                        current.Parent.Right = current.Left;
                    }
                }
            }
            // Case 2: If current's right child has no left child, then current's right child
            //         replaces current
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (current.Parent == null)
                {
                    Head = current.Right;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // if parent value is greater than current value
                        // make the current right child a left child of parent
                        current.Parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // if parent value is less than current value
                        // make the current right child a right child of parent
                        current.Parent.Right = current.Right;
                    }
                }
            }
            // Case 3: If current's right child has a left child, replace current with current's
            //         right child's left-most child
            else
            {
                // find the right's left-most child
                AVLTreeNode<T> leftmost = current.Right.Left;

                while (leftmost.Left != null)
                {
                    leftmost = leftmost.Left;
                }

                // the parent's left subtree becomes the leftmost's right subtree
                leftmost.Parent.Left = leftmost.Right;

                // assign leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;

                leftmost.Right = current.Right;

                if (current.Parent == null)
                {
                    Head = leftmost;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // if parent value is greater than current value
                        // make leftmost the parent's left child
                        current.Parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // if parent value is less than current value
                        // make leftmost the parent's right child
                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (treeToBalance != null)
            {
                treeToBalance.Balance();
            }
            else
            {
                if (Head != null)
                {
                    Head.Balance();
                }
            }

            return true;
        }

        /// <summary>
        /// Enumerates the values contains in the binary tree in in-order traversal order.
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<T> InOrderTraversal()
        {
            // This is a non-recursive algorithm using a stack to demonstrate removing
            // recursion to make using the yield syntax easier.
            if (Head != null)
            {
                // store the nodes we've skipped in this stack (avoids recursion)
                Stack<AVLTreeNode<T>> stack = new Stack<AVLTreeNode<T>>();

                AVLTreeNode<T> current = Head;

                // when removing recursion we need to keep track of whether or not
                // we should be going to the left node or the right nodes next.
                bool goLeftNext = true;

                // start by pushing Head onto the stack
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // If we're heading left...
                    if (goLeftNext)
                    {
                        // push everything but the left-most node to the stack
                        // we'll yield the left-most after this block
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // in-order is left->yield->right
                    yield return current.Value;

                    // if we can go right then do so
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // once we've gone right once, we need to start
                        // going left again.
                        goLeftNext = true;
                    }
                    else
                    {
                        // if we can't go right then we need to pop off the parent node
                        // so we can process it and then go to it's right node
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that performs an in-order traversal of the binary tree
        /// </summary>
        /// <returns>The in-order enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        /// <summary>
        /// Returns an enumerator that performs an in-order traversal of the binary tree
        /// </summary>
        /// <returns>The in-order enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            Head = null;
            Count = 0;
        }


        public int Count { get; private set; }
    }


    #endregion

    #region BTree

    internal class BTreeNode<T> where T : IComparable<T>
    {
        private readonly List<T> _values;
        private readonly List<BTreeNode<T>> _children;

        internal BTreeNode(BTreeNode<T> parent, bool leaf, int minimumDegree, T[] values, BTreeNode<T>[] children)
        {
            ValidatePotentialState(parent, leaf, minimumDegree, values, children);

            Parent = parent;
            Leaf = leaf;
            MinimumDegree = minimumDegree;

            _values = new List<T>(values);
            _children = new List<BTreeNode<T>>(children);
        }

        /// <summary>
        /// Returns true if the node has (2 * T - 1) nodes, false otherwise.
        /// </summary>
        internal bool Full { get; private set; }

        /// <summary>
        /// True if the node is a leaf node, false otherwise.
        /// </summary>
        internal bool Leaf { get; private set; }

        /// <summary>
        /// The node's values
        /// </summary>
        internal IList<T> Values { get; private set; }

        /// <summary>
        /// The node's children
        /// </summary>
        internal IList<BTreeNode<T>> Children { get; private set; }

        /// <summary>
        /// The minimum degree of the node is the minimum degree of the tree. 
        /// If the minimum degree is T then the node must have at least (T-1) 
        /// values but no more than (2*T-1).
        /// </summary>
        internal int MinimumDegree { get; private set; }

        /// <summary>
        /// The parent of the current node (or null if the root node)
        /// </summary>
        internal BTreeNode<T> Parent { get; set; }

        // Validates the ctor parameters 
        private static void ValidatePotentialState(BTreeNode<T> parent, bool leaf, int minimumDegree, T[] values, BTreeNode<T>[] children)
        {
            bool root = parent == null;

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            if (minimumDegree < 2)
            {
                throw new ArgumentOutOfRangeException("minimumDegree", "The minimum degree must be greater than or equal to 2");
            }

            if (values.Length == 0)
            {
                if (children.Length != 0)
                {
                    throw new ArgumentException("An empty node cannot have children");
                }
            }
            else
            {
                if (values.Length > (2 * minimumDegree - 1))
                {
                    throw new ArgumentException("There are too many values");
                }

                if (!root)
                {
                    if (values.Length < minimumDegree - 1)
                    {
                        throw new ArgumentException("Each non-root node must have at least degree - 1 children");
                    }
                }

                if (!leaf && !root)
                {
                    if (values.Length + 1 != children.Length)
                    {
                        throw new ArgumentException("There should be one more child than values");
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        private void ValidateValues()
        {
            if (_values.Count > 1)
            {
                for (int i = 1; i < _values.Count; i++)
                {
                    Debug.Assert(_values[i - 1].CompareTo(_values[i]) < 0);
                }
            }
        }

        /// <summary>
        /// Splits a full child node, pulling the split value into the current node
        /// </summary>
        /// <param name="indexOfChildToSplit">The child to split</param>
        internal void SplitFullChild(int indexOfChildToSplit)
        {
            // Splits a child node by pulling the middle node up from it
            // into the current (parent) node.

            //     [3          9]
            // [1 2] [4 5 6 7 8] [10 11] 
            //
            // splitting [4 5 6 7 8] would pull 6 up to its parent
            //
            //     [3     6     9]
            // [1 2] [4 5] [7 8] [10 11] 
            int medianIndex = Children[indexOfChildToSplit].Values.Count / 2;

            bool isChildLeaf = Children[indexOfChildToSplit].Leaf;

            // get the value 6
            T valueToPullUp = Children[indexOfChildToSplit].Values[medianIndex];

            // build node [4 5]
            BTreeNode<T> newLeftSide = new BTreeNode<T>(this, isChildLeaf, MinimumDegree,
                Children[indexOfChildToSplit].Values.Take(medianIndex).ToArray(),
                Children[indexOfChildToSplit].Children.Take(medianIndex + 1).ToArray());

            // build node [7 8]
            BTreeNode<T> newRightSide = new BTreeNode<T>(this, isChildLeaf, MinimumDegree,
                Children[indexOfChildToSplit].Values.Skip(medianIndex + 1).ToArray(),
                Children[indexOfChildToSplit].Children.Skip(medianIndex + 1).ToArray());

            // add 6 to [3 9] making [3 6 9]
            _values.Insert(indexOfChildToSplit, valueToPullUp);

            // sanity check
            ValidateValues();

            // remove the child that pointed to the old node [4 5 6 7 8]
            _children.RemoveAt(indexOfChildToSplit);

            // add the child pointing to [4 5] and [7 8]
            _children.InsertRange(indexOfChildToSplit, new[] { newLeftSide, newRightSide });
        }

        /// <summary>
        /// Splits the full root node into a new root and two children
        /// </summary>
        /// <returns>The new root node</returns>
        internal BTreeNode<T> SplitFullRootNode()
        {
            // The root of the tree, and in fact the entire tree, is
            //
            // [1 2 3 4 5]
            //
            // So pull out 3 and split the left and right side
            //
            //     [3]
            // [1 2] [4 5]

            // find the index of the value to pull up: 3
            int medianIndex = Values.Count / 2;

            // now get the 3
            T rootValue = Values[medianIndex];

            // Build the new root node (empty)
            BTreeNode<T> result = new BTreeNode<T>(Parent, false, MinimumDegree, new T[0], new BTreeNode<T>[0]);

            // build the left noed [1 2]
            BTreeNode<T> newLeftSide = new BTreeNode<T>(result, Leaf, MinimumDegree,
                Values.Take(medianIndex).ToArray(),
                Children.Take(medianIndex + 1).ToArray());

            // build the right node [4 5]
            BTreeNode<T> newRightSide = new BTreeNode<T>(result, Leaf, MinimumDegree,
                Values.Skip(medianIndex + 1).ToArray(),
                Children.Skip(medianIndex + 1).ToArray());

            // add the 3 to the root node
            result._values.Add(rootValue);

            // Add the left child [1 2]
            result._children.Add(newLeftSide);

            // Add the right child [4 5]
            result._children.Add(newRightSide);

            return result;
        }

        /// <summary>
        /// Insert the specified value into the non-Full leaf node
        /// </summary>
        /// <param name="value">The value to insert</param>
        internal void InsertKeyToLeafNode(T value)
        {
            // Leaf validation is done by caller
            if (!Leaf)
            {
                throw new InvalidOperationException("Unable to insert into a non-leaf node");
            }

            // Non-Full validation done by caller
            if (Full)
            {
                throw new InvalidOperationException("Unable to insert into a full node");
            }

            // Find the index to insert at
            int index = 0;
            while (index < Values.Count && value.CompareTo(Values[index]) > 0)
            {
                index++;
            }

            // Insert
            _values.Insert(index, value);

            // Sanity check
            ValidateValues();
        }

        /// <summary>
        /// If it exists, removes the specified value from the leaf node.
        /// </summary>
        /// <param name="value">The value to remove</param>
        /// <returns>True if a value was removed, false otherwise</returns>
        internal bool DeleteKeyFromLeafNode(T value)
        {
            if (!Leaf)
            {
                throw new InvalidOperationException("Unable to leaf-delete from a non-leaf node");
            }

            return _values.Remove(value);
        }

        /// <summary>
        /// Replaces the value at the specified index with the new value
        /// </summary>
        /// <param name="valueIndex">The index of the value to replace</param>
        /// <param name="newValue">The new value</param>
        internal void ReplaceValue(int valueIndex, T newValue)
        {
            _values[valueIndex] = newValue;
            ValidateValues();
        }

        //     [3     6]
        // [1 2] [4 5] [7 8]
        // becomes
        //           [6]
        // [1 2 3 4 5] [7 8]
        internal BTreeNode<T> PushDown(int valueIndex)
        {
            List<T> values = new List<T>();
            // [1 2] -> [1 2]
            values.AddRange(Children[valueIndex].Values);
            // [3]   -> [1 2 3]
            values.Add(Values[valueIndex]);
            // [4 5] -> [1 2 3 4 5]
            values.AddRange(Children[valueIndex + 1].Values);

            List<BTreeNode<T>> children = new List<BTreeNode<T>>();
            children.AddRange(Children[valueIndex].Children);
            children.AddRange(Children[valueIndex + 1].Children);

            BTreeNode<T> newNode = new BTreeNode<T>(this,
                Children[valueIndex].Leaf,
                MinimumDegree,
                values.ToArray(),
                children.ToArray());

            // [3 6] -> [6]
            _values.RemoveAt(valueIndex);
            // [c1 c2 c3] -> [c2 c3]
            _children.RemoveAt(valueIndex);
            // [c2 c3] -> [newNode c3]
            _children[valueIndex] = newNode;

            return newNode;
        }

        /// <summary>
        /// Adds the specified value to the node and, if the specified node is non-null,
        /// adds the node to the children
        /// </summary>
        internal void AddEnd(T valueToPushDown, BTreeNode<T> bTreeNode)
        {
            _values.Add(valueToPushDown);
            ValidateValues();
            if (bTreeNode != null)
            {
                _children.Add(bTreeNode);
            }
        }

        /// <summary>
        /// Removes the first value and child (if applicable)
        /// </summary>
        internal void RemoveFirst()
        {
            _values.RemoveAt(0);

            if (!Leaf)
            {
                _children.RemoveAt(0);
            }
        }

        /// <summary>
        /// Removes the last value and child (if applicable)
        /// </summary>
        internal void RemoveLast()
        {
            _values.RemoveAt(_values.Count - 1);
            if (!Leaf)
            {
                _children.RemoveAt(_children.Count - 1);
            }
        }

        /// <summary>
        /// Adds the specified value to the front of the values and adds, if non-null,
        /// the specified value to the children
        /// </summary>
        internal void AddFront(T newValue, BTreeNode<T> bTreeNode)
        {
            _values.Insert(0, newValue);
            ValidateValues();
            if (bTreeNode != null)
            {
                _children.Insert(0, bTreeNode);
            }
        }
    }

    public class BTree<T> : ICollection<T> where T : IComparable<T>
    {
        BTreeNode<T> root = null;
        const int MinimumDegree = 2;

        public void Add(T value)
        {
            if (root == null)
            {
                root = new BTreeNode<T>(null, true, MinimumDegree, new[] { value }, new BTreeNode<T>[] { });
            }
            else
            {
                if (root.Full)
                {
                    root = root.SplitFullRootNode();
                }

                InsertNonFull(root, value);
            }

            Count++;
        }

        private void InsertNonFull(BTreeNode<T> node, T value)
        {
            if (node.Leaf)
            {
                node.InsertKeyToLeafNode(value);
            }
            else
            {
                int index = node.Values.Count - 1;
                while (index >= 0 && value.CompareTo(node.Values[index]) < 0)
                {
                    index--;
                }
                index++;
                if (node.Children[index].Full)
                {
                    node.SplitFullChild(index);
                    if (value.CompareTo(node.Values[index]) > 0)
                    {
                        index++;
                    }
                }
                InsertNonFull(node.Children[index], value);
            }
        }

        public bool Remove(T value)
        {
            bool removed = false;

            if (Count > 0)
            {
                removed = RemoveValue(root, value);
                if (removed)
                {
                    Count--;

                    if (Count == 0)
                    {
                        root = null;
                    }
                    else if (root.Values.Count == 0)
                    {
                        root = root.Children[0];
                    }
                }
            }
            return removed;
        }

        internal static bool RemoveValue(BTreeNode<T> node, T value)
        {
            if (node.Leaf)
            {
                // Deleting case 1...

                // By the time we are in a leaf node we have either pushed down
                // values such that the leaf node has minimum degree children
                // and can therefore have one node removed OR the root node is
                // also a leaf node we can freely violate the minimum rule.
                return node.DeleteKeyFromLeafNode(value);
            }

            int valueIndex;
            if (TryGetIndexOf(node, value, out valueIndex))
            {
                // Deletion case 2...

                // We have found the non-leaf node the value is in - since we can only delete values
                // from a leaf node we need to push the value to delete down into a child.

                // If the child that precedes the value to delete (the "left" child) has
                // at least the minimum degree of children ...
                if (node.Children[valueIndex].Values.Count >= node.Children[valueIndex].MinimumDegree)
                {
                    //     [3       6         10]
                    // [1 2]  [4 5]   [7 8 9]    [11 12]

                    // deleting 10

                    // find the largest value in the child node that contains smaller values
                    // than what is being deleted... (this is the value 9)
                    T valuePrime = FindPredecessor(node, valueIndex);

                    // and REPLACE the value to delete with the next largest value (the one
                    // we just found)  (swapping 9 and 10)
                    node.ReplaceValue(valueIndex, valuePrime);

                    // after the swap...

                    //     [3       6         9]
                    // [1 2]  [4 5]   [7 8 9]    [11 12]

                    // notice that 9 is in the tree twice. This is not a typo. We are about 
                    // to delete it from the child we took it from.

                    // delete the value we moved up (9) from the child (this may in turn
                    // push it down to subsequent children until it is in a leaf
                    return RemoveValue(node.Children[valueIndex], valuePrime);

                    // final tree...

                    //     [3       6        9]
                    // [1 2]  [4 5]   [7 8 ]   [11 12]
                }
                else
                {
                    // if the left child did not have enough values to move one of its values up,
                    // check if the right child does
                    if (node.Children[valueIndex + 1].Values.Count >= node.Children[valueIndex + 1].MinimumDegree)
                    {
                        // see the above algorithm and do the opposite...

                        //     [3       6         10]
                        // [1 2]  [4 5]   [7 8 9]    [11 12]

                        // deleting 6

                        // successor = 7
                        T valuePrime = FindSuccessor(node, valueIndex);
                        node.ReplaceValue(valueIndex, valuePrime);

                        // after replace the tree is

                        //     [3       7         10]
                        // [1 2]  [4 5]   [7 8 9]    [11 12]

                        // now remove 7 from the child
                        return RemoveValue(node.Children[valueIndex + 1], valuePrime);

                        // final tree...
                        //     [3       7         10]
                        // [1 2]  [4 5]   [8 9]    [11 12]
                    }
                    else
                    {
                        // If neither child has the minimum degree of children that means they both
                        // have (minimum degree - 1) children. Since a node can have 
                        // (2 * <minimum degree> - 1) children we can safely merge the two nodes
                        // into a single child.

                        //
                        //     [3     6     9]
                        // [1 2] [4 5] [7 8] [10 11]
                        //
                        // deleting 6
                        // 
                        // [4 5] and [7 8] are merged into a single node with [6] pushed down into it
                        //
                        //     [3          9]
                        // [1 2] [4 5 6 7 8] [10 11]
                        //
                        BTreeNode<T> newChildNode = node.PushDown(valueIndex);

                        // now that we've pushed the value down a level we can call remove
                        // on the new child node [4 5 6 7 8]
                        return RemoveValue(newChildNode, value);
                    }
                }
            }
            else
            {
                // Deletion case 3...

                // We are at an internal node which does not contain the value we want to delete.
                // First find the child path that the value we want to delete would be in
                // if it does existing in the tree...
                int childIndex;
                FindPotentialPath(node, value, out valueIndex, out childIndex);

                // now that we know where the value should be we need to ensure that the node
                // we are going to has the minimum number of values necessary to delete from.
                if (node.Children[childIndex].Values.Count == node.Children[childIndex].MinimumDegree - 1)
                {
                    // since the node does not have enough values, what we want to do is borrow
                    // a value from a sibling that has enough values to share.

                    // determine if the left or right sibling has the most children
                    int indexOfMaxSibling = GetIndexOfMaxSibling(childIndex, node);

                    // if a sibling with values exists (maybe we're 
                    // the root node and don't have one)
                    // and that sibling has enough values...
                    if (indexOfMaxSibling >= 0 &&
                        node.Children[indexOfMaxSibling].Values.Count >= node.Children[indexOfMaxSibling].MinimumDegree)
                    {
                        // rotate the appropriate value from the sibling 
                        // through the parent into the current node
                        // so that we have enough values in the current 
                        // node to push a value down into the 
                        // child we are going to check next.

                        //     [3      7]
                        // [1 2] [4 5 6]  [8 9]
                        //
                        // the node we want to travel through is [1 2] but we 
                        // need another node in it. So we rotate the 4 
                        // up to the root and push the 3 down into the [1 2] 
                        // node.
                        //
                        //       [4     7]
                        // [1 2 3] [5 6]  [7 8]
                        RotateAndPushDown(node, childIndex, indexOfMaxSibling);
                    }
                    else
                    {
                        // merge (which may push the only node in the root down - so new root)
                        BTreeNode<T> pushedDownNode = node.PushDown(valueIndex);

                        // now find the node we just pushed down
                        childIndex = 0;
                        while (pushedDownNode != node.Children[childIndex])
                        {
                            childIndex++;
                        }
                    }
                }
                return RemoveValue(node.Children[childIndex], value);
            }
        }

        internal static void RotateAndPushDown(BTreeNode<T> node, int childIndex, int indexOfMaxSibling)
        {
            int valueIndex;
            if (childIndex < indexOfMaxSibling)
            {
                valueIndex = childIndex;
            }
            else
            {
                valueIndex = childIndex - 1;
            }

            if (indexOfMaxSibling > childIndex)
            {
                // we are moving the left-most key from the right sibling into the parent
                // and pushing the parent down into the child 
                //
                //     [6      10]
                //  [1]  [7 8 9] [11]
                //
                // deleting something less than 6
                // 
                //       [7   10]
                //    [1 6] [8 9] [11]

                // grab the 7
                T valueToMoveToX = node.Children[indexOfMaxSibling].Values.First();

                // get 7's left child if it has one (not a leaf)
                BTreeNode<T> childToMoveToNode = node.Children[indexOfMaxSibling].Leaf ? null : node.Children[indexOfMaxSibling].Children.First();

                // record the 6 (the push down value)
                T valueToMoveDown = node.Values[valueIndex];

                // move the 7 into the parent
                node.ReplaceValue(valueIndex, valueToMoveToX);

                // move the 6 into the child
                node.Children[childIndex].AddEnd(valueToMoveDown, childToMoveToNode);

                // remove the first value and child from the sibling now that they've been moved
                node.Children[indexOfMaxSibling].RemoveFirst();
            }
            else
            {
                // we are moving the right-most key from the left sibling into the parent
                // and pushing the parent down into the child 
                //
                //     [6      10]
                //  [1]  [7 8 9] [11]
                //
                // deleting something greater than 10
                // 
                //     [6     9]
                //  [1]  [7 8] [10, 11]

                // grab the 9
                T valueToMoveToX = node.Children[indexOfMaxSibling].Values.Last();

                // get 9's right child if it has one (not a leaf node) 
                BTreeNode<T> childToMoveToNode = node.Children[indexOfMaxSibling].Leaf ? null : node.Children[indexOfMaxSibling].Children.Last();

                // record the 10 (the push down value)
                T valueToMoveDown = node.Values[valueIndex];

                // move the 9 into the parent
                node.ReplaceValue(valueIndex, valueToMoveToX);

                // move the 10 into the child
                node.Children[childIndex].AddFront(valueToMoveDown, childToMoveToNode);

                // remove the last value and child from the sibling now that they've been moved
                node.Children[indexOfMaxSibling].RemoveLast();
            }
        }

        internal static void FindPotentialPath(BTreeNode<T> node, T value, out int valueIndex, out int childIndex)
        {
            // We want to find out which child the value we are searching for (value)
            // would be in if the value were in the tree.
            childIndex = node.Children.Count - 1;
            valueIndex = node.Values.Count - 1;

            // start at the right-most child and value indexes and work
            // backwards until we are less than the value we want.
            while (valueIndex > 0)
            {
                int compare = value.CompareTo(node.Values[valueIndex]);

                if (compare > 0)
                {
                    break;
                }

                childIndex--;
                valueIndex--;
            }

            // if we make it all the way to the last value...
            if (valueIndex == 0)
            {
                // if the value we are searching for is less than the first 
                // value in the node then the child is the 0 index child, 
                // not the 1 index.
                if (value.CompareTo(node.Values[valueIndex]) < 0)
                {
                    childIndex--;
                }
            }
        }

        // Returns the index (to the left or right) of the child node 
        // that has the most values in it.
        //
        // Example
        //
        //     [3      7]
        // [1 2] [4 5 6] [8 9]
        //
        // If we pass in the [3 7] node with index 0, the left child [1 2]
        // and right child [4 5 6] would be checked and the index 1, for child
        // node [4 5 6] would be returned.
        // 
        // If we checked [3 7] with index 1, the left child [4 5 6] and the
        // right child [8 9] would be checked and the value 1 would be returned
        private static int GetIndexOfMaxSibling(int index, BTreeNode<T> node)
        {
            int indexOfMaxSibling = -1;

            BTreeNode<T> leftSibling = null;
            if (index > 0)
            {
                leftSibling = node.Children[index - 1];
            }

            BTreeNode<T> rightSibling = null;
            if (index + 1 < node.Children.Count)
            {
                rightSibling = node.Children[index + 1];
            }

            if (leftSibling != null || rightSibling != null)
            {
                if (leftSibling != null && rightSibling != null)
                {
                    indexOfMaxSibling = leftSibling.Values.Count > rightSibling.Values.Count ?
                        index - 1 : index + 1;
                }
                else
                {
                    indexOfMaxSibling = leftSibling != null ? index - 1 : index + 1;
                }
            }
            return indexOfMaxSibling;
        }

        // Gets the index of the specified value from the current node's values
        // returning true if the value was found, false otherwise.
        private static bool TryGetIndexOf(BTreeNode<T> node, T value, out int valueIndex)
        {
            for (int index = 0; index < node.Values.Count; index++)
            {
                if (value.CompareTo(node.Values[index]) == 0)
                {
                    valueIndex = index;
                    return true;
                }
            }

            valueIndex = -1;
            return false;
        }

        // Finds the value of the predecessor value of a specific value in a node
        //
        // Example
        //
        //     [3     6]
        // [1 2] [4 5] [7 8]
        //
        // the predecessor of 3 is 2.
        private static T FindPredecessor(BTreeNode<T> node, int index)
        {
            node = node.Children[index];

            while (!node.Leaf)
            {
                node = node.Children.Last();
            }

            return node.Values.Last();
        }

        // Finds the value of the successor of a specific value in a node
        //
        // Example
        //
        //     [3     6]
        // [1 2] [4 5] [7 8]
        //
        // the successor of 3 is 4.
        private static T FindSuccessor(BTreeNode<T> node, int index)
        {
            node = node.Children[index + 1];

            while (!node.Leaf)
            {
                node = node.Children.First();
            }

            return node.Values.First();
        }

        public bool Contains(T value)
        {
            BTreeNode<T> node;
            int valueIndex;
            return TryFindNodeContainingValue(value, out node, out valueIndex);
        }

        // searches the node, and its children, looking for the specified value.
        internal bool TryFindNodeContainingValue(T value, out BTreeNode<T> node, out int valueIndex)
        {
            BTreeNode<T> current = root;

            // if the current node is null then we never found the value
            // otherwise we still have hope.
            while (current != null)
            {
                int index = 0;
                // check each value in the node
                while (index < current.Values.Count)
                {
                    int compare = value.CompareTo(current.Values[index]);

                    // did we find it?
                    if (compare == 0)
                    {
                        // awesome!
                        node = current;
                        valueIndex = index;
                        return true;
                    }
                    // if the value to find is less than the current node's value
                    // then we want to go left (which is where we are ...)
                    if (compare < 0)
                    {
                        break;
                    }

                    // otherwise move on to the next value in the node
                    index++;
                }
                if (current.Leaf)
                {
                    // if we are at a leaf node there is no child to go
                    // down to
                    break;
                }
                else
                {
                    // otherwise go into the child we determined must contain the 
                    // value we want to find
                    current = current.Children[index];
                }
            }

            node = null;
            valueIndex = -1;
            return false;
        }

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T value in InOrderEnumerator(root))
            {
                array[arrayIndex++] = value;
            }
        }

        public int Count { get; private set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderEnumerator(root).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> InOrderEnumerator(BTreeNode<T> node)
        {
            if (node != null)
            {
                if (node.Leaf)
                {
                    foreach (T value in node.Values)
                    {
                        yield return value;
                    }
                }
                else
                {
                    IEnumerator<BTreeNode<T>> children = node.Children.GetEnumerator();
                    IEnumerator<T> values = node.Values.GetEnumerator();

                    while (children.MoveNext())
                    {
                        foreach (T childValue in InOrderEnumerator(children.Current))
                        {
                            yield return childValue;
                        }

                        if (values.MoveNext())
                        {
                            yield return values.Current;
                        }
                    }
                }
            }
        }
    }

    #endregion
}



