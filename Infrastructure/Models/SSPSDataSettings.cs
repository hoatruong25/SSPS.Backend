namespace Infrastructure.Models
{
    public class SSPSDataSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string ForgotPasswordCollectionName { get; set; } = null!;
        public string MoneyPlanCollectionName { get; set; } = null!;
        public string NoteCollectionName { get; set; } = null!;
        public string ToDoNoteCollectionName { get; set; } = null!;

    }
}
