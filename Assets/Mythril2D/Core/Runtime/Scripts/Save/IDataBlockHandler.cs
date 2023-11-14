namespace Gyvr.Mythril2D
{
    public interface IDataBlockHandler<T>
    {
        public void LoadDataBlock(T block);
        public T CreateDataBlock();
    }
}
