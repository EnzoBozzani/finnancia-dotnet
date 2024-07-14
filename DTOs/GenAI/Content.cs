namespace FinnanciaCSharp.DTOs.GenAI
{
    public class Content
    {
        public string role { get; set; } = string.Empty;
        public List<Part> parts { get; set; } = new List<Part>();
    }
}