using System;

namespace AssignmentsAccessor
{
    public class StudentAssignment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Group { get; set; }
        public DateTime SubmissionTime { get; set; }
        public string Status { get; set; }
        public int Grade { get; set; }
        public int ProgressValue { get; set; }
        public bool IsCheated { get; set; }
        public float Budget { get; set; }
        public float Consumed { get; set; }
    }
}