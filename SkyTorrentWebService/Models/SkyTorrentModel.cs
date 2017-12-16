namespace SkyTorrentWebService.Models
{
    public class SkyTorrentModel
    {
        public string Name { get; set; }
        public string UploadedDate { get; set; }
        public string Size { get; set; }
        public int Seed { get; set; }
        public int Peers { get; set; }
        public string Download { get; set; }
        public string Magnet { get; set; }
        public bool Verified { get; set; }
    }
}
