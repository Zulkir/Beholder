/*
Copyright (c) 2010-2013 Beholder Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Beholder.Utility.Collections
{
    public class OneWayLinkedList<T> : IEnumerable<OneWayLinkedListNode<T>>
    {
        /// <summary>
        /// Virtual node that always stands before the first one.
        /// </summary>
        public OneWayLinkedListNode<T> Dummy { get; private set; }
        public OneWayLinkedListNode<T> First { get { return Dummy.Next; } }
        public OneWayLinkedListNode<T> Last { get; private set; }
        public int Count { get; private set; }

        public OneWayLinkedList()
        {
            Dummy = new OneWayLinkedListNode<T>(default(T));
        }

        public bool Contains(T item, Func<T, T, bool> equals)
        {
            var node = Dummy.Next;
            while (node != null)
            {
                if (equals(node.Item, item)) return true;
                node = node.Next;
            }

            return false;
        }

        public void AddFirst(T item) { AddFirst(new OneWayLinkedListNode<T>(item)); }
        public void AddFirst(OneWayLinkedListNode<T> node)
        {
            node.Next = Dummy.Next;
            Dummy.Next = node;
            Count++;
        }

        public void AddLast(T item) { AddLast(new OneWayLinkedListNode<T>(item)); }
        public void AddLast(OneWayLinkedListNode<T> node)
        {
            Last.Next = node;
            Last = node;
            node.Next = null;
            Count++;
        }

        public void AddAfter(OneWayLinkedListNode<T> node, T newItem) { AddAfter(node, new OneWayLinkedListNode<T>(newItem)); }
        public void AddAfter(OneWayLinkedListNode<T> node, OneWayLinkedListNode<T> newNode)
        {
            if (node == Last)
            {
                AddLast(newNode);
            }
            else
            {
                newNode.Next = node.Next;
                node.Next = newNode;
                Count++;
            }
        }

        public void RemoveFirst()
        {
            if (Dummy.Next == null) 
                throw new InvalidOperationException("Trying to remove a first node from an empty OneWayLinkedList.");
            Dummy.Next = Dummy.Next.Next;
            Count--;
            if (Dummy.Next == null) 
                Last = null;
        }

        public void RemoveAfter(OneWayLinkedListNode<T> node)
        {
            if (node.Next == null) 
                throw new InvalidOperationException("Trying to remove a OneWayLinkedListNode that comes after the last one.");
            node.Next = node.Next.Next;
            Count--;
            if (node.Next == null) 
                Last = node;
        }

        public void RemoveEverythingAfter(OneWayLinkedListNode<T> node)
        {
            int removedCount = 0;
            OneWayLinkedListNode<T> currentNode = node;
            while (currentNode.Next != null)
            {
                currentNode.Next = currentNode.Next.Next;
                removedCount++;
            }

            node.Next = null;
            Count -= removedCount;

            Last = node;
        }

        public void Clear()
        {
            Dummy.Next = Last = null;
            Count = 0;
        }

        public IEnumerable<T> Values
        {
            get
            {
                var node = Dummy.Next;
                while (node != null)
                {
                    yield return node.Item;
                    node = node.Next;
                }
            }
        }

        public IEnumerable<OneWayLinkedListNode<T>> NodesWithDummy
        {
            get
            {
                var node = Dummy;
                while (node != null)
                {
                    yield return node;
                    node = node.Next;
                } 
            }
        }

        public IEnumerator<OneWayLinkedListNode<T>> GetEnumerator()
        {
            var node = Dummy.Next;
            while (node != null)
            {
                yield return node;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
