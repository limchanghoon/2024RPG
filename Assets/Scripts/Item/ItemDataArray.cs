[System.Serializable]
public class ItemDataArray<T> where T : ItemData, new()
{
    public T[] itemDatas;

    public ItemDataArray(int sz)
    {
        itemDatas = new T[sz];
        for (int i = 0; i < sz; i++)
        {
            itemDatas[i] = new T();
        }
    }

    public T[] ToArray()
    {
        T[] des = new T[itemDatas.Length];
        for (int i = 0; i < des.Length; i++)
        {
            des[i] = itemDatas[i];
        }
        return des;
    }

    public ItemDataArray(T[] arr)
    {
        // 값 복사냐 레퍼런스 복사냐 나중에 확정시키자!!!!
        itemDatas = arr;
    }
}