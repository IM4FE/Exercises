namespace SwaggerWebApi.Models
{
    public class LowerBaseResponse
    {
        public string? input { get; set; }
        public string? result { get; set; }
        public NumLetters[] numLetters { get; set; }
        public string? vowelRegion { get; set; }
        public Sort sortResult { get; set; }
        public string? randomSpaceString { get; set; }
    }
}