using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ListComparitor
{
    public class ListComparitor<T>
    {
        private IEnumerable<T> oldList;
        private IEnumerable<T> newList;

        public IEnumerable<T> AddedItems => newList.Except(oldList);
        public IEnumerable<T> RemovedItems => oldList.Except(newList);
        public IEnumerable<T> ExistingItems => newList.Intersect(oldList);

        public ListComparitor(IEnumerable<T> oldList, IEnumerable<T> newList)
        {
            this.oldList = oldList;
            this.newList = newList;
        }
    }
}
