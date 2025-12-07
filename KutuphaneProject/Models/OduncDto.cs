namespace KutuphaneProject.Models
{
    public class OduncDto
    {
        public int OduncId { get; set; }

        public string OduncAdi { get; set; }

        public int KalanSure { get; set; } = 20;

        public int BorcMiktari { get; set; } = 0;

        public byte[] Image { get; set; }

        public string ImageSrc
        {
            get
            {
                if (Image != null)
                {
                    string base64String = Convert.ToBase64String(Image, 0, Image.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
