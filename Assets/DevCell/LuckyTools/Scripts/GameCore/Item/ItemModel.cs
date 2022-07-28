namespace GameCore.Item
{
    public class ItemModel
    {
        private ItemDescription _description;

        public ItemDescription Description => _description;

        public ItemModel(ItemDescription description)
        {
            _description = description;
        }
    }
}
