
using Benchmarks.Core.Collections;
using Core.Collections.Lists;

void MyCachedList128()
{
    Work.ReferenceTypeMatcher matcher = new();
    Work.InsertFindRemove(
        1000,
        createList: () => new List<int, Work.ReferenceType<int>>(matcher),
        addElement: (list, key) => { list.Insert(key); },
        removeElement: (list, key) => { list.Remove(key); },
        findElement: (list, key) => list.Find(key, out Work.ReferenceType<int>? value) ? value : default!,
        getCount: list => list.Count);
}

MyCachedList128();
