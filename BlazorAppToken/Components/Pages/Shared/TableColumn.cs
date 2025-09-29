namespace BalzorAppVlan.Components.Pages.Shared
{
    public class TableColumn<TItem>
    {
        public string Header { get; set; }
        public Func<TItem, object> Cell { get; set; }
    }
}
