namespace KutuphaneProject.Models.MesajList
{
    public class PagedListDto<T>
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public List<T> Data { get; set; }
    }
}
