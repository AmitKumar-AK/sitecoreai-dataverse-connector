namespace SitecoreAI.Dataverse.Model
{
    public class Department
    {
        public string? Id { get; set; }
        public string? DepartmentName { get; set; }
    }

    public class DepartmentsRoot
    {
        public List<DepartmentDto>? departments { get; set; }
    }

    public class DepartmentDto
    {
        public string? id { get; set; }
        public string? department { get; set; }
    }
}
