namespace Project_ServerSide.Models.Algorithm
{
    public class StudentTagJSON
    {
        int studentId;
        int tagId;
        double tagCount;

        public int StudentId { get => studentId; set => studentId = value; }
        public int TagId { get => tagId; set => tagId = value; }
        public double TagCount { get => tagCount; set => tagCount = value; }
    }
}
