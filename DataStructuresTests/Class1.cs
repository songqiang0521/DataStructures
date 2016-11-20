using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS_Library;
using Xunit;

namespace DataStructuresTests
{
    public class Class1
    {
        [Fact]
        private void LinkedList_Add()
        {
            ADS_LinkedList<int> list = new ADS_LinkedList<int>();
            list.Add(3);
            list.Add(5);
            list.Add(7);

            var firstNode = list.Head;
            Assert.True(firstNode.Value == 3);

            var midNode = list.Head.Next;
            Assert.True(midNode.Value == 5);

            var lastNode = list.Tail;
            Assert.True(lastNode == midNode.Next);
            Assert.True(lastNode.Value == 7);

        }


        [Fact]
        private void LinkedList_Remove()
        {
            ADS_LinkedList<int> list = new ADS_LinkedList<int>();
            list.Add(3);
            list.Add(5);
            list.Add(7);

            list.RemoveFirst();
            list.RemoveLast();

            Assert.True(list.Head == list.Tail);
            Assert.True(list.Head.Value == 5);
        }
    }


}
