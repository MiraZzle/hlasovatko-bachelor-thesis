namespace server.Models.Activities
{
    /// <summary>
    /// Interface representing a basic activity structure.
    /// </summary>
    public interface IActivity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ActivityType { get; set; }
        public string Definition { get; set; }
        public List<string> Tags { get; set; }
    }
}
